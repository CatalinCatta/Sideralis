using System;
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
    
    public static bool CheckIfValid(int x, int y, int rows, int columns) =>
        x >= 0 && x < rows && y >= 0 && y < columns;
    
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
    
    public static void SetColor(Transform currentTransform, Color newColor)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                spriteRenderer.color =  newColor;

            SetColor(child, newColor);
        }
    }

    public static ObjectSize GetSizeOfObject(ObjectType objectType) => objectType switch
        {
            ObjectType.ConstructPlace => ObjectSize.Small,
            ObjectType.ConstructRotatedPlace => ObjectSize.Small,
            ObjectType.SmallRoom => ObjectSize.Small,
            ObjectType.MediumRoom => ObjectSize.Medium,
            ObjectType.RotatedMediumRoom => ObjectSize.MediumRotated,
            ObjectType.BigRoom => ObjectSize.Large,
            ObjectType.Road => ObjectSize.Small,
            ObjectType.RoadRotated => ObjectSize.Small,
            ObjectType.CrossRoad => ObjectSize.Small,
            ObjectType.LRoad => ObjectSize.Small,
            ObjectType.LRoadRotated90 => ObjectSize.Small,
            ObjectType.LRoadRotated180 => ObjectSize.Small,
            ObjectType.LRoadRotated270 => ObjectSize.Small,
            ObjectType.Crew => ObjectSize.Small,
            ObjectType.Pointer => ObjectSize.Small,
            ObjectType.MergeButton => ObjectSize.Small,
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    
    public static ObjectType CheckObjectTypeIntegrity(ObjectType objectType, Quaternion rotation) =>  objectType switch
        {
            ObjectType.Road => rotation == Quaternion.Euler(0, 0, 90) ? ObjectType.RoadRotated : objectType,

            ObjectType.LRoad => rotation.eulerAngles.z switch
            {
                90 => ObjectType.LRoadRotated90,
                180 => ObjectType.LRoadRotated180,
                270 => ObjectType.LRoadRotated270,
                _ => objectType
            },

            ObjectType.MediumRoom => rotation == Quaternion.Euler(0, 0, 90) ? ObjectType.RotatedMediumRoom : objectType,

            _ => objectType
        };

    public static string DoubleToString(double number) => number switch
    {
        >= 100_000_000_000_000 => (number / 100_000_000_000_000).ToString("F0") + "t",

        >= 10_000_000_000_000 => (number / 10_000_000_000_000).ToString("F1") + "t",
        
        >= 1_000_000_000_000 => (number / 1_000_000_000_000).ToString("F2") + "t",

        >= 100_000_000_000 => (number / 100_000_000_000).ToString("F0") + "b",
        
        >= 10_000_000_000 => (number / 10_000_000_000).ToString("F1") + "b",
        
        >= 1_000_000_000 => (number / 1_000_000_000).ToString("F2") + "b",

        >= 100_000_000 => (number / 100_000_000).ToString("F0") + "m",
        
        >= 10_000_000 => (number / 10_000_000).ToString("F1") + "m",
        
        >= 1_000_000 => (number / 1_000_000).ToString("F2") + "m",

        >= 100_000 => (number / 100_000).ToString("F0") + "k",
        
        >= 10_000 => (number / 10_000).ToString("F1") + "k",
        
        >= 1_000 => (number / 1_000).ToString("F2") + "k",

        >= 100 => number.ToString("F0"),

        >= 10 => number.ToString("F1"),
        
        0 => "0",

        _ => number.ToString("F2")

    };
}