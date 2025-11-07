using UnityEngine;

public static class RectTransformExtension
{
    public static bool IsOverlapping(this RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetScreenRect(rect1);
        Rect r2 = GetScreenRect(rect2);
        return r1.Overlaps(r2);
    }

    private static Rect GetScreenRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float xMin = corners[0].x;
        float xMax = corners[2].x;
        float yMin = corners[0].y;
        float yMax = corners[2].y;
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}