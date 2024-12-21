using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetZone : MonoBehaviour
{
    [Header("Objects to Reset")]
    [SerializeField] private List<GameObject> resettableObjects; // List of objects to reset

    // Dictionary to store the original positions and rotations of objects
    private Dictionary<GameObject, TransformData> originalTransforms = new Dictionary<GameObject, TransformData>();

    // Class to store position and rotation data
    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;

        public TransformData(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    void Start()
    {
        // Save the original positions and rotations for each object in the list
        foreach (GameObject obj in resettableObjects)
        {
            if (obj != null)
            {
                originalTransforms[obj] = new TransformData(obj.transform.position, obj.transform.rotation);
            }
        }
    }

    public void ResetGameSection()
    {
        Debug.Log("ResetZone: ResetGameSection called. Resetting objects.");

        foreach (KeyValuePair<GameObject, TransformData> entry in originalTransforms)
        {
            GameObject obj = entry.Key;
            TransformData data = entry.Value;

            if (obj != null)
            {
                // Reset position and rotation
                obj.transform.position = data.position;
                obj.transform.rotation = data.rotation;

                // Reset Rigidbody physics if it exists
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero; // Stop movement
                    rb.angularVelocity = 0f;    // Stop rotation
                }

                Debug.Log($"ResetZone: {obj.name} reset to original position.");
            }
        }
    }

    // Optional: Reset on trigger if the player enters the zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("ResetZone: Player entered reset zone.");
            ResetGameSection();
        }
    }
}
