using UnityEngine;

public class RaceInputController : MonoBehaviour, IDependency<CarInputControl>, IDependency<RaceStateTracker>
{
    [SerializeField] private CarInputControl carControl;
    public void Construct(CarInputControl obj) => carControl = obj;

    [SerializeField] private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        raceStateTracker.Started += OnRaceStarted;
        raceStateTracker.Completed += OnRaceCompleted;

        carControl.enabled = false;
    }

    private void OnDestroy()
    {
        raceStateTracker.Started -= OnRaceStarted;
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceStarted()
    {
        carControl.enabled = true;
    }

    private void OnRaceCompleted()
    {
        carControl.Stop();
        carControl.enabled = false;
    }
}