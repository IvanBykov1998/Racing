using UnityEngine;
using UnityEngine.UI;

public class UITrackType : MonoBehaviour, IDependency<TrackpointCircuit>, IDependency<RaceStateTracker>
{
    [SerializeField] private GameObject typeObject;
    [SerializeField] private Text LableText;
    [SerializeField] private Text countPointText;

    private TrackpointCircuit trackpointCircuit;
    public void Construct(TrackpointCircuit obj) => trackpointCircuit = obj;

    private RaceStateTracker raceStateTracker;
    public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

    private int currentPoint = 0;
    private int countPoint;

    private void Start()
    {
        trackpointCircuit.CurentPoint += OnPointChanged;
        raceStateTracker.Started += OnRaceStarted;
        raceStateTracker.Completed += OnRaceCompleted;

        typeObject.SetActive(false);
    }

    private void OnDestroy()
    {
        trackpointCircuit.CurentPoint -= OnPointChanged;
        raceStateTracker.Started -= OnRaceStarted;
        raceStateTracker.Completed -= OnRaceCompleted;
    }

    private void OnRaceStarted()
    {
        typeObject.SetActive(true);

        if (trackpointCircuit.Type == TrackType.Circular)
        {
            LableText.text = "Круг:";
            countPoint = raceStateTracker.LapsToComplete;
            countPointText.text = $"{currentPoint} / {countPoint}";
        }

        if (trackpointCircuit.Type == TrackType.Sprint)
        {
            LableText.text = "Чекпоинт:";
            countPoint = trackpointCircuit.LengthPoints;
            countPointText.text = $"{currentPoint} / {countPoint}";
        }
    }

    private void OnRaceCompleted()
    {
        typeObject.SetActive(false);
    }

    private void OnPointChanged(int count)
    {
        currentPoint = count;

        countPointText.text = $"{currentPoint} / {countPoint}";
    }
}