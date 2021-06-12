using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private int inverseSpawnRate;
    [SerializeField] private List<int> patternSceneBuildIndexes;

    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomPattern), inverseSpawnRate, inverseSpawnRate);
    }

    private void SpawnRandomPattern()
    {
        int randomSceneBuildIndex = patternSceneBuildIndexes[Random.Range(0, patternSceneBuildIndexes.Count)];
        
        SceneManager.LoadScene(randomSceneBuildIndex, LoadSceneMode.Additive);
    }
}
