using System;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternSpawner : MonoBehaviour
{
    [Serializable]
    private struct PatternSegment
    {
        public GameObject prefab;
        public float startTimeFrame;
        public float endTimeFrame;
    }
    
    [SerializeField] private float TimeBetweenSpawnMax;
    [SerializeField] private float TimeBetweenSpawnMin;
    [SerializeField] private float timeBetweenSpawnIncreaseInterval = 1f;
    [SerializeField] private float timeBetweenSpawnIncrease;
    [SerializeField] private float timeBetweenSpawnIncreaseMax = Mathf.Infinity;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private List<GameObject> patternPrefabs;
    [SerializeField] private List<PatternSegment> patternSegments; 

    private GameManager gameManager;
    private float timer = 0;
    private float startTimer;
    private float currentIncreaseInterval;
    private float startTimeBetweenSpawnMax;
    private float startTimeBetweenSpawnMin;
    private float totalIncrease;
    private float timeFrame;
    
    public HashSet<GameObject> SpawnedPatternObjects { get; } = new HashSet<GameObject>();
    
    public static PatternSpawner Instance { get; private set; }

    void Start()
    {
        Instance = this;
        gameManager = FindObjectOfType<GameManager>();
        timer = TimeBetweenSpawnMax;

        startTimeBetweenSpawnMin = TimeBetweenSpawnMin;
        startTimeBetweenSpawnMax = TimeBetweenSpawnMax;
    }

    public void Reset()
    {
        startTimer = 0;
        timer = 0;
        TimeBetweenSpawnMin = startTimeBetweenSpawnMin;
        TimeBetweenSpawnMax = startTimeBetweenSpawnMax;
        totalIncrease = 0;
        timeFrame = 0;

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

        if (totalIncrease < timeBetweenSpawnIncreaseMax && currentIncreaseInterval >= timeBetweenSpawnIncreaseInterval)
        {
            TimeBetweenSpawnMin += timeBetweenSpawnIncrease;
            TimeBetweenSpawnMax += timeBetweenSpawnIncrease;
            totalIncrease += timeBetweenSpawnIncrease;
            currentIncreaseInterval = 0;
        }

        startTimer += Time.deltaTime;
        timeFrame += Time.deltaTime;
        currentIncreaseInterval += Time.deltaTime;
    }

    private void SpawnRandomPattern()
    {
        GameObject patternObject = null;
        HashSet<int> visited = new HashSet<int>();

        do
        {
            int index = Random.Range(0, patternPrefabs.Count);
            var segment = patternSegments[index];
            visited.Add(index);
            
            if (segment.startTimeFrame >= timeFrame && segment.endTimeFrame <= timeFrame)
            {
                patternObject = segment.prefab;
                break;
            }
        } while (visited.Count < patternSegments.Count);

        if (patternObject != null)
        {
            Vector2 position = new Vector2(
                gameManager.ViewportRightSide * 3,
                0
            );

            GameObject go = Instantiate(patternObject, position, quaternion.identity, transform);
            SpawnedPatternObjects.Add(go);
        }
    }

    public void RemoveSpawnPatternObject(GameObject go)
    {
        SpawnedPatternObjects.Remove(go);
    }
}
