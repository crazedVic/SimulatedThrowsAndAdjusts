using UnityEngine;

public class RailMoverWithConstantSpeed : MonoBehaviour
{
    public WaypointsGenerator waypointManager; // Reference to the WaypointsGenerator script
    public float movementSpeed = 2.0f; // Constant speed of movement

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private Vector3[] waypoints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Ensure gravity affects the object

        // Get waypoints from the WaypointsGenerator
        if (waypointManager != null)
        {
            waypoints = waypointManager.waypoints;
        }
    }

    void FixedUpdate()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            MoveAlongRail();
        }
    }

    void MoveAlongRail()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex + 1];

            // Calculate direction from current position to target position
            Vector3 direction = (targetPosition - currentPosition).normalized;

            // Set the velocity to move towards the next waypoint
            rb.velocity = direction * movementSpeed;

            // Check if we reached the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    rb.velocity = Vector3.zero; // Stop movement at the end of the rail
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw the rail in the Scene view for debugging
        Gizmos.color = Color.red;
        if (waypointManager != null && waypointManager.waypoints != null)
        {
            for (int i = 0; i < waypointManager.waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypointManager.waypoints[i], waypointManager.waypoints[i + 1]);
            }
        }
    }
}
