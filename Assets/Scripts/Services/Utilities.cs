﻿using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        if (currentTransform.TryGetComponent<SpriteRenderer>(out var spriteRenderer1))
            spriteRenderer1.color =  newColor;

        if (currentTransform.TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI1))
            textMeshProUGUI1.color =  newColor;
            
        if (currentTransform.TryGetComponent<Image>(out var image1))
            image1.color =  newColor;
        
        foreach (Transform child in currentTransform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                spriteRenderer.color =  newColor;

            if (child.TryGetComponent<TextMeshProUGUI>(out var textMeshProUGUI))
                textMeshProUGUI.color =  newColor;
            
            if (child.TryGetComponent<Image>(out var image))
                image.color =  newColor;

            SetColor(child, newColor);
        }
    }

    public static ObjectSize GetSizeOfObject(ObjectType objectType) => objectType switch
        {
            ObjectType.SmallConstructPlace => ObjectSize.Small,
            ObjectType.MediumConstructPlace => ObjectSize.Medium,
            ObjectType.MediumConstructPlaceRotated => ObjectSize.MediumRotated,
            ObjectType.LargeConstructPlace => ObjectSize.Large,
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
            ObjectType.TRoad => ObjectSize.Small,
            ObjectType.TRoadRotated90 => ObjectSize.Small,
            ObjectType.TRoadRotated180 => ObjectSize.Small,
            ObjectType.TRoadRotated270 => ObjectSize.Small,
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

            ObjectType.TRoad => rotation.eulerAngles.z switch
            {
                90 => ObjectType.TRoadRotated90,
                180 => ObjectType.TRoadRotated180,
                270 => ObjectType.TRoadRotated270,
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

    public static string DoubleToTime(double seconds)
    {
        var timeSpan = TimeSpan.FromSeconds(seconds);

        string formattedTime;

        if (timeSpan.TotalDays >= 1)
            formattedTime =
                $"{(int)timeSpan.TotalDays}d {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        else if (timeSpan.TotalHours >= 1)
            formattedTime = 
                $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        else if (timeSpan.TotalMinutes >= 1)
            formattedTime = 
                $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        else
            formattedTime = 
                $"{timeSpan.Seconds:D2}";

        return formattedTime;
    }
}