using Player;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace ML
{
    public class WitchRayAgent : Agent
    {
        private Rigidbody2D rb;
        private Rigidbody2D plRb;
        private Vector3 startPos;
        public float speed = 12;
        public GameObject payload;
        private float throwTimer;
        public bool done;
        private PlayerController playerController;
    
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            plRb = payload.GetComponent<Rigidbody2D>();
            startPos = transform.localPosition;
            playerController = GetComponent<PlayerController>();
        }

        public override void OnEpisodeBegin()
        {
            // Reset agent
            rb.velocity = Vector2.zero;
            plRb.velocity = Vector2.zero;
            transform.localPosition = startPos;
            throwTimer = 0f;
            done = false;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(rb.velocity);
            sensor.AddObservation(plRb.velocity);
            sensor.AddObservation(gameObject.transform.position);
            sensor.AddObservation(payload.transform.position);
            sensor.AddObservation(Vector3.Distance(payload.transform.position, gameObject.transform.position));
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            var actionY = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
            Vector2 nextPos = rb.position;
            nextPos.y += actionY * speed * Time.fixedDeltaTime;
            nextPos.y = Mathf.Clamp(nextPos.y, -7, 6);
            rb.MovePosition(nextPos);

            if (actionBuffers.DiscreteActions[0] == 1)
            {
                if (throwTimer <= 0)
                {
                    playerController.OnTetherML();
                    throwTimer = 0.2f;
                }
            }

            throwTimer -= Time.deltaTime;

            switch (done)
            {
                case false:
                    SetReward(0.1f);
                    break;
                case true:
                    SetReward(-1.0f);
                    EndEpisode();
                    break;
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Vertical");
        
            var discreteActionsOut = actionsOut.DiscreteActions;
            //Space
            discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
        }
    }
}