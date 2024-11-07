using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public Transform pointA; // First point
    public Transform pointB; // Second point
    public float speed = 1f; // Speed of interpolation

    private float progress = -1;


    // Update is called once per frame
    void Update()
    {
        InteractableProxy px = GetComponent<InteractableProxy>();
        progress = px.getProgress();

        transform.position = Vector3.Lerp(pointA.position, pointB.position, progress);
    }
}
