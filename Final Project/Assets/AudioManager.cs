using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // For background music
    [SerializeField] private AudioSource SFXSource;   // For one-shot sound effects
    [SerializeField] private AudioSource climbingSource; // For climbing-related sounds
    [SerializeField] private AudioSource grappleSource;  // For grapple-related sounds
    [SerializeField] private AudioSource gasSource;
    [SerializeField] private AudioSource waterSource; // For looping water movement sound


    [Header("Audio Clips")]
    public List<AudioClip> levelBackgroundMusic; // List of background music for levels
    public AudioClip pressurePlateSound;
    public AudioClip FloorTouch; // For walking or floor interaction
    public AudioClip WallTouch;  // For hitting walls
    public AudioClip climbVine1; // First climbing sound
    public AudioClip climbVine2; // Second climbing sound
    public AudioClip jumpSound;
    public AudioClip switchingCharacterSound; // Character switch sound
    public AudioClip grappleVine;
    public AudioClip PlantBallmovement;
    public AudioClip Magneticability;
    public AudioClip keyCollectedSound; // Sound effect for key collection
    public AudioClip doorOpenSound;     // Sound effect for door opening
    public AudioClip doorCloseSound;    // Sound effect for door closing
    public AudioClip toxicGasSound;
    public AudioClip waterEnterSound;  // Sound for entering water
    public AudioClip waterMovementSound; // Continuous water movement sound

    private void Start()
    {
        PlayLevelMusic();
    }

    // Play the assigned background music for the current level
    private void PlayLevelMusic()
    {
        if (musicSource == null || levelBackgroundMusic == null || levelBackgroundMusic.Count == 0)
        {
            Debug.LogWarning("AudioManager: Music source or level background music list is not set up.");
            return;
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if the scene index has an assigned background music
        if (currentSceneIndex < levelBackgroundMusic.Count && levelBackgroundMusic[currentSceneIndex] != null)
        {
            musicSource.clip = levelBackgroundMusic[currentSceneIndex];
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log($"AudioManager: Playing background music for level {currentSceneIndex} - {levelBackgroundMusic[currentSceneIndex].name}");
        }
        else
        {
            Debug.LogWarning($"AudioManager: No background music assigned for level {currentSceneIndex}");
        }
    }
    private bool isWaterSoundPlaying = false;

    // Play water entry sound (one-shot)
    public void PlayWaterEnterSound()
    {
        if (SFXSource != null && waterEnterSound != null)
        {
            SFXSource.PlayOneShot(waterEnterSound);
            Debug.Log("AudioManager: Water entry sound played.");
        }
    }

    // Play grapple sound effect
    public void PlayGrappleSound()
    {
        if (grappleSource != null && grappleVine != null)
        {
            grappleSource.PlayOneShot(grappleVine);
            Debug.Log("AudioManager: Grapple sound played.");
        }
        else
        {
            Debug.LogWarning("AudioManager: Grapple sound or source is not set!");
        }
    }

    // Play water movement sound (looping)
    public void PlayWaterMovementSound()
    {
        if (waterSource != null && waterMovementSound != null && !waterSource.isPlaying)
        {
            waterSource.clip = waterMovementSound;
            waterSource.loop = true;
            waterSource.Play();
            isWaterSoundPlaying = true;
            Debug.Log("AudioManager: Water movement sound started.");
        }
    }

    // Stop water movement sound
    public void StopWaterMovementSound()
    {
        if (waterSource != null && waterSource.isPlaying)
        {
            waterSource.Stop();
            isWaterSoundPlaying = false;
            Debug.Log("AudioManager: Water movement sound stopped.");
        }
    }

    // Check if water movement sound is playing
    public bool IsWaterSoundPlaying()
    {
        return isWaterSoundPlaying;
    }

    // Play gas sound in a loop
    public void PlayGasSound()
    {
        if (gasSource != null && toxicGasSound != null && !gasSource.isPlaying)
        {
            gasSource.clip = toxicGasSound;
            gasSource.loop = true;
            gasSource.Play();
        }
    }

    public void StopGasSound()
    {
        if (gasSource != null && gasSource.isPlaying)
        {
            gasSource.Stop();
        }
    }

    public bool IsGasSoundPlaying()
    {
        return gasSource != null && gasSource.isPlaying;
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

    // Play a random climbing sound
public void PlayRandomClimbingSound()
{
    if (climbingSource != null)
    {
        // Randomly select one of the two climbing sounds
        AudioClip selectedClip = Random.value < 0.5f ? climbVine1 : climbVine2;

        if (selectedClip != null)
        {
            climbingSource.clip = selectedClip;
            climbingSource.loop = true;
            climbingSource.Play();
            Debug.Log($"AudioManager: Playing climbing sound {selectedClip.name}");
        }
    }
}

// Stop the climbing sound
public void StopClimbingSound()
{
    if (climbingSource != null && climbingSource.isPlaying)
    {
        climbingSource.Stop();
        Debug.Log("AudioManager: Climbing sound stopped.");
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
