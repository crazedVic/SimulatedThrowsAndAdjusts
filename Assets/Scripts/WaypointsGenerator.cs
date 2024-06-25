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


        // Calculate the new final position
        Vector3 newFinalPosition = waypoints[waypoints.Length - 1] + offset;


        // Calculate the new final direction
        Vector3 newFinalDirection = newFinalPosition - waypoints[0];


        // Calculate the rotation required to align the current final direction with the new final direction
        Quaternion rotation = Quaternion.FromToRotation(currentFinalDirection, newFinalDirection);


        // Apply the rotation to all waypoints
        rotatedWaypoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 directionFromStart = waypoints[i] - waypoints[0];
            Vector3 rotatedDirection = rotation * directionFromStart;
            rotatedWaypoints[i] = waypoints[0] + rotatedDirection;
        }

        Debug.Log(rotatedWaypoints.Length);

    }

    public void RotateAndStretchWaypoints(Vector3 offset)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints array is empty or not initialized.");
            return;
        }

        // Calculate the current final direction
        Vector3 currentFinalDirection = waypoints[waypoints.Length - 1] - waypoints[0];

        // Calculate the new final position
        Vector3 newFinalPosition = waypoints[waypoints.Length - 1] + offset;

        // Calculate the new final direction
        Vector3 newFinalDirection = newFinalPosition - waypoints[0];

        // Calculate the rotation required to align the current final direction with the new final direction
        Quaternion rotation = Quaternion.FromToRotation(currentFinalDirection, newFinalDirection);

        // Determine the scale factor for the length of the new direction relative to the old direction
        float scale = newFinalDirection.magnitude / currentFinalDirection.magnitude;

        // Apply the rotation and scaling to all waypoints
        rotatedWaypoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 directionFromStart = waypoints[i] - waypoints[0];

            // Scale the direction
            directionFromStart *= scale;

            // Apply rotation
            Vector3 rotatedDirection = rotation * directionFromStart;

            // Translate back to the new start position
            rotatedWaypoints[i] = waypoints[0] + rotatedDirection;
        }
    }


    public void RotateAndStretchWaypointsOld(Vector3 offset)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints array is empty or not initialized.");
            return;
        }

        // Calculate the current final direction
        Vector3 currentFinalDirection = waypoints[waypoints.Length - 1] - waypoints[0];

        // Calculate the new final position
        Vector3 newFinalPosition = waypoints[waypoints.Length - 1] + offset;

        // Calculate the new final direction
        Vector3 newFinalDirection = newFinalPosition - waypoints[0];

        // Calculate the rotation required to align the current final direction with the new final direction
        Quaternion rotation = Quaternion.FromToRotation(currentFinalDirection, newFinalDirection);

        // Determine the scale factor for the z-axis and overall direction length
        float scale = newFinalDirection.magnitude / currentFinalDirection.magnitude;

        // Apply the rotation and scaling to all waypoints
        rotatedWaypoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 directionFromStart = waypoints[i] - waypoints[0];

            // Scale the direction
            directionFromStart *= scale;

            // Apply rotation
            Vector3 rotatedDirection = rotation * directionFromStart;
            rotatedWaypoints[i] = waypoints[0] + rotatedDirection;
        }

        Debug.Log(rotatedWaypoints.Length);
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
