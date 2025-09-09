using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WindEffect : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AudioSource windSound;
    [SerializeField] private AnimationCurve volumeCurve;
    [SerializeField] private float basePitch = 1.0f;

    private void Start()
    {
        windSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        windSound.pitch = basePitch + volumeCurve.Evaluate(car.LinearVelocity / car.MaxSpeed);
        windSound.volume = volumeCurve.Evaluate(car.LinearVelocity / car.MaxSpeed);
    }
}