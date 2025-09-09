using UnityEngine;

public static class TrackCircuitBuilder
{
    public static TrackPoint[] Build(Transform trackTransform, TrackType type)
    {
        TrackPoint[] points = new TrackPoint[trackTransform.childCount];

        ResetPoints(trackTransform, points);
        MakeLincs(points, type);
        MarkPoint(points, type);

        return points;
    }

    private static void ResetPoints(Transform trackTransform, TrackPoint[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = trackTransform.GetChild(i).GetComponent<TrackPoint>();

            if (points[i] == null)
            {
#if UNITY_EDITOR
                Debug.LogError("There if no TraclPoint script on the one of the child objects");
#endif
                return;
            }

            points[i].Reset();
        }
    }

    private static void MakeLincs(TrackPoint[] points, TrackType type)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            points[i].Next = points[i + 1];
        }

        if (type == TrackType.Circular)
        {
            points[points.Length - 1].Next = points[0];
        }
    }

    private static void MarkPoint(TrackPoint[] points, TrackType type)
    {
        points[0].isFirst = true;

        if (type == TrackType.Sprint)
        {
            points[points.Length - 1].isLast = true;
        }

        if (type == TrackType.Circular)
        {
            points[0].isLast = true;
        }
    }
}