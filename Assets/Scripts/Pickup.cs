using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int score;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Payload"))
        {
            gameManager.Score += score;
            Destroy(gameObject);
        }
    }
}
