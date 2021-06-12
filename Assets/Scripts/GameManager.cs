using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int startLives;
    [SerializeField] private LivesUI livesUI;
    [SerializeField] private PostcardController _postcardController;

    private int score;
    [SerializeField] private float windForce = 5f;

    public static GameManager Instance { get; private set; }

    public float ViewportRightSide { get; private set; }
    
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

        ViewportRightSide = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x;
    }

    private void Start()
    {
        _postcardController.Spawn();
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
        Reset();
        // SceneManager.LoadScene(0);
    }
}
