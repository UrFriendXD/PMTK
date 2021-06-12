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
    
    void Start()
    {
        Reset();
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
