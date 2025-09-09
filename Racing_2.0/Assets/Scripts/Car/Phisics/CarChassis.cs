using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxles;
    [SerializeField] private float WheelBaseLength;

    [SerializeField] private Transform centerOfMass;

    [Header("AngularDamping")]
    [SerializeField] private float angularDampingMin;
    [SerializeField] private float angularDampingMax;
    [SerializeField] private float angularDampingFactor;

    [Header("DownForce")]
    [SerializeField] private float downForceMin;
    [SerializeField] private float downForceMax;
    [SerializeField] private float downForceFactor;

    // DEBUG
    public float SteerTorque;
    public float MotorTorque;
    public float BrakeTorque;

    public float LinearVelocity => rigidbody.linearVelocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null ? GetComponent<Rigidbody>(): rigidbody;

    public float GetAvarageRpm()
    {
        float sum = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }

        return sum / wheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAvarageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (centerOfMass != null)
        {
            rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].ConfigureVehicleSubsteps(50, 50, 50);
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDamping();
        UpdateDownForce();
        UpdateWheelAxles();
    }

    private void UpdateAngularDamping()
    {
        rigidbody.angularDamping = Mathf.Clamp(angularDampingFactor * LinearVelocity, angularDampingMin, angularDampingMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            if (wheelAxles[i].IsMotor == true)
            {
                amountMotorWheel += 2;
            }
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();

            wheelAxles[i].ApplySteerAngle(SteerTorque, WheelBaseLength);
            wheelAxles[i].ApplyMotorTorque(MotorTorque / amountMotorWheel);
            wheelAxles[i].ApplyBrakeTorque(BrakeTorque);
        }
    }

    public void Reset()
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}