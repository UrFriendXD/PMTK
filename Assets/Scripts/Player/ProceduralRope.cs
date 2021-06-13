using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class ProceduralRope : MonoBehaviour
    {
        [SerializeField] private float jointMass = 0.2f;
        [SerializeField] private int jointCount = 8;

        private LineRenderer lineRenderer;
        private PlayerController playerController;
        private Joint2D rootJoint;
        private float distancePerSegment;
        private List<Joint2D> joints = new List<Joint2D>();

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            lineRenderer = GetComponent<LineRenderer>();
            rootJoint = GetComponent<DistanceJoint2D>();
        }

        private void Start()
        {
            distancePerSegment = (jointCount - 2) / playerController.PayloadDistance;
            GenerateJoints();
        }

        private void Update()
        {
            if (playerController.IsReleased)
            {
                lineRenderer.positionCount = 0;
            }
            else
            {
                lineRenderer.positionCount = joints.Count + 2;

                Vector3[] positions = joints
                    .Select(h => h.transform.position)
                    .Prepend(transform.position)
                    .Append(playerController.PayloadBody.position)
                    .ToArray();
                
                lineRenderer.SetPositions(positions);
            }
        }

        public void GenerateJoints()
        {
            Vector2 direction = playerController.PayloadVector.normalized;
            Joint2D lastJoint = rootJoint;

            for (int i = 0; i < jointCount; i++)
            {
                float segmentDistance = distancePerSegment * i;
                Vector2 segmentPosition = (Vector2) transform.position + (direction * segmentDistance);

                GameObject jointObject = new GameObject($"Rope Joint - Segment {i}");
                jointObject.transform.SetParent(transform.parent);
                jointObject.transform.position = segmentPosition;
                
                var jointBody = jointObject.AddComponent<Rigidbody2D>();
                jointBody.mass = jointMass;
                
                var joint = jointObject.AddComponent<DistanceJoint2D>();
                lastJoint.connectedBody = jointBody;
                SetSegmentJointDistance(lastJoint);
                
                lastJoint = joint;
                joints.Add(joint);
            }
            
            lastJoint.connectedBody = playerController.PayloadBody;
            SetSegmentJointDistance(lastJoint);
        }

        public void ClearJoints()
        {
            foreach (Joint2D joint in joints)
            {
                Destroy(joint.gameObject);
            }
            
            rootJoint.connectedBody = null;
            joints.Clear();
        }

        private void SetSegmentJointDistance(Joint2D joint)
        {
            // Bad game jam code
            if (joint is DistanceJoint2D distanceJoint)
            {
                distanceJoint.autoConfigureDistance = false;
                distanceJoint.distance = distancePerSegment;
            }
        }
    }
}