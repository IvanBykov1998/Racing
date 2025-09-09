using UnityEngine;
using UnityEngine.Events;

public enum RaceStat
{
    Preparation,
    Countdown,
    Race,
    Passed
}

public class RaceStateTracker : MonoBehaviour, IDependency<TrackpointCircuit>
{
    public event UnityAction PreparationStarted;
    public event UnityAction Started;
    public event UnityAction Completed;
    public event UnityAction<TrackPoint> TrackPointPassed;
    public event UnityAction<int> LapCompleted;

    private TrackpointCircuit trackpointCircuit;
    public void Construct(TrackpointCircuit obj) => trackpointCircuit = obj;

    [SerializeField] private Timer countdownTimer;
    public Timer CountdownTimer => countdownTimer;

    [SerializeField] private int lapsToComplete;
    public int LapsToComplete => lapsToComplete;

    private RaceStat state;
    public RaceStat State => state;
    
    private void StartState(RaceStat state)
    {
        this.state = state;
    }

    private void Start()
    {
        StartState(RaceStat.Preparation);

        countdownTimer.enabled = false;

        countdownTimer.Finished += OnCountdownTimerFinished;

        trackpointCircuit.TrackPointTriggered += OnTrackPointTriggered;
        trackpointCircuit.LapCompleted += OnLapCompleted;
    }

    private void OnDestroy()
    {
        trackpointCircuit.TrackPointTriggered -= OnTrackPointTriggered;
        trackpointCircuit.LapCompleted -= OnLapCompleted;
        countdownTimer.Finished -= OnCountdownTimerFinished;
    }

    private void OnTrackPointTriggered(TrackPoint trackPoint)
    {
        TrackPointPassed?.Invoke(trackPoint);
    }

    private void OnLapCompleted(int lapAmount)
    {
        if (trackpointCircuit.Type == TrackType.Sprint)
        {
            CompleteRace();
        }

        if (trackpointCircuit.Type == TrackType.Circular)
        {
            if (lapAmount == lapsToComplete)
            {
                CompleteRace();
            }
            else
            {
                CompleteLap(lapAmount);
            }
        }
    }

    private void OnCountdownTimerFinished()
    {
        StartRace();
    }

    public void LaunchPreparationStart()
    {
        if (state != RaceStat.Preparation) return;
        StartState(RaceStat.Countdown);

        countdownTimer.enabled = true;
        PreparationStarted?.Invoke();
    }

    private void StartRace()
    {
        if (state != RaceStat.Countdown) return;

        StartState(RaceStat.Race);

        Started?.Invoke();
    }

    private void CompleteRace()
    {
        if (state != RaceStat.Race) return;

        StartState(RaceStat.Passed);

        Completed?.Invoke();
    }

    private void CompleteLap(int lapAmount)
    {
        LapCompleted?.Invoke(lapAmount);
    }
}