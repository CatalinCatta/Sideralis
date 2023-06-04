using UnityEngine;
using System.Collections.Generic;

public class MappingHelper
{
    public readonly GameObject[,] Ship;
    public readonly int Rows;
    public readonly int Columns;

    public MappingHelper(GameObject[,] ship)
    {
        Ship = ship;
        Rows = Ship.GetLength(0);
        Columns = Ship.GetLength(1);
    }
    
     public bool IsNotValid((int x, int y) nextPosition, (int x, int y) currentPosition, IReadOnlyList<int> direction) =>
        !IsValidPosition(nextPosition.x, nextPosition.y, Rows, Columns) ||
        Ship[nextPosition.x, nextPosition.y] == null ||
              (!IsValidRoad(nextPosition.x, nextPosition.y, direction[0] * -1, direction[1] * -1) &&
               !Ship[nextPosition.x, nextPosition.y].TryGetComponent<Room>(out _)) ||
              (!Ship[currentPosition.x, currentPosition.y].TryGetComponent<Room>(out _) &&
               !IsValidRoad(currentPosition.x, currentPosition.y, direction[0], direction[1]));
     
    private bool IsValidRoad(int x, int y, int directionX, int directionY)
    {
        var roadObject = Ship[x, y];
        var roadRotation = roadObject.transform.rotation;
        
        return roadObject.TryGetComponent<Road>(out var road) && 
               (road is CrossRoad ||
                (road is LinearRoad &&
                 ((roadRotation == Quaternion.Euler(0f, 0f, 0f) &&
                   directionX is -1 or +1)||
                  (roadRotation == Quaternion.Euler(0f, 0f, 90f) &&
                   directionY is -1 or +1))) ||
                (road is LRoad &&
                 ((directionX == -1 &&
                   (roadRotation == Quaternion.Euler(0f, 0f, 0f) || roadRotation == Quaternion.Euler(0f, 0f, 90f))) ||
                  (directionX == +1 &&
                   (roadRotation == Quaternion.Euler(0f, 0f, 180f) || roadRotation == Quaternion.Euler(0f, 0f, 270f))) ||
                  (directionY == -1 &&
                   (roadRotation == Quaternion.Euler(0f, 0f, 90f) || roadRotation == Quaternion.Euler(0f, 0f, 180f))) ||
                  (directionY == +1 &&
                   (roadRotation == Quaternion.Euler(0f, 0f, 0f) || roadRotation == Quaternion.Euler(0f, 0f, 270f))))));
    }

    private static bool IsValidPosition(int x, int y, int rows, int columns) =>
        x >= 0 && x < rows && y >= 0 && y < columns;
}
