using UnityEngine;

namespace Player
{
    public class CatchIndicator : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform target;
        
        private void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.GetChild(0).position);
        }
        
        public void Show()
        {
            lineRenderer.enabled = true;
        }

        public void Hide()
        {
            lineRenderer.enabled = false;
        }
    }
}