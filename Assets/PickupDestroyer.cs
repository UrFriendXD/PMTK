using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroySceneTrigger"))
        {
            Debug.Log("yes");
            SceneManager.UnloadSceneAsync(other.gameObject.scene);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
