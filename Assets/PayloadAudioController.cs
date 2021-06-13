using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PayloadAudioController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> Collide = new List<AudioClip>();

    public float delay;
    private float timer;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (timer < 0)
        {
            var clip = Collide[(Random.Range(0, Collide.Count - 1))];
            _audioSource.PlayOneShot(clip);
            timer = delay;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }
}
