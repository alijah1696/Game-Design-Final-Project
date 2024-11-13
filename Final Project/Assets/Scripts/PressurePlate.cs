using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform platform;
    public Vector3 targetPosition;
    public float speed = 2f;
    private bool platformActivated = false;

    private void Update()
    {
        if (platformActivated && platform != null)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, speed * Time.deltaTime);

            if (platform.position == targetPosition)
            {
                platformActivated = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for tags "Plant" or "Robot" to activate the pressure plate
        if ((other.CompareTag("Plant") || other.CompareTag("Robot")) && !platformActivated)
        {
            platformActivated = true;
            Debug.Log("Pressure plate activated by " + other.gameObject.name);
        }
    }
}
