using UnityEngine;
using UnityEngine.UI;

public class CarSpeedIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Text speedText;

    private void Update()
    {
        speedText.text = car.LinearVelocity.ToString("F0");
    }
}