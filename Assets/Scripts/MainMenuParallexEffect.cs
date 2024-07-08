using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuParallaxEffect : MonoBehaviour
{
    public float speed = 5.0f;
    public float resetPositionX = -25.0f; // Position at which the object resets
    public float startPositionX = 0.55f;  // Starting position of the object

    void Update()
    {
        // Move the object to the left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Check if the object has moved off screen
        if (transform.position.x <= resetPositionX)
        {
            // Reset the object's position to the starting position
            Vector3 newPosition = transform.position;
            newPosition.x = startPositionX;
            transform.position = newPosition;
        }
    }
}