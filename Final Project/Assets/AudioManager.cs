using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;    
    [SerializeField] private AudioSource SFXSource;      
    [SerializeField] private AudioSource climbingSource; 

    [Header("Audio Clips")]
    public AudioClip background;            
    public AudioClip pressurePlateSound;     
    public AudioClip FloorTouch;             
    public AudioClip WallTouch;              
    public AudioClip climbVine;              
    public AudioClip jumpSound;              
    public AudioClip switchingCharacterSound; 

    private void Start()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
            Debug.Log("Background music playing!");
        }
    }

    
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    
    public void StartClimbingSound()
    {
        if (climbingSource != null && climbVine != null)
        {
            climbingSource.clip = climbVine;
            climbingSource.loop = true;
            climbingSource.Play();
        }
    }

    
    public void StopClimbingSound()
    {
        if (climbingSource != null)
        {
            climbingSource.Stop();
        }
    }
}
