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
        
        private Rigidbody2D rb;
        private HingeJoint2D hinge;
        private Rigidbody2D payloadBody;
        private float currentMotor;
        private Vector2 startHingePos;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            hinge = GetComponent<HingeJoint2D>();
            payloadBody = hinge.gameObject.GetComponent<Rigidbody2D>();

            startHingePos = payloadBody.position;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var moveValue = -context.ReadValue<float>();

            currentMotor += moveValue * hingePower;
        }

        private void FixedUpdate()
        {
            Vector2 payloadOffsetVector = startHingePos - payloadBody.position;
            float payloadOffset = payloadOffsetVector.magnitude;

            currentMotor += payloadOffset;

            hinge.motor = new JointMotor2D()
            {
                motorSpeed = currentMotor,
                maxMotorTorque = float.MaxValue
            };
        }
    }
}