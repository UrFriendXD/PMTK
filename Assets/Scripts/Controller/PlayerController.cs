using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Hinge stuff")]
        [SerializeField] private float hingePower = 50f;
        [SerializeField] private float returnGravity = 0.1f;
        [SerializeField] private float decceleration = 0.1f;
        
        private Rigidbody2D rb;
        private HingeJoint2D hinge;
        private Rigidbody2D payloadBody;
        private float currentMotor;
        private bool hasMotorInput;
        private Vector2 startHingePos;
        private float startDistance;
        private float moveValue;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            hinge = GetComponent<HingeJoint2D>();
            payloadBody = hinge.connectedBody.GetComponent<Rigidbody2D>();

            startHingePos = payloadBody.position;
            startDistance = Vector2.Distance(startHingePos, rb.position);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveValue = -context.ReadValue<float>();

            if (context.started)
                hasMotorInput = true;

            if (context.canceled)
                hasMotorInput = false;
        }

        public void OnTether(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (hinge.connectedBody)
                {
                    hinge.connectedBody = null;
                }
            }
        }

        private void FixedUpdate()
        {
            if (hasMotorInput)
            {
                currentMotor += moveValue * hingePower;
            }
            
            if (false && !hasMotorInput)
            {
                // float predictedDistance = hinge.jointSpeed * Time.fixedDeltaTime;
                // float leftAngle = hinge.jointAngle;
                // float leftDistance = Mathf.Tan(leftAngle) * startDistance;
                //
                // float gravity = (leftDistance - predictedDistance) * returnGravity;
                // currentMotor += gravity / Time.fixedDeltaTime;
                // Debug.Log($"current motor {currentMotor} gravity {gravity / Time.fixedDeltaTime}");
                
                float predictedNextDistance = hinge.jointSpeed * Time.fixedDeltaTime;
                float predictedNextAngle = Mathf.Atan(predictedNextDistance / startDistance);
                float leftAngle = hinge.jointAngle;
                float leftDistance = Mathf.Tan(leftAngle) * startDistance;

                float gravity = -Mathf.Lerp(leftAngle, 0, 0.001f);
                currentMotor += gravity;
                Debug.Log($"current motor {currentMotor} gravity {gravity}");
            }

            hinge.motor = new JointMotor2D()
            {
                motorSpeed = currentMotor * Time.fixedDeltaTime,
                maxMotorTorque = float.MaxValue
            };

            currentMotor = Mathf.Lerp(currentMotor, 0, decceleration);
        }
    }
}