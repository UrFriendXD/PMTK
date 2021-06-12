using UnityEngine;

public class Breakable : MonoBehaviour
{
    private string brokenByTag;
    private void Start()
    {
        brokenByTag = tag switch
        {
            "PlayerObstacle" => "Payload",
            "PayloadObstacle" => "Player",
            _ => brokenByTag
        };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(brokenByTag))
        {
            Break();
        }
    }

    private void Break()
    {
        // TODO: Breaking animation

        Destroy(gameObject);
    }
}
