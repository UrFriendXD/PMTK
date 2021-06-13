using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip Hover;
    [SerializeField] private AudioClip Click;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private UIController ui;

    public void OnClick()
    {
        if (!ui.Enabled)
            return;
        
        _audioSource.clip = Click;
        _audioSource.Play();
    }
    
    public void OnHover()
    {
        if (!ui.Enabled || _audioSource.isPlaying)
            return;
        
        _audioSource.clip = Hover;
        _audioSource.Play();
    }
}
