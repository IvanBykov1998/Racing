using UnityEngine;

public class CarGearboxSound : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AudioSource gearboxSound;

    private void Start()
    {
        car.GearIndex += GearChangedSound;
    }

    private void OnDestroy()
    {
        car.GearIndex -= GearChangedSound;
    }

    private void GearChangedSound(int gearIndex)
    {
        if (gearIndex > 0)
        {
            gearboxSound.Play();
        }
    }
}