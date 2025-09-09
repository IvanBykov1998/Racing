using UnityEngine;
using UnityEngine.Rendering;

public class CarRespawner : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<Car>, IDependency<CarInputControl>
{
    [SerializeField] private float raspawnHeight;

    private TrackPoint respawnTrackPoint;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private Car car;
    public void Construct(Car obj) => car = obj;

    private CarInputControl carControl;
    public void Construct(CarInputControl obj) => carControl = obj;

    private void Start()
    {
        raceStateTracker.TrackPointPassed += OnTrackPointPassed;
    }

    private void OnDestroy()
    {
        raceStateTracker.TrackPointPassed -= OnTrackPointPassed;
    }

    private void OnTrackPointPassed(TrackPoint point)
    {
        respawnTrackPoint = point;
    }

    private void Respawn()
    {
        if (respawnTrackPoint == null) return;

        if (raceStateTracker.State != RaceStat.Race) return;

        carControl.Reset();

        car.Respawn(respawnTrackPoint.transform.position + respawnTrackPoint.transform.up * raspawnHeight, respawnTrackPoint.transform.rotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            Respawn();
        }
    }
}