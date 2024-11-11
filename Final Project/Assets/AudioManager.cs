using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource climbingSource;
    [SerializeField] private AudioSource grappleSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip pressurePlateSound;
    public AudioClip FloorTouch;
    public AudioClip WallTouch;
    public AudioClip climbVine;
    public AudioClip jumpSound;
    public AudioClip switchingCharacterSound;
    public AudioClip grappleVine;
    public AudioClip PlantBallmovement;
    public AudioClip Magneticability;


    private void Start()
    {
        // Play background music if assigned
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("Background music playing!");
        }
    }

    // Generic method to play one-shot sound effects
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
            Debug.Log("Playing SFX: " + clip.name);
        }
    }

    // Start looping climbing sound effect
    public void StartClimbingSound()
    {
        if (climbingSource != null && climbVine != null)
        {
            climbingSource.clip = climbVine;
            climbingSource.loop = true;
            climbingSource.Play();
            Debug.Log("Climbing sound started.");
        }
    }

    // Stop climbing sound effect
    public void StopClimbingSound()
    {
        if (climbingSource != null && climbingSource.isPlaying)
        {
            climbingSource.Stop();
            Debug.Log("Climbing sound stopped.");
        }
    }

    // Start looping grapple sound effect
    public void StartGrappleSound()
    {
        if (grappleSource != null && grappleVine != null)
        {
            grappleSource.clip = grappleVine;
            grappleSource.loop = true;
            grappleSource.Play();
            Debug.Log("Grapple sound started.");
        }
    }

    // Stop grapple sound effect
    public void StopGrappleSound()
    {
        if (grappleSource != null && grappleSource.isPlaying)
        {
            grappleSource.Stop();
            Debug.Log("Grapple sound stopped.");
        }
    }

    // Start looping ball movement sound effect
    public void StartBallMovementSound()
    {
        if (SFXSource != null && PlantBallmovement != null)
        {
            SFXSource.loop = true;
            SFXSource.clip = PlantBallmovement;
            SFXSource.Play();
            Debug.Log("Ball movement sound started.");
        }
    }

    // Stop ball movement sound effect
    public void StopBallMovementSound()
    {
        if (SFXSource != null && SFXSource.clip == PlantBallmovement && SFXSource.isPlaying)
        {
            SFXSource.Stop();
            SFXSource.loop = false; // Reset loop to false after stopping
            Debug.Log("Ball movement sound stopped.");
        }
    }
}