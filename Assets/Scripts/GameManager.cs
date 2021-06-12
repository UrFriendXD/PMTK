using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    
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
    
    void Start()
    {
        Score = 0;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + Score;
    }
}
