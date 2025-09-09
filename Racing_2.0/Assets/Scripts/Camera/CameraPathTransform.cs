using UnityEngine;

public class CameraPathTransform : MonoBehaviour, IDependency<Car>, IDependency<RaceStateTracker>
{
    private Car car;
    public void Construct(Car obj) => car = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private void Start()
    {
        transform.position = car.transform.position;
        raceStateTracker.Completed += OnRaceCompleted;
    }

    private void OnDestroy()
    {
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceCompleted()
    {
        transform.position = car.transform.position;
    }
}