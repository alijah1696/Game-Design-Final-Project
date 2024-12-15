using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // For background music
    [SerializeField] private AudioSource SFXSource;   // For one-shot sound effects
    [SerializeField] private AudioSource climbingSource; // For climbing-related sounds
    [SerializeField] private AudioSource grappleSource;  // For grapple-related sounds

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip pressurePlateSound;
    public AudioClip FloorTouch; // For walking or floor interaction
    public AudioClip WallTouch;  // For hitting walls
    public AudioClip climbVine;
    public AudioClip jumpSound;
    public AudioClip switchingCharacterSound; // Character switch sound
    public AudioClip grappleVine;
    public AudioClip PlantBallmovement;
    public AudioClip Magneticability;
    public AudioClip keyCollectedSound; // Sound effect for key collection
    public AudioClip doorOpenSound;     // Sound effect for door opening
    public AudioClip doorCloseSound;    // Sound effect for door closing

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

    // Method to play key collected sound
    public void PlayKeyCollectedSound()
    {
        if (SFXSource != null && keyCollectedSound != null)
        {
            SFXSource.PlayOneShot(keyCollectedSound);
            Debug.Log("Key collected sound played.");
        }
        else
        {
            Debug.LogWarning("Key collected sound or SFXSource is missing!");
        }
    }

    // Method to play door open or close sound
    public void PlayDoorSound(bool isOpen)
    {
        AudioClip clipToPlay = isOpen ? doorOpenSound : doorCloseSound;
        if (SFXSource != null && clipToPlay != null)
        {
            SFXSource.PlayOneShot(clipToPlay);
            Debug.Log(isOpen ? "Door open sound played." : "Door close sound played.");
        }
        else
        {
            Debug.LogWarning("Door sound or SFXSource is missing!");
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

    // Start looping magnetic ability sound
    public void StartMagneticAbilitySound()
    {
        if (SFXSource != null && Magneticability != null)
        {
            SFXSource.loop = true;
            SFXSource.clip = Magneticability;
            SFXSource.Play();
            Debug.Log("Magnetic ability sound started.");
        }
    }

    // Stop magnetic ability sound
    public void StopMagneticAbilitySound()
    {
        if (SFXSource != null && SFXSource.clip == Magneticability && SFXSource.isPlaying)
        {
            SFXSource.Stop();
            SFXSource.loop = false; // Reset loop to false after stopping
            Debug.Log("Magnetic ability sound stopped.");
        }
    }

    // Start looping walking sound effect
    public void StartWalkingSound()
    {
        if (SFXSource != null && FloorTouch != null)
        {
            SFXSource.loop = true;
            SFXSource.clip = FloorTouch;
            SFXSource.Play();
            Debug.Log("Walking sound started.");
        }
    }

    // Stop looping walking sound effect
    public void StopWalkingSound()
    {
        if (SFXSource != null && SFXSource.clip == FloorTouch && SFXSource.isPlaying)
        {
            SFXSource.Stop();
            SFXSource.loop = false; // Reset loop to false after stopping
            Debug.Log("Walking sound stopped.");
        }
    }
}
