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
        [SerializeField] private DistanceJoint2D payloadJoint;
        [Header("Release")] 
        [SerializeField] private float releaseVelocityMultiplier = 1f;
        [SerializeField] private float releaseForceUpdate = 40f;
        [SerializeField] private float releaseImpulseForce = 0f;
        [SerializeField] private float releaseTime = 1.5f;
        [SerializeField] private float releaseWindForce = 10f;
        [Header("Catch")] 
        [SerializeField] private float catchDistance = Mathf.Infinity;
        
        private Rigidbody2D rb;
        private bool hasMotorInput;
        private float moveValue;
        private float startDistance;
        private float currentReleaseTime;
        private float payloadJointStartDistance;

        private PlayerAnimationController _playerAnimationController;
        public bool IsReleased => payloadJoint.connectedBody == null;

        public float MoveValue => moveValue;

        public Rigidbody2D PayloadBody => payloadBody;

        public float StartDistance => startDistance;

        public Vector2 PayloadVector => payloadBody.position - rb.position;
        
        public float PayloadDistance => Vector2.Distance(payloadBody.position, rb.position);

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            startDistance = PayloadDistance;
            payloadJointStartDistance = payloadJoint.distance;

            _playerAnimationController = GetComponent<PlayerAnimationController>();
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
                if (!IsReleased)
                {
                    payloadJoint.connectedBody = null;
                    payloadBody.velocity *= releaseVelocityMultiplier;
                    payloadBody.AddForce(Vector2.right * releaseImpulseForce, ForceMode2D.Impulse);
                }
                else if (PayloadDistance < catchDistance)
                {
                    payloadJoint.connectedBody = payloadBody;
                    payloadJoint.distance = payloadJointStartDistance;
                    currentReleaseTime = 0f;
                }
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerObstacle"))
            {
                GameManager.Instance.LoseLife();
            }
        }
    }
}