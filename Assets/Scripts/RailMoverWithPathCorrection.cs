using UnityEngine;

public class RailMoverWithPathCorrection : MonoBehaviour
{
    public WaypointsGenerator waypointManager; // Reference to the WaypointsGenerator script
    public float baseSpeed = 3.0f; // Base speed of the ball along the path
    public float gravityEffect = 12.81f; // Simulated gravity effect
    public float dragEffect = 0.1f; // Simulated drag effect

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private Vector3[] waypoints;
    private Vector3 initialVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity to follow the path exactly

        // Get waypoints from the WaypointsGenerator
        if (waypointManager != null)
        {
            waypointManager.RotateWaypoints(new Vector3(0f,0f,0f));
            waypoints = waypointManager.rotatedWaypoints;
        }

        // Start the ball movement along the waypoints
        StartMovement();
    }

    void StartMovement()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            currentWaypointIndex = 0;
            MoveToNextWaypoint();
        }
    }

    void FixedUpdate()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex + 1];

            // Calculate direction and velocity from current position to target position
            Vector3 direction = (targetPosition - currentPosition).normalized;
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Calculate the base velocity
            Vector3 baseVelocity = direction * baseSpeed;

            // Simulate gravity by adjusting the y-component of the velocity
            if (targetPosition.y > currentPosition.y)
            {
                baseVelocity.y -= gravityEffect * Time.fixedDeltaTime;
            }
            else if (targetPosition.y < currentPosition.y)
            {
                baseVelocity.y += gravityEffect * Time.fixedDeltaTime;
            }

            // Simulate drag by reducing the z-component of the velocity
            baseVelocity.z *= (1 - dragEffect * Time.fixedDeltaTime);

            // Apply the calculated velocity to the Rigidbody
            rb.velocity = Vector3.Lerp(rb.velocity, baseVelocity, Time.fixedDeltaTime);

            // Check if we reached the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    rb.velocity = Vector3.zero; // Stop movement at the end of the rail
                }
                else
                {
                    MoveToNextWaypoint();
                }
            }
        }
    }

    void MoveAlongPathWithoutInterpolation()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = waypoints[currentWaypointIndex + 1];

            // Calculate direction from current position to target position
            Vector3 direction = (targetPosition - currentPosition).normalized;
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Calculate the base velocity
            Vector3 baseVelocity = direction * baseSpeed;

            // Simulate gravity by adjusting the y-component of the velocity
            if (targetPosition.y > currentPosition.y)
            {
                baseVelocity.y -= gravityEffect * Time.fixedDeltaTime;
            }
            else if (targetPosition.y < currentPosition.y)
            {
                baseVelocity.y += gravityEffect * Time.fixedDeltaTime;
            }

            // Simulate drag by reducing the z-component of the velocity
            baseVelocity.z *= (1 - dragEffect * Time.fixedDeltaTime);

            // Apply the calculated velocity to the Rigidbody
            rb.velocity = baseVelocity;

            // Check if we reached the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    rb.velocity = Vector3.zero; // Stop movement at the end of the rail
                }
                else
                {
                    MoveToNextWaypoint();
                }
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 nextWaypoint = waypoints[currentWaypointIndex + 1];
            Vector3 direction = (nextWaypoint - transform.position).normalized;
            rb.velocity = direction * baseSpeed;
        }
    }
}
