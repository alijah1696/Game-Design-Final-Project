using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMOHandler : MonoBehaviour
{
    public Transform[] spawnPoints; // Array of spawn points
    public int initialSpawnIndex = 0; // Initial spawn index
    private int currentSpawnIndex;
    public GameObject player; // Reference to the player object

    void Start()
    {
        currentSpawnIndex = initialSpawnIndex; // Set the initial spawn index
        StartCoroutine(LateStart()); // Start the coroutine
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame
        // Move the player to the initial spawn point
        if (spawnPoints.Length > 0)
        {
            player.GetComponent<SwapCharacters>().GetCurrentForm().transform.position = spawnPoints[currentSpawnIndex].position;
            Debug.Log("Player initially moved to: " + spawnPoints[currentSpawnIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Detect Enter key press
        {
            SwapToNextSpawnPoint();
        }
    }

    void SwapToNextSpawnPoint()
    {
        if (spawnPoints.Length == 0) return; // Return if there are no spawn points

        // Increment the current spawn index and wrap around if necessary
        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;

        // Move the player to the next spawn point
        player.GetComponent<SwapCharacters>().GetCurrentForm().transform.position = spawnPoints[currentSpawnIndex].position;

        Debug.Log("Player moved to: " + spawnPoints[currentSpawnIndex].position);
    }
}
