using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroySceneTrigger"))
        {
            SceneManager.UnloadSceneAsync(other.gameObject.scene);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
