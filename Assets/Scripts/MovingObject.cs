using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.left * gameManager.WindForce;
    }
}