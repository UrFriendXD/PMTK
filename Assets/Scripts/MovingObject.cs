using UnityEngine;

namespace DefaultNamespace
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] private int speed;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        
            rb.velocity = Vector2.left * speed;
        }
    }
}