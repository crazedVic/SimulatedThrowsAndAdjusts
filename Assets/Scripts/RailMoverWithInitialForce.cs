using UnityEngine;

public class RailMoverWithInitialForce : MonoBehaviour
{
    public WaypointsGenerator waypointManager; // Reference to the WaypointsGenerator script
    public float initialForceMagnitude = 10.0f; // Magnitude of the initial force
    public float waypointThreshold = 0.1f; // Distance to switch to the next waypoint

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private Vector3[] waypoints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.1f; // Adjust drag to simulate air friction
        rb.useGravity = true; // Ensure gravity affects the object

        // Get waypoints from the WaypointsGenerator
        if (waypointManager != null)
        {
            waypoints = waypointManager.waypoints;
        }

        // Apply initial force
        ApplyInitialForce();
    }

    void ApplyInitialForce()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            Vector3 direction = (waypoints[1] - waypoints[0]).normalized;
            rb.AddForce(direction * initialForceMagnitude, ForceMode.Impulse);
            isMoving = true;
        }
        else
        {
            Debug.LogError("No Waypoints found");
        }
    }

    void FixedUpdate()
    {
        if (isMoving && waypoints != null && waypoints.Length > 1)
        {
            ConstrainMovementToRail();
        }
    }

    void ConstrainMovementToRail()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex + 1];

            // Calculate direction from current position to target position
            Vector3 direction = (targetPosition - currentPosition).normalized;

            // Project the velocity onto the direction of the rail
            Vector3 projectedVelocity = Vector3.Project(rb.velocity, direction);

            // Apply the projected velocity to the Rigidbody, but keep y-velocity affected by gravity
            rb.velocity = new Vector3(projectedVelocity.x, projectedVelocity.y, projectedVelocity.z);

            // Check if we reached the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) < waypointThreshold)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    isMoving = false; // Stop movement at the end of the rail
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
