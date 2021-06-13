using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;

namespace Player
{
    public class ProceduralRope : MonoBehaviour
    {
        [SerializeField] private float jointMass = 0.2f;
        [SerializeField] private float endJointReleaseMass = 0.5f;
        [SerializeField] private int jointCount = 8;
        [SerializeField] private bool shouldHaveCollider = true;
        [SerializeField] private float jointWidth = 0.3f;
        [SerializeField] private PhysicsMaterial2D jointPhysicsMaterial;
        [Header("Break")]
        [SerializeField] private bool shouldBreak = true;
        [SerializeField] private float ropeBreakForce = 1100;

        private LineRenderer lineRenderer;
        private PlayerController playerController;
        private DistanceJoint2D rootJoint;
        private float distancePerSegment;
        private List<GameObject> jointObjects = new List<GameObject>();
        private List<Joint2D> joints = new List<Joint2D>();
        private List<Joint2D> shownLineJoints = new List<Joint2D>();
        
        public Joint2D EndJoint { get; private set; }

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            lineRenderer = GetComponent<LineRenderer>();
            rootJoint = GetComponent<DistanceJoint2D>();

            var ropeController = rootJoint.gameObject.AddComponent<RopeController>();
            ropeController.OnJointBreak += OnRopeBreak;
        }

        private void Start()
        {
            distancePerSegment = playerController.PayloadDistance / (jointCount + 1);
            GenerateJoints();
        }

        private void Update()
        {

            lineRenderer.positionCount = 0;

            if (!playerController.IsReleased)
            {
                lineRenderer.positionCount = shownLineJoints.Count + 1;

                if (EndJoint)
                    lineRenderer.positionCount++;

                var positions = shownLineJoints
                    .Select(h => h.transform.position)
                    .Prepend(transform.position + (Vector3) rootJoint.anchor);

                if (EndJoint)
                    positions = positions.Append(playerController.PayloadBody.position);
                
                lineRenderer.SetPositions(positions.ToArray());
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
                Vector2 offset = -direction * distancePerSegment / 2f;

                GameObject jointObject = new GameObject($"Rope Joint - Segment {i}");
                jointObject.transform.SetParent(transform.parent);
                jointObject.transform.position = segmentPosition + offset;
                
                var jointBody = jointObject.AddComponent<Rigidbody2D>();
                jointBody.mass = jointMass;
                jointBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                jointBody.interpolation = RigidbodyInterpolation2D.Interpolate;
                jointBody.sharedMaterial = jointPhysicsMaterial;
                ApplyCollider(jointBody, direction);

                var joint = jointObject.AddComponent<DistanceJoint2D>();
                lastJoint.connectedBody = jointBody;
                
                if (shouldBreak)
                    joint.breakForce = ropeBreakForce;
                joint.anchor = offset;
                //joint.anchor = Vector2.zero;
                //joint.connectedAnchor = offset;
                SetSegmentJointDistance(lastJoint);
                var ropeController = jointObject.AddComponent<RopeController>();
                ropeController.OnJointBreak += OnRopeBreak;
                
                lastJoint = joint;
                joints.Add(joint);
                jointObjects.Add(joint.gameObject);
                shownLineJoints.Add(joint);
            }
            
            lastJoint.connectedBody = playerController.PayloadBody;
            EndJoint = lastJoint;
            playerController.PayloadBody.sharedMaterial = jointPhysicsMaterial;
            //ApplyCollider(lastJoint.gameObject, direction);
            SetSegmentJointDistance(lastJoint);
        }

        private void OnRopeBreak()
        {
            ClearJoints();
        }
        
        public IEnumerator DoDelayedClearJoints(float delay, Vector2 impulseForce)
        {
            EndJoint.attachedRigidbody.mass = endJointReleaseMass;
            EndJoint.attachedRigidbody.AddForce(impulseForce, ForceMode2D.Impulse);
            EndJoint.connectedBody = null;
            shownLineJoints.Remove(EndJoint);
            
            Destroy(EndJoint);
            yield return new WaitForSeconds(delay);
            ClearJoints();
        }

        public void ClearJoints()
        {
            foreach (GameObject jointObject in jointObjects)
            {
                if (jointObject != null)
                    Destroy(jointObject);
            }
            
            rootJoint.connectedBody = null;
            joints.Clear();
            jointObjects.Clear();
            shownLineJoints.Clear();
        }

        private void ApplyCollider(Rigidbody2D body, Vector2 direction)
        {
            if (!shouldHaveCollider) return;

            GameObject go = body.gameObject;
            go.layer = 7;
            var jointCollider = go.AddComponent<BoxCollider2D>();
            // Vector2 colliderCenter = go.transform
            //     .InverseTransformVector(-direction * distancePerSegment / 2f);
            // jointCollider.offset = colliderCenter;
            jointCollider.size = new Vector2(distancePerSegment * 2, jointWidth);
            //body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void SetSegmentJointDistance(Joint2D joint)
        {
            // Bad game jam code
            if (joint is DistanceJoint2D distanceJoint)
            {
                distanceJoint.autoConfigureDistance = false;
                distanceJoint.distance = distancePerSegment / 2f;
                distanceJoint.maxDistanceOnly = true;
            }

            if (joint is SpringJoint2D springJoint)
            {
                springJoint.autoConfigureDistance = false;
                springJoint.distance = distancePerSegment / 2f;
                springJoint.frequency = 20;
                springJoint.dampingRatio = 1;
            }
        }
    }
}