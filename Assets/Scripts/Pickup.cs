using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int speed;

    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = Vector2.left * speed;

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
