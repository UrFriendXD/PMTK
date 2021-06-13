using System;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private float TimeBetweenSpawnMax;
    [SerializeField] private float TimeBetweenSpawnMin;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private List<GameObject> patternPrefabs;

    private GameManager gameManager;
    private float timer = 0;
    private float startTimer;
    
    public HashSet<GameObject> SpawnedPatternObjects { get; } = new HashSet<GameObject>();
    
    public static PatternSpawner Instance { get; private set; }

    void Start()
    {
        Instance = this;
        gameManager = FindObjectOfType<GameManager>();
        timer = TimeBetweenSpawnMax;
    }

    public void Reset()
    {
        startTimer = 0;
        timer = 0;

        foreach (GameObject go in SpawnedPatternObjects)
        {
            Destroy(go);
        }
    }

    private void Update()
    {
        if (gameManager.GameActive && startTimer > startDelay)
        {
            if (timer <= 0)
            {
                SpawnRandomPattern();
                timer = Random.Range(TimeBetweenSpawnMin, TimeBetweenSpawnMax);
            }

            timer -= Time.deltaTime;
        }

        startTimer += Time.deltaTime;
    }

    private void SpawnRandomPattern()
    {
        GameObject randomPattern = patternPrefabs[Random.Range(0, patternPrefabs.Count)];

        Vector2 position = new Vector2(
            gameManager.ViewportRightSide,
            0
        );

        GameObject go = Instantiate(randomPattern, position, quaternion.identity, transform);
        SpawnedPatternObjects.Add(go);
    }

    public void RemoveSpawnPatternObject(GameObject go)
    {
        SpawnedPatternObjects.Remove(go);
    }
}
