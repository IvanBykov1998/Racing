using UnityEngine;

public class CarCameraFovCorrector : CarCameraComponent
{
    [SerializeField] private float minFieldOfView;
    [SerializeField] private float maxFieldOfView;

    private float defaultFow;

    private void Start()
    {
        camera.fieldOfView = defaultFow;
    }

    private void Update()
    {
        camera.fieldOfView = Mathf.Lerp(minFieldOfView, maxFieldOfView, car.NormalizeLinearVelosity);
    }
}