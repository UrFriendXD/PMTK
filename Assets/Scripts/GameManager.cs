using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private int startLives;
    [SerializeField] private GameObject lifeUIPrefab;
    [SerializeField] private Transform lifeUIContainer;
    
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
            UpdateLivesUI();
        }
    }

    private List<GameObject> lifeUIs = new List<GameObject>();
    
    void Start()
    {
        Reset();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + Score;
    }

    private void UpdateLivesUI()
    {
        while (Lives > lifeUIs.Count)
        {
            AddLifeUI();
        }
        
        while (Lives < lifeUIs.Count)
        {
            RemoveLifeUI();
        }
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

    private void AddLifeUI()
    {
        GameObject newLifeUI = Instantiate(lifeUIPrefab, lifeUIContainer);
        
        lifeUIs.Add(newLifeUI);
    }

    private void RemoveLifeUI()
    {
        GameObject lifeUI = lifeUIs.Last();

        lifeUIs.Remove(lifeUI);
        
        Destroy(lifeUI);
    }
}
