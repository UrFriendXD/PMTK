using UnityEngine;

public class Pattern : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(
            FindObjectOfType<GameManager>().ViewportRightSide,
            0,
            0
        );
    }
}
