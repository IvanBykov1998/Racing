using UnityEngine;

public class CarCameraFollow : CarCameraComponent
{
    [Header("Offset")]
    [SerializeField] private float viewHieght;
    [SerializeField] private float distance;
    [SerializeField] private float height;

    [Header("Damping")]
    [SerializeField] private float rotationDamping;
    [SerializeField] private float heightDamping;
    [SerializeField] private float speedThreshold;

    private Transform target;
    private Rigidbody carRigidbody;

    private void FixedUpdate()
    {
        Vector3 velocity = carRigidbody.linearVelocity;
        Vector3 targetRotation = target.eulerAngles;

        if (velocity.magnitude > speedThreshold)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles;
        }

        // Lerp
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.y, rotationDamping * Time.fixedDeltaTime);
        float currentHeight = Mathf.Lerp(transform.position.y, target.position.y + height, heightDamping * Time.fixedDeltaTime);

        // Position
        Vector3 positionOffset = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
        transform.position = target.position - positionOffset;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Rotation
        transform.LookAt(target.position + new Vector3(0, viewHieght, 0));
    }

    public override void SetProperties(Car car, Camera camera)
    {
        base.SetProperties(car, camera);

        target = car.transform;
        carRigidbody = car.Rigidbody;
    }
}