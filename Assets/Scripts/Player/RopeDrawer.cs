using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class RopeDrawer : MonoBehaviour
    {
        [SerializeField] private List<Joint2D> hinges = new List<Joint2D>();
        
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
                lineRenderer.positionCount = hinges.Count + 2;

                Vector3[] positions = hinges
                    .Select(h => h.transform.position)
                    .Prepend(transform.position)
                    .Append(playerController.PayloadBody.position)
                    .ToArray();
                
                lineRenderer.SetPositions(positions);
            }
        }
    }
}