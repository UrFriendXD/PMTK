using UnityEngine;

public class PatternDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyPatternTrigger"))
        {
            GameObject go = other.transform.parent.gameObject;
            PatternSpawner.Instance.RemoveSpawnPatternObject(go);
            Destroy(go);
        }
        else if (other.CompareTag("Payload"))
        {
            GameManager.Instance.LoseLife();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
