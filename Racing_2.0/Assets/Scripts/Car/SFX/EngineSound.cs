using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AudioSource engineAudioSource;

    [SerializeField] private float pitchMofifier;
    [SerializeField] private float voleuModifier;
    [SerializeField] private float rpmModifier;

    [SerializeField] private float basePitch = 1.0f;
    [SerializeField] private float baseVolume = 0.4f;

    private void Start()
    {
        engineAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        engineAudioSource.pitch = basePitch + pitchMofifier * ((car.EngineRpm / car.EngineMaxRpm) * rpmModifier);
        engineAudioSource.volume = baseVolume + voleuModifier * (car.EngineRpm / car.EngineMaxRpm);
    }
}