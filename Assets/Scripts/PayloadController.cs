using UnityEngine;

public class PayloadController : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("PayloadObstacle"))
    //     {
    //         gameManager.LoseLife();
    //     }
    // }
}
