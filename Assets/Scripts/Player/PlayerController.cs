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
        [SerializeField] private float ropeReleaseImpulseForce;
        [SerializeField] private float ropeDelay = 0.2f;
        [SerializeField] private float releaseCooldown = 0.2f;
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
        private float startPayloadDistance;
        private float currentReleaseTime;
        private ProceduralRope proceduralRope;
        private DistanceJoint2D mainJoint;
        private float currentReleaseCooldown;

        private PlayerAnimationController _playerAnimationController;
        
        public bool IsReleased => mainJoint.connectedBody == null;

        public bool CanRelease => currentReleaseCooldown <= 0f;
        
        public bool IsPartiallyReleased { get; private set; }

        public float MoveValue => moveValue;

        public Rigidbody2D PayloadBody => payloadBody;

        public float StartPayloadDistance => startPayloadDistance;

        public Vector2 PayloadVector => payloadBody.position - rb.position;
        
        public float PayloadDistance => Vector2.Distance(payloadBody.position, rb.position);

        public Rigidbody2D AttachedRigidbody => rb;
        
        public static PlayerController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of player!");
            }
            
            Instance = this;
            rb = GetComponent<Rigidbody2D>();

            startPayloadDistance = PayloadDistance;

            _playerAnimationController = GetComponent<PlayerAnimationController>();
            proceduralRope = GetComponent<ProceduralRope>();
            mainJoint = GetComponent<DistanceJoint2D>();
            //Debug.Log(_playerAnimationController);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.GameActive) return;
            
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
            if (!GameManager.Instance.GameActive) return;
            
            if (context.started && CanRelease)
            {
                // When payload is still attached and we're about to release
                if (!IsReleased)
                {
                    Debug.Log("Released with cooldown " + currentReleaseCooldown);
                    UnJoinPayload();
                    currentReleaseCooldown = releaseCooldown;
                    payloadBody.velocity *= releaseVelocityMultiplier;
                    payloadBody.AddForce(Vector2.right * releaseImpulseForce, ForceMode2D.Impulse);
                    _playerAnimationController.Fling();
                }
                // When about to catch the payload
                else if (PayloadDistance < catchDistance)
                {
                    JoinPayload();
                }
            }
        }

        /// Logic for detaching payload
        private void UnJoinPayload()
        {
            StartCoroutine(proceduralRope.DoDelayedClearJoints(ropeDelay, Vector2.right * ropeReleaseImpulseForce));
            _payloadAnimationController.Disconnect();
            IsPartiallyReleased = true;
        }

        /// Logic for joining payload
        private void JoinPayload()
        {
            currentReleaseTime = 0f;
            proceduralRope.GenerateJoints();
            _payloadAnimationController.Connect();
            IsPartiallyReleased = false;
        }

        public void Respawn()
        {
            transform.position = Vector3.zero;
            proceduralRope.ClearJoints();
            payloadBody.position = Vector2.left * startPayloadDistance;
            JoinPayload();
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

            currentReleaseCooldown = Mathf.Max(0, currentReleaseCooldown - Time.fixedDeltaTime);
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