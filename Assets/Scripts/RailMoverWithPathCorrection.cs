using UnityEngine;

public class RailMoverWithPathCorrection : MonoBehaviour
{
    public WaypointsGenerator waypointManager; // Reference to the WaypointsGenerator script
    private float baseSpeed = 3.6f; // Base speed of the ball along the path
    private float speedIncrement = 1.5f; // Speed increment per waypoint
    public Collider target;
    public Collider bullseye;
    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private Vector3[] waypoints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity to follow the path exactly

        // Rotate waypoints and get the rotated waypoints from the WaypointsGenerator
        if (waypointManager != null)
        {
            Vector3 offset = bullseye.transform.position - target.transform.position;
            waypointManager.RotateAndStretchWaypoints(offset);// (new Vector3(0f, 3f, 2f));
            waypoints = waypointManager.rotatedWaypoints;
            Debug.Log(waypoints.Length);
        }

        // Start the ball movement along the waypoints
        StartMovement();
    }

    void StartMovement()
    {
        if (waypoints != null && waypoints.Length > 1)
        {
            currentWaypointIndex = 0;
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

            // Calculate the base velocity and apply incremental speed
            float speed = baseSpeed + currentWaypointIndex * speedIncrement;
            Vector3 baseVelocity = direction * speed;

            // Apply the calculated velocity to the Rigidbody
            rb.velocity = baseVelocity;

            // Check if we reached the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) < 0.3f)
            {
                currentWaypointIndex++;
                Debug.Log(currentWaypointIndex);
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    rb.velocity = Vector3.zero; // Stop movement at the end of the rail
                    Debug.Log("done");
                    gameObject.SetActive(false);
                }
                
            }
        }
    }
}
