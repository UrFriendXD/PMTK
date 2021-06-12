using System;
using UnityEngine;

namespace Player
{
    public class RopeDrawer : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (playerController.IsReleased)
            {
                lineRenderer.positionCount = 0;
            }
            else
            {
                lineRenderer.positionCount = 2;

                Vector3[] positions = {
                    transform.position,
                    playerController.PayloadBody.position
                };
                
                lineRenderer.SetPositions(positions);
            }
        }
    }
}