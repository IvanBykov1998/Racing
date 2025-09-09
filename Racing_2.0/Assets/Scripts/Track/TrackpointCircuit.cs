using UnityEngine;
using UnityEngine.Events;

public enum TrackType
{
    Circular,
    Sprint
}

public class TrackpointCircuit : MonoBehaviour
{
    public event UnityAction<TrackPoint> TrackPointTriggered;
    public event UnityAction<int> LapCompleted;
    public event UnityAction<int> CurentPoint; // для UITrackPoint = текущее кол-во пройденных точек/кругов


    [SerializeField] private TrackType type;
    public TrackType Type => type;

    private TrackPoint[] points;
    public int LengthPoints => points.Length - 1;

    private int lapsCompleted = -1;
    public int LapsCompleted => lapsCompleted;

    private int currentPoint = -1;
    public int CurrentPoint => currentPoint;

    private void Awake()
    {
        BuildCircuit();
    }

    private void Start()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].Triggered += OnTrackPointTriggered;
        }

        points[0].AssignAsTarget();
    }
        
    private void OnDestroy()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].Triggered -= OnTrackPointTriggered;
        }
    }

    private void OnTrackPointTriggered(TrackPoint trackPoint)
    {
        if (trackPoint.IsTarget == false) return;

        trackPoint.Passed();
        trackPoint.Next?.AssignAsTarget();

        currentPoint++;

        if (type == TrackType.Sprint)
        {
            CurentPoint?.Invoke(currentPoint); // передаем текущую пройденую точку в UITrackType
        }

        TrackPointTriggered?.Invoke(trackPoint);

        if (trackPoint.isLast == true)
        {
            lapsCompleted++;

            if (type == TrackType.Sprint)
            {
                LapCompleted?.Invoke(lapsCompleted);
            }

            if (type == TrackType.Circular)
            {
                CurentPoint?.Invoke(lapsCompleted); // передаем текущий пройденный круг в UITrackType

                if (lapsCompleted > 0)
                {
                    LapCompleted?.Invoke(lapsCompleted);
                }
            }
        }
    }

    [ContextMenu(nameof(BuildCircuit))]
    private void BuildCircuit()
    {
        points = TrackCircuitBuilder.Build(transform, type);
    }
}