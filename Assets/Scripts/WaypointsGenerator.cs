using UnityEngine;

public class WaypointsGenerator : MonoBehaviour
{
    public Vector3[] waypoints;
    public Vector3[] rotatedWaypoints;

    // Method to get rotated waypoints based on an offset
    public void RotateWaypoints(Vector3 offset)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints array is empty or not initialized.");
            return; // Return null if waypoints array is empty or not initialized
        }

        // Calculate the current final direction
        Vector3 currentFinalDirection = waypoints[waypoints.Length - 1] - waypoints[0];
        Debug.Log($"Current final direction: {currentFinalDirection}");

        // Calculate the new final position
        Vector3 newFinalPosition = waypoints[waypoints.Length - 1] + offset;
        Debug.Log($"New final position: {newFinalPosition}");

        // Calculate the new final direction
        Vector3 newFinalDirection = newFinalPosition - waypoints[0];
        Debug.Log($"New final direction: {newFinalDirection}");

        // Calculate the rotation required to align the current final direction with the new final direction
        Quaternion rotation = Quaternion.FromToRotation(currentFinalDirection, newFinalDirection);
        Debug.Log($"Calculated rotation: {rotation.eulerAngles}");

        // Apply the rotation to all waypoints
        rotatedWaypoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 directionFromStart = waypoints[i] - waypoints[0];
            Vector3 rotatedDirection = rotation * directionFromStart;
            rotatedWaypoints[i] = waypoints[0] + rotatedDirection;
        }

        // Log the rotated waypoints for debugging
        for (int i = 0; i < rotatedWaypoints.Length; i++)
        {
            Debug.Log($"Rotated waypoint {i}: {rotatedWaypoints[i]}");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (rotatedWaypoints != null)
        {
            for (int i = 0; i < rotatedWaypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(rotatedWaypoints[i], rotatedWaypoints[i + 1]);
            }
        }
    }
}
