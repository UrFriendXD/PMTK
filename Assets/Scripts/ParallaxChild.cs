using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxChild : MonoBehaviour
{
    [SerializeField] private bool isRight;
    // Start is called before the first frame update
    void OnValidate()
    {
        var length = GetComponent<SpriteRenderer>().bounds.size.x;
        if (!isRight)
        {
            length *= -1;
        }
        transform.localPosition = new Vector3( length, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
