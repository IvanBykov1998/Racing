using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class EngineIndicatorColor
{
    public float MaxRpm;
    public Color color;
}

public class CarEngineRpm : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Image engineRpmFill;
    [SerializeField] private EngineIndicatorColor[] colors;

    private void Update()
    {
        engineRpmFill.fillAmount = car.EngineRpm / car.EngineMaxRpm;

        for (int i = 0; i < colors.Length; i++)
        {
            if (car.EngineRpm <= colors[i].MaxRpm)
            {
                engineRpmFill.color = colors[i].color;
                break;
            }
        }
    }
}