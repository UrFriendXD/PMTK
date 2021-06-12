using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private int inverseSpawnRate;
    [SerializeField] private List<GameObject> patternPrefabs;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        InvokeRepeating(nameof(SpawnRandomPattern), 0, inverseSpawnRate);
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
