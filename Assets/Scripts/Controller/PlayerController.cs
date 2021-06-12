using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [Header("Release")]
        [SerializeField] private float releaseForce = 40f;
        [SerializeField] private float releaseTime = 1.5f;
        [SerializeField] private float releaseWindForce = 10f;
        
        private Rigidbody2D rb;
        private DistanceJoint2D joint;
        private Rigidbody2D payloadBody;
        private bool hasMotorInput;
        private float moveValue;
        private float startDistance;
        private float currentReleaseTime;
        private LineRenderer lineRenderer;

        private bool IsReleased => joint.connectedBody == null;

        public float MoveValue => moveValue;

        public Rigidbody2D PayloadBody => payloadBody;

        public float StartDistance => startDistance;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            joint = GetComponent<DistanceJoint2D>();
            payloadBody = joint.connectedBody.GetComponent<Rigidbody2D>();
            lineRenderer = GetComponent<LineRenderer>();

            startDistance = joint.distance;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveValue = context.ReadValue<float>();

            if (context.started)
                hasMotorInput = true;

            if (context.canceled)
                hasMotorInput = false;
        }

        public void OnTether(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (joint.connectedBody)
                {
                    joint.connectedBody = null;
                }
                else
                {
                    joint.connectedBody = payloadBody;
                    joint.distance = startDistance;
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
                rb.MovePosition(nextPos);
            }

            if (IsReleased)
            {
                if (currentReleaseTime < releaseTime)
                {
                    payloadBody.AddForce(new Vector2(releaseForce, 0f));
                }
                else
                {
                    payloadBody.AddForce(new Vector2(-releaseWindForce, 0f));
                }

                currentReleaseTime += Time.fixedDeltaTime;
            }
            else
            {
                payloadBody.AddForce(new Vector2(-GameManager.Instance.WindForce, 0f));
            }
        }
    }
}