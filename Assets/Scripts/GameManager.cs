using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // [SerializeField] private int startLives;
    [SerializeField] private PostcardController cardController;

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
            // livesUI.UpdateLivesUI(lives);
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

    public void UpdateScoreUI()
    {
        if (scoreText)
            scoreText.text = Score.ToString();
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
        // Lives = startLives;
    }

    [ContextMenu("Game Over")]
    private void GameOver()
    {
        // TODO
        Debug.Log("Game Over");
        Reset();
        // SceneManager.LoadScene(0);

        cardController.Rasterise();
    }

    private void Start()
    {
        cardController.Spawn();
        cardController.Top.ShowUI(UIController.UIType.Menu);
    }

    public void OnBeginPlay()
    {
        if (!Camera.main)
        {
            Debug.LogError("There is no main camera.");
            return;
        }

        GameObject titleCard = GameObject.FindGameObjectWithTag("Title Card");
        if (titleCard && titleCard.activeInHierarchy)
        {
            cardController.Rasterise(false);
            titleCard.SetActive(false);
        }

        cardController.Disable();
        cardController.Spawn();
        cardController.Top.ShowUI(UIController.UIType.Game);
        // TODO: Start the game (reset score, start obstacle spawning?)

        Reset();
    }
}
