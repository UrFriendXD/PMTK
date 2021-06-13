using System;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private int TimeBetweenSpawn;
    [SerializeField] private List<GameObject> patternPrefabs;

    private GameManager gameManager;
    private float timer = 0;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timer = TimeBetweenSpawn;
    }

    private void Update()
    {
        if (gameManager.GameActive)
        {
            if (timer <= 0)
            {
                SpawnRandomPattern();
                timer = TimeBetweenSpawn;
            }

            timer -= Time.deltaTime;
        }
    }

    private void SpawnRandomPattern()
    {
        GameObject randomPattern = patternPrefabs[Random.Range(0, patternPrefabs.Count)];

        Vector2 position = new Vector2(
            gameManager.ViewportRightSide,
            0
        );

        Instantiate(randomPattern, position, quaternion.identity, transform);
    }
}
