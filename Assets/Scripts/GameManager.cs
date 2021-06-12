using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private float windForce = 5f;

    private int score;

    public static GameManager Instance { get; private set; }
    
    public float WindForce => windForce;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UpdateScoreUI();
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Score = 0;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + Score;
    }
}
