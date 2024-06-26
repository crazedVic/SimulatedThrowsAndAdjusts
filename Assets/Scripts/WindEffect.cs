using UnityEngine;
using System.Collections.Generic;

public class WindEffect : MonoBehaviour
{
    [Range(0f, 50f)]
    public float windForce = 10f; // Adjust the wind force as needed

    public enum WindDirection
    {
        Left,
        Right
    }

    public WindDirection windDirection = WindDirection.Right; // Default wind direction to Right

    // List to keep track of objects inside the wind zone
    private List<Rigidbody> objectsInWindZone = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mapper") && other.attachedRigidbody != null)
        {
            // Add the object to the list when it enters the collider
            objectsInWindZone.Add(other.attachedRigidbody);
            Debug.Log($"Entered Wind box at {other.ClosestPoint(transform.position)}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mapper") && other.attachedRigidbody != null)
        {
            // Remove the object from the list when it exits the collider
            objectsInWindZone.Remove(other.attachedRigidbody);
            Debug.Log($"Exited Wind box at {other.ClosestPoint(transform.position)}");
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (windDirection == WindDirection.Right) ? Vector3.right : Vector3.left;
        // Apply continuous wind force to all objects in the list
        foreach (Rigidbody rb in objectsInWindZone)
        {
            rb.AddForce(direction * windForce * Time.fixedDeltaTime, ForceMode.Force);
        }
    }
}
