using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private int startLives;
    [SerializeField] private LivesUI livesUI;

    private int score;
    [SerializeField] private float windForce = 5f;

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

    private int lives;

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            livesUI.UpdateLivesUI(lives);
        }
    }    
    
    private void Awake()
    {
        Reset();
        if (Instance != null)
        {
            Destroy(this);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + Score;
    }
    
    public void LoseLife()
    {
        Debug.Log("Lose Life");
        Lives--;

        if (Lives == 0)
        {
            GameOver();
        }
    }

    private void Reset()
    {
        Score = 0;
        Lives = startLives;
    }

    private void GameOver()
    {
        // TODO
        Debug.Log("Game Over");
    }
}
