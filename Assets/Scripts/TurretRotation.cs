using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [Header("Waypoint Coordinates (Z-axis Only)")]
    public float waypoint1Z;  // First waypoint Z position
    public float waypoint2Z;  // Second waypoint Z position

    [Header("Rotation Speed")]
    public float rotationSpeed = 2.0f;  // Speed at which the turret rotates between waypoints

    private float currentTargetZ;

    void Start()
    {
        // Initially set the target to waypoint1
        currentTargetZ = waypoint1Z;
    }

    void Update()
    {
        RotateTurret();
    }

    void RotateTurret()
    {
        // Calculate the new Z position for rotation
        Vector3 currentRotation = transform.eulerAngles;
        float targetZ = currentTargetZ;

        // Smoothly rotate towards the target Z position
        currentRotation.z = Mathf.MoveTowardsAngle(currentRotation.z, targetZ, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = currentRotation;

        // Check if we have reached the target position (Z-axis only)
        if (Mathf.Approximately(currentRotation.z, targetZ))
        {
            // Alternate between waypoint1Z and waypoint2Z
            currentTargetZ = (currentTargetZ == waypoint1Z) ? waypoint2Z : waypoint1Z;
        }
    }
}
