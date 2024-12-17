using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject respawnPoint;
    private SwapCharacters sc;
    private AudioManager audioManager;  // Reference to AudioManager

    void Start()
    {
        // Find the SwapCharacters and AudioManager components at the start
        sc = FindObjectOfType<SwapCharacters>();
        audioManager = FindObjectOfType<AudioManager>();  // Automatically find the AudioManager in the scene
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player", "Robot", or "Plant"
        if (other.CompareTag("Robot")) // Handling for Robot
        {
            sc.Kill(respawnPoint);
            PlayWaterEnterSound();
        }
        else if (other.CompareTag("Plant")) // Handling for Plant
        {
            MoveCharacter mv = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            mv.InDanger();
            PlayWaterEnterSound();
        }
    }

    // Play water entry sound effect
    private void PlayWaterEnterSound()
    {
        if (audioManager != null && audioManager.waterEnterSound != null)
        {
            audioManager.PlaySFX(audioManager.waterEnterSound);
        }
        else
        {
            Debug.LogWarning("AudioManager or waterEnterSound is not set or is null");
        }
    }
}
