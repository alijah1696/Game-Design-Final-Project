using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clips -----------")]
    public AudioClip background;
    public AudioClip Death;
    public AudioClip CheckPoint;
    public AudioClip WallTouch;
    public AudioClip Climbwall;
    public AudioClip water;
    public AudioClip switchingCharacter;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        Debug.Log("Background Sound playing!");
    }

    // Method to play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
