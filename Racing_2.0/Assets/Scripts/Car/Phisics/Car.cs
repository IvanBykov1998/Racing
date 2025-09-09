using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    public event UnityAction<string> GearChanged;
    public event UnityAction<int> GearIndex;

    [SerializeField] private float maxSteerTorque;
    [SerializeField] private float maxBrakeTorque;

    [Header("Engine")]
    [SerializeField] private AnimationCurve engineTorqueCurve;
    [SerializeField] private float engineMaxTorque; // Максимальный крутящий момент
    // DEBUG
    [SerializeField] private float engineTorque; // Текущий крутящий момент
    // DEBUG
    [SerializeField] private float engineRpm; // Текущее число оборотов двигателя
    [SerializeField] private float engineMinRpm; // Минимальное число оборотов двигателя
    [SerializeField] private float engineMaxRpm; // Максимальное число оборотов двигателя

    // Для GUI
    public float EngineRpm => engineRpm;
    public float EngineMaxRpm => engineMaxRpm;

    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private float finalDriveRatio;
    // DEBUG
    [SerializeField] private int selectedGearIndex;
    public int SelectedGearIndex => selectedGearIndex;
    // DEBUG
    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRpm;
    [SerializeField] private float downShiftEngineRpm;

    [SerializeField] private float maxSpeed;

    public float LinearVelocity => chassis.LinearVelocity;
    public float NormalizeLinearVelosity => chassis.LinearVelocity / maxSpeed;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;

    private CarChassis chassis;
    public Rigidbody Rigidbody => chassis == null ? GetComponent<CarChassis>().Rigidbody: chassis.Rigidbody;

    // DEBUG
    public float Speed;
    public float ThrottleControl;
    public float SteerControl;
    public float BrakeControl;

    private void Start()
    {
        chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        Speed = LinearVelocity;

        UpdateEngineTorque();
        AutoGearShift();

        if (LinearVelocity >= maxSpeed)
        {
            engineTorque = 0;
        }

        chassis.MotorTorque = engineTorque * ThrottleControl;
        chassis.SteerTorque = maxSteerTorque * SteerControl;
        chassis.BrakeTorque = maxBrakeTorque * BrakeControl;
    }

    public string GetSelectedGearName()
    {
        if (selectedGear == rearGear) return "R";

        if (LinearVelocity < 1 && LinearVelocity > 1) return "N";

        return (selectedGearIndex + 1).ToString();
    }

    private void AutoGearShift() // Автоматическое переключение передач
    {
        if (selectedGear < 0) return;

        if (engineRpm >= upShiftEngineRpm)
        {
            UpGear();
        }

        if (engineRpm < downShiftEngineRpm)
        {
            DownGear();
        }
    }

#region ПЕРЕКЛЮЧЕНИЕ ПЕРЕДАЧ
    public void UpGear()
    {
        ShiftGear(selectedGearIndex + 1);
    }

    public void DownGear()
    {
        ShiftGear(selectedGearIndex - 1);
    }

    public void ShiftToReverGear()
    {
        selectedGear = rearGear;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }

    public void ShiftToNetral()
    {
        selectedGear = 0;
    }
    #endregion

    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);
        selectedGear = gears[gearIndex];

        if (selectedGearIndex != gearIndex)
        {
            GearIndex?.Invoke(gearIndex);
        }

        selectedGearIndex = gearIndex;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void UpdateEngineTorque()
    {
        engineRpm = engineMinRpm + Mathf.Abs(chassis.GetAvarageRpm() * selectedGear * finalDriveRatio);
        engineRpm = Mathf.Clamp(engineRpm, engineMinRpm, engineMaxRpm);

        engineTorque = engineTorqueCurve.Evaluate(engineRpm / engineMaxRpm) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear) * gears[0];
    }

    public void Reset()
    {
        chassis.Reset();

        chassis.MotorTorque = 0;
        chassis.SteerTorque = 0;
        chassis.BrakeTorque = 0;

        ThrottleControl = 0;
        SteerControl = 0;
        BrakeControl = 0;

    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();

        transform.position = position;
        transform.rotation = rotation;
    }
}