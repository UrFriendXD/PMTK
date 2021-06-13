using System;
using Player;
using UnityEngine;

public class PayloadController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private bool isImmaterialWhenThrown;

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

    private void Update()
    {
        if (playerController.IsPartiallyReleased && isImmaterialWhenThrown)
            collider.enabled = false;
        else
            collider.enabled = true;
    }
}
