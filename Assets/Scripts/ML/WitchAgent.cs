using Player;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace ML
{
    public class WitchAgent : Agent
    {
        private Rigidbody2D rb;
        private Vector3 startPos;
        public float speed = 20;
        public GameObject payload;
        private float timer;
        private float throwTimer;
        public bool done;
        private PlayerController playerController;
    
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            startPos = transform.localPosition;
            playerController = GetComponent<PlayerController>();
        }

        public override void OnEpisodeBegin()
        {
            // Reset agent
            //rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            transform.localPosition = startPos;
            timer = 0;
            done = false;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            float topHit = 100;
            float bottomHit = 100;
            float rightHit = 100;
            float leftHit = 100;

            float r = 10;
            float rcR = 5;
            var objTransform = payload.transform;
            var mask = LayerMask.GetMask("Obstacle");
            // //put cat on "Ignore Raycast" layer so these don't count it as a hit
            // RaycastHit2D hit = Physics2D.CircleCast(objTransform.position, r, Vector2.up, 50.0f);
            //
            // Debug.DrawRay(objTransform.position, Vector2.right * 50, Color.red);
            // Debug.DrawRay(objTransform.position, Vector2.up * 50, Color.red);
            // Debug.DrawRay(objTransform.position, Vector2.right * -50, Color.red);
            // Debug.DrawRay(objTransform.position, Vector2.up * -50, Color.red);
            //
            // if (hit.collider != null)
            // {
            //     topHit = hit.distance;
            // }
            //
            // hit = Physics2D.CircleCast(objTransform.position, r, -Vector2.up, 50.0f);
            // if (hit.collider != null)
            // {
            //     bottomHit = hit.distance;
            // }
            //
            RaycastHit2D hit = Physics2D.CircleCast(objTransform.position, rcR, Vector2.right, 50.0f, mask);
            if (hit.collider != null)
            {
                rightHit = hit.distance;
            }
            //
            // hit = Physics2D.CircleCast(objTransform.position, r, -Vector2.right, 50.0f);
            // if (hit.collider != null)
            // {
            //     leftHit = hit.distance;
            // }
        
        
            var objects = Physics2D.OverlapCircleAll(objTransform.position, r, mask, -10, 10);
            //Debug.DrawCircle(objTransform.position, Vector2.up * -50, Color.red);
            // Gizmos.color = Color.red;
            // Gizmos.DrawSphere(objTransform.position, r);
            Debug.DrawRay(objTransform.position, Vector2.right * r, Color.red);
            Debug.DrawRay(objTransform.position, Vector2.up * r, Color.red);
            Debug.DrawRay(objTransform.position, Vector2.right * -r, Color.red);
            Debug.DrawRay(objTransform.position, Vector2.up * -r, Color.red);
        
        
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(payload.transform.position);
            sensor.AddObservation(rb.velocity);
            // sensor.AddObservation(topHit);
            // sensor.AddObservation(bottomHit);
            sensor.AddObservation(rightHit);
            // sensor.AddObservation(leftHit);
            foreach (var t in objects)
            {
                sensor.AddObservation(t.gameObject.transform.position);
            }
        
            //PLay pos & distance 
            sensor.AddObservation(gameObject.transform.position);
            sensor.AddObservation(Vector3.Distance(objTransform.position, gameObject.transform.position));
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // Vector3 controlSignal = Vector3.zero;
            // controlSignal.x = vectorAction[0];
            //
            // if (vectorAction[1] == 2)
            // {
            //     controlSignal.z = 1;
            // }
            // else
            // {
            //     controlSignal.z = -vectorAction[1];
            // }
            //
            // if (transform.localPosition.x < 8.5)
            // {
            //     rb.AddForce(controlSignal * speed);
            // }
            //rb.AddForce(this.transform.up * speed * vectorAction[0]);
            //rb.AddForce(this.transform.up * -speed * vectorAction[1]);
            var actionY = 2f * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
            Vector2 nextPos = rb.position;
            nextPos.y += actionY * speed * Time.fixedDeltaTime;
            nextPos.y = Mathf.Clamp(nextPos.y, -7, 6);
            rb.MovePosition(nextPos);

            if (actionBuffers.DiscreteActions[0] == 1)
            {
                if (throwTimer <= 0)
                {
                    //Debug.Log("Throw");
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
