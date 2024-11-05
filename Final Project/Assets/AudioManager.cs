using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Sources -----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource climbingSource;  // New AudioSource for climbing sound

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;
    public AudioClip Death;
    public AudioClip CheckPoint;
    public AudioClip WallTouch;
    public AudioClip FloorTouch;
    public AudioClip switchingCharacter;
    public AudioClip climbVine;  // Clip for climbing sound

    private void Start()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
            Debug.Log("Background Sound playing!");
        }
    }

    // Play a one-shot sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    // Start playing the climbing sound continuously
    public void StartClimbingSound()
    {
        if (climbingSource != null && climbVine != null)
        {
            climbingSource.clip = climbVine;
            climbingSource.loop = true;  // Loop the climbing sound
            climbingSource.Play();
        }
    }

    // Stop the climbing sound immediately
    public void StopClimbingSound()
    {
        if (climbingSource != null)
        {
            climbingSource.Stop();
        }
    }
}
