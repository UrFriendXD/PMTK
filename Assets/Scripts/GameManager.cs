using System;
using System.Timers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // [SerializeField] private int startLives;
    [SerializeField] private PostcardController cardController;

    private float score;
    [SerializeField] private float startWindForce = 4f;
    [SerializeField] private float maxWindForce = 5f;
    [SerializeField] private float windForceAcceleration = 0.1f;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float currentWindForce;

    [SerializeField] private float connectedScorePerSecond;
    [SerializeField] private float disconnectedScorePerSecond;
    [SerializeField] private float addScoreOnDisconnect;

    public static GameManager Instance { get; private set; }

    public float ViewportRightSide { get; private set; }

    public float WindForce => currentWindForce;

    public bool GameActive;
    
    private float timer = 0;

    public float Score
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
            Instance.scoreText = scoreText;
            Instance.cardController = cardController;
            Destroy(this);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        ViewportRightSide = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x;
    }

    public void UpdateScoreUI()
    {
        if (scoreText)
            scoreText.text = (Math.Round(Score)).ToString();
    }

    public void LoseLife()
    {
        Debug.Log("Lose Life");
        //Lives--;

        GameOver();
        /*if (Lives == 0)
        {
            GameOver();
        }*/
    }

    private void Reset()
    {
        Score = 0;
        // Lives = startLives;
        currentWindForce = startWindForce;
        timer = 0;
    }

    [ContextMenu("Game Over")]
    private void GameOver()
    {
        if (GameActive)
        {
            // TODO have a timer for animations and raterise 
            Debug.Log("Game Over");
            // Reset();
            PatternSpawner.Instance.Reset();
            // SceneManager.LoadScene(0);

            cardController.Rasterise();
            playerController.Death();

            GameActive = false;
        }
    }

    private void Start()
    {
        cardController.Spawn();
        cardController.Top.ShowUI(UIController.UIType.Menu);
        cardController.Top.posterise = false;
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
        GameActive = true;
        PlayerController.Instance.Respawn();

        Reset();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void RestartGameAfterLoad(Scene scene, LoadSceneMode mode)
    {
        cardController.Spawn();
    }

    private void Update()
    {
        if (GameActive)
        {
            if (currentWindForce < maxWindForce)
            {
                currentWindForce += windForceAcceleration * Time.deltaTime;
            }
            
            if (playerController.IsReleased)
                timer += Time.deltaTime * disconnectedScorePerSecond;
            else 
                timer += Time.deltaTime * connectedScorePerSecond;

            Score = timer;
        }
    }

    public void OnRelease()
    {
        timer += addScoreOnDisconnect;
    }
}
