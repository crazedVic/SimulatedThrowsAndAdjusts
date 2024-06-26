using UnityEngine;

public class RailMoverWithInitialVelocity: MonoBehaviour
{
    public WaypointsGenerator waypointManager; // Reference to the WaypointsGenerator script
    public float initialVelocityMagnitude = 10.0f; // Magnitude of the initial velocity
    public float correctionFactor = 0.1f; // Factor to correct the ball's path

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
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

        // Apply initial velocity
        ApplyInitialVelocity();
    }

    void ApplyInitialVelocity()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            Vector3 direction = (waypoints[1] - waypoints[0]).normalized;
            rb.velocity = direction * initialVelocityMagnitude;
        }
    }

    void FixedUpdate()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            CorrectPath();
        }
    }

    void CorrectPath()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex + 1];

            // Calculate direction from current position to target position
            Vector3 desiredDirection = (targetPosition - currentPosition).normalized;

            // Set the position to stay exactly on the path
            Vector3 desiredPosition = Vector3.MoveTowards(currentPosition, targetPosition, rb.velocity.magnitude * Time.fixedDeltaTime);
            rb.MovePosition(desiredPosition);

            // Adjust velocity to match the direction of movement
            rb.velocity = desiredDirection * rb.velocity.magnitude;

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
}
