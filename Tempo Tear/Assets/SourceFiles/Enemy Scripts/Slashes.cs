using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slashes : MonoBehaviour
{
    // Initialization
    void Start()
    {
        // Starting position
        Vector3 starting;

        // Checks if pattern spawned on the right
        if (transform.position.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            starting = new Vector3(transform.position.x - 1.1f, -1.3f, 0f);
        }
        // Checks if pattern spawned on the left
        else
        {
            starting = new Vector3(transform.position.x + 1.1f, -1.3f, 0f);
        }
        
        // Moves pattern to starting position
        transform.position = starting;

        // Set patterns to black if on level 1
        if (Level.level == 1)
        {
            this.GetComponent<SpriteRenderer>().material.color = new Color(0, 0, 0);
        }
    }

    // Deletes slash pattern from screen
    public void DeletePattern()
    {
        Destroy(gameObject);
    }
}
