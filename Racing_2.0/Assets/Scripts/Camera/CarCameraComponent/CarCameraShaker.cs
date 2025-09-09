using UnityEngine;

public class CarCameraShaker : CarCameraComponent
{
    [SerializeField] private ParticleSystem windVisualEffect;
    [SerializeField][Range(0.0f, 1.0f)] private float normalizeSpeedShake;
    [SerializeField] private float shakeAmount;

    private void Update()
    {

        if (car.NormalizeLinearVelosity >= normalizeSpeedShake)
        {
            windVisualEffect.Emit(1);
            transform.localPosition += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
        }

        windVisualEffect.Stop();
    }
}