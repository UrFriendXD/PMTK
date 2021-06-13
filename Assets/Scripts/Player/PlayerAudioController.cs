using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> Grunt = new List<AudioClip>(); 
    [SerializeField] private List<AudioClip> CatThrow = new List<AudioClip>(); 
    [SerializeField] private AudioClip CatThrow2; 
    [SerializeField] private AudioClip WitchThrow;
    
    [SerializeField] private List<AudioClip> WitchLaughter = new List<AudioClip>(); // Play at begin of each level

    [SerializeField] private AudioClip WitchDeath;
    
    [SerializeField] private AudioClip WitchCatch;
    [SerializeField] private AudioClip WitchCatchSwoosh;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnDeath()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(WitchDeath);
    }

    public void OnThrow()
    {
        var clip = Grunt[(Random.Range(0, Grunt.Count - 1))];
        var clip2 = CatThrow[(Random.Range(0, CatThrow.Count - 1))];
        _audioSource.PlayOneShot(clip);
        _audioSource.PlayOneShot(clip2);
        _audioSource.PlayOneShot(WitchThrow);
        _audioSource.PlayOneShot(CatThrow2);
    }

    public void OnCatch()
    {
        _audioSource.PlayOneShot(WitchCatch);
        _audioSource.PlayOneShot(WitchCatchSwoosh);
    }

    public void OnRespawn()
    {
        var clip = WitchLaughter[(Random.Range(0, WitchLaughter.Count - 1))];
        _audioSource.PlayOneShot(clip);
    }
}
