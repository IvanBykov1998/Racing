using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class AudioMixerFloatSetting : Setting
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string nameParameter;

    [SerializeField] private float minRealValue;
    [SerializeField] private float maxRealValue;

    [SerializeField] private float virtualStep;
    [SerializeField] private float minVirtualValue;
    [SerializeField] private float maxVirtualValue;

    private float currentValue = 0f;

    public override bool IsMinValue { get => currentValue <= minRealValue + 0.5f; }
    public override bool IsMaxValue { get => currentValue >= maxRealValue - 0.5f; }

    public override void SetNextValue()
    {
        AddValue(Mathf.Abs(maxRealValue - minRealValue) / virtualStep);
    }

    public override void SetPreviousValue()
    {
        AddValue(-Mathf.Abs(maxRealValue - minRealValue) / virtualStep);
    }

    public override object GetValue()
    {
        return currentValue;
    }

    public override string GetStringValue()
    {
        return Mathf.Lerp(minVirtualValue, maxVirtualValue, (currentValue - minRealValue) / (maxRealValue - minRealValue)).ToString("F0");
    }

    public override void Apply()
    {
        audioMixer.SetFloat(nameParameter, currentValue);

        Save();
    }

    public override void Load()
    {
        currentValue = PlayerPrefs.GetFloat(Title, 0);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(Title, currentValue);
    }

    private void AddValue(float value)
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, minRealValue, maxRealValue);
    }
}