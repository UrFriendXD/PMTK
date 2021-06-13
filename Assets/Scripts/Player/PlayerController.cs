using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private Rigidbody2D payloadBody;
        [SerializeField] private float windForce;
        [Header("Release")] 
        [SerializeField] private float releaseVelocityMultiplier = 1f;
        [SerializeField] private float releaseForceUpdate = 40f;
        [SerializeField] private float releaseImpulseForce = 0f;
        [SerializeField] private float releaseTime = 1.5f;
        [SerializeField] private float releaseWindForce = 10f;
        [Header("Catch")] 
        [SerializeField] private float catchDistance = Mathf.Infinity;

        [SerializeField] private PayloadAnimationController _payloadAnimationController;
        [SerializeField] private CatchIndicator catchIndicator;
        
        private Rigidbody2D rb;
        private bool hasMotorInput;
        private float moveValue;
        private float startDistance;
        private float currentReleaseTime;
        private ProceduralRope proceduralRope;
        private DistanceJoint2D mainJoint;

        private PlayerAnimationController _playerAnimationController;
        
        public bool IsReleased => mainJoint.connectedBody == null;

        public float MoveValue => moveValue;

        public Rigidbody2D PayloadBody => payloadBody;

        public float StartDistance => startDistance;

        public Vector2 PayloadVector => payloadBody.position - rb.position;
        
        public float PayloadDistance => Vector2.Distance(payloadBody.position, rb.position);

        public Rigidbody2D AttachedRigidbody => rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            startDistance = PayloadDistance;

            _playerAnimationController = GetComponent<PlayerAnimationController>();
            proceduralRope = GetComponent<ProceduralRope>();
            mainJoint = GetComponent<DistanceJoint2D>();
            //Debug.Log(_playerAnimationController);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveValue = context.ReadValue<float>();

            if (context.started)
            {
                hasMotorInput = true;
                _playerAnimationController.UpdateMoveAnimation(moveValue);
            }

            if (context.canceled)
            {
                hasMotorInput = false;
                _playerAnimationController.UpdateMoveAnimation(0);
            }
        }

        public void OnTether(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Debug.Log("Tether" + gameObject.name);
                // When payload is still attached and we're about to release
                if (!IsReleased)
                {
                    proceduralRope.ClearJoints();
                    payloadBody.velocity *= releaseVelocityMultiplier;
                    payloadBody.AddForce(Vector2.right * releaseImpulseForce, ForceMode2D.Impulse);
                    _playerAnimationController.Fling();
                    _payloadAnimationController.Disconnect();
                }
                // When about to catch the payload
                else if (PayloadDistance < catchDistance)
                {
                    currentReleaseTime = 0f;
                    proceduralRope.GenerateJoints();
                    _payloadAnimationController.Connect();
                }
            }
        }

        public void OnRestart(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameManager.Instance.RestartGame();
            }
        }

        private void FixedUpdate()
        {
            if (hasMotorInput)
            {
                Vector2 nextPos = rb.position;
                nextPos.y += moveValue * moveSpeed * Time.fixedDeltaTime;
                nextPos.y = Mathf.Clamp(nextPos.y, -7, 6);
                rb.MovePosition(nextPos);
            }

            if (IsReleased)
            {
                if (currentReleaseTime < releaseTime)
                {
                    payloadBody.AddForce(new Vector2(releaseForceUpdate, 0f));
                }
                else
                {
                    payloadBody.AddForce(new Vector2(-releaseWindForce, 0f));
                }

                currentReleaseTime += Time.fixedDeltaTime;
            }
            else
            {
                payloadBody.AddForce(new Vector2(-windForce, 0f));
            }
        }

        private void Update()
        {
            if (IsReleased && PayloadDistance < catchDistance)
                catchIndicator.Show();
            else
                catchIndicator.Hide();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerObstacle"))
            {
                GameManager.Instance.LoseLife();
            }
        }
    }
}