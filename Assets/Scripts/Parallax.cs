﻿using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public Camera cam;
    public float parallaxEffect;
    public float offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
            Debug.Log("Cam wasn't set, defaulting to main camera");
        }
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed);
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + (length - offset))
        {
            startpos += length;
        }
        else if (temp < startpos - (length - offset))
        {
            startpos -= length;
        }
    }
}
