using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    
        rb.velocity = Vector2.left * gameManager.WindForce * Time.deltaTime;
    }
}