using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private float spawnAreaHeight;
    [SerializeField] private int inverseSpawnRate;
    [SerializeField] private List<int> patternSceneBuildIndexes;

    void Start()
    {
        // InvokeRepeating(nameof(SpawnPickup), inverseSpawnRate, inverseSpawnRate);
        InvokeRepeating(nameof(SpawnRandomPattern), inverseSpawnRate, inverseSpawnRate);
    }
    

    private void SpawnPickup()
    {
        Vector2 position = new Vector2(
            transform.position.x,
            transform.position.y + Random.Range(spawnAreaHeight / -2, spawnAreaHeight / 2)
        );
        
        Instantiate(pickupPrefab, position, quaternion.identity, transform);
    }

    private void SpawnRandomPattern()
    {
        int randomSceneBuildIndex = patternSceneBuildIndexes[Random.Range(0, patternSceneBuildIndexes.Count)];
        
        SceneManager.LoadScene(randomSceneBuildIndex, LoadSceneMode.Additive);
        
        // SceneManager.MergeScenes(SceneManager.GetSceneAt(1), SceneManager.GetActiveScene());
        //
        // // SceneManager.MergeScenes(patternScenes[Random.Range(0, patternScenes.Count)], SceneManager.GetActiveScene());
    }
}
