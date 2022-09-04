using UnityEngine;

public static class BezierCurve
{
    public static Vector2 GetPoint(Vector2 start, Vector2 end, Vector2 startTangent, Vector2 endTangent, float t)
    {
        Debug.DrawLine(start, end, Color.red, 1f);
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * start +
            2f * oneMinusT * t * startTangent +
            t * t * endTangent +
            t * t * end;
    }
}