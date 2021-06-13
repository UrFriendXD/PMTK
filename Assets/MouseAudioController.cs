using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip Hover;
    [SerializeField] private AudioClip Click;

    [SerializeField] private AudioSource _audioSource;

    public void OnClick()
    {
        _audioSource.clip = Click;
        _audioSource.Play();
    }
    
    public void OnHover()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = Hover;
            _audioSource.Play();
        }
    }
}
