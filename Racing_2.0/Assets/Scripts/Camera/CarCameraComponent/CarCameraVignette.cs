using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CarCameraVignette : CarCameraComponent
{
    [SerializeField] private PostProcessVolume processVolume;
    [SerializeField] private AnimationCurve vignetteValueCurve;
    [SerializeField] private AnimationCurve chromaticValueCurve;

    private Vignette vignette;
    private ChromaticAberration chromatic;

    private void Start()
    {
        if (processVolume != null)
        {
            vignette = processVolume.profile.GetSetting<Vignette>();
            chromatic = processVolume.profile.GetSetting<ChromaticAberration>();
        }
    }

    private void Update()
    {
        if (vignette != null)
        {
            vignette.intensity.value = vignetteValueCurve.Evaluate(car.LinearVelocity / car.MaxSpeed);
        }

        if (chromatic != null)
        {
            chromatic.intensity.value = chromaticValueCurve.Evaluate(car.LinearVelocity / car.MaxSpeed);
        }
    }
}