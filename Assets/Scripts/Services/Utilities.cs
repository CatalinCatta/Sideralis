using UnityEngine;

public static class Utilities
{
    public static readonly int[][] Directions =
    {
        new[] { -1, 0 },         // Up
        new[] { 1, 0 },          // Down
        new[] { 0, -1 },         // Left
        new[] { 0, 1 }           // Right
    };
    
    public static Vector3 GetInGameCoordinateForPosition(double x, double y, float z) => 
        new ((float)((y - 24.5) * 10), (float)((24.5 - x) * 10), z);

    public static (double, double) GetPositionInArrayOfCoordinate(Vector2 position) => 
        ((double)-Mathf.RoundToInt(position.y) / 10 + 24.5, (double)Mathf.RoundToInt(position.x) / 10 + 24.5);
    
    public static void SetTransparency(Transform currentTransform, float transparency)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                var currentColor = spriteRenderer.color;
                spriteRenderer.color =  new Color(currentColor.r, currentColor.g, currentColor.b, transparency);
            }

            SetTransparency(child, transparency);
        }
    }
}
