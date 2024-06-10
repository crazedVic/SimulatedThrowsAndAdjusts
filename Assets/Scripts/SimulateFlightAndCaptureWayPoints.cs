using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateFlightAndCaptureWaypoints : MonoBehaviour
{
    private Vector3 initialForce = new Vector3(0, 9, 6); // Initial force to be applied
    private float captureInterval = 0.05f; // Time interval to capture waypoints
    private List<Vector3> capturedWaypoints = new List<Vector3>();
    private Rigidbody rb;
    private float timeSinceLastCapture = 0f;
    private bool capturing = false;
    public GameObject pathSphere; // Reference to the sphere that will follow the path
    public GameObject waypointManager; // Reference to the WaypointsManager GameObject

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity initially
    }

    void Update()
    {
        // Check for space bar press to apply the initial force
        if (Input.GetKeyDown(KeyCode.Space) && !capturing)
        {
            ApplyInitialForce();
        }
    }

    void ApplyInitialForce()
    {
        // Apply initial force and enable gravity
        rb.AddForce(initialForce, ForceMode.Impulse);
        rb.useGravity = true;
        capturing = true;
    }

    void FixedUpdate()
    {
        if (capturing)
        {
            timeSinceLastCapture += Time.fixedDeltaTime;

            if (timeSinceLastCapture >= captureInterval)
            {
                capturedWaypoints.Add(transform.position);
                timeSinceLastCapture = 0f;
            }

            // Check if the ball has hit the ground, and allow a small threshold
            if (transform.position.y <= -0.01f)
            {
                capturing = false;

                // Save captured waypoints to WaypointsGenerator
                WaypointsGenerator waypointsGenerator = waypointManager.GetComponent<WaypointsGenerator>();
                waypointsGenerator.waypoints = capturedWaypoints.ToArray();

                // Reset ball state
                rb.velocity = Vector3.zero;
                rb.freezeRotation = true;
                rb.rotation = Quaternion.identity;
                rb.useGravity = false;

                // Optionally, deactivate this script and activate the RailMoverWithPathCorrection script
                this.enabled = false;
                pathSphere.SetActive(true);
                gameObject.SetActive(false);

                Debug.Log("Waypoints captured: " + capturedWaypoints.Count);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < capturedWaypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(capturedWaypoints[i], capturedWaypoints[i + 1]);
        }
    }
}
