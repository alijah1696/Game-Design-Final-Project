using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject respawnPoint;
    private SwapCharacters sc;
    private AudioManager audioManager; // Reference to AudioManager
    private bool isPlayerInWater = false; // Track if the player is in water
    private MoveCharacter currentPlayer;  // To check movement state

    void Start()
    {
        // Find the SwapCharacters and AudioManager components at the start
        sc = FindObjectOfType<SwapCharacters>();
        audioManager = FindObjectOfType<AudioManager>(); // Automatically find the AudioManager in the scene
    }

    void Update()
    {
        // Check if the player is in water and moving
        if (isPlayerInWater && currentPlayer != null)
        {
            if (currentPlayer.IsMoving() && !audioManager.IsWaterSoundPlaying())
            {
                audioManager.PlayWaterMovementSound();
            }
            else if (!currentPlayer.IsMoving() && audioManager.IsWaterSoundPlaying())
            {
                audioManager.StopWaterMovementSound();
            }
        }
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
            currentPlayer = sc.GetCurrentForm().GetComponent<MoveCharacter>();
            currentPlayer.InDanger();
            PlayWaterEnterSound();
            isPlayerInWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            if (currentPlayer != null)
            {
                currentPlayer.Safe();
            }

            isPlayerInWater = false;
            audioManager.StopWaterMovementSound();
        }
    }

    // Play water entry sound effect
    private void PlayWaterEnterSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayWaterEnterSound();
        }
    }
}
