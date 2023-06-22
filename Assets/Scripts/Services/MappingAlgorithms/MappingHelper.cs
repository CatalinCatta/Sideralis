using UnityEngine;
using System.Collections.Generic;

public class MappingHelper
{
    public GameObject[,] Ship;
    public readonly int Rows;
    public readonly int Columns;

    public MappingHelper(GameObject[,] ship)
    {
        Rows = ship.GetLength(0);
        Columns = ship.GetLength(1);
        CopyShipArray(ship);
    }
    
    private void CopyShipArray(GameObject[,] original)
    {
        Ship = new GameObject[Rows, Columns];

        for (var i = 0; i < Rows; i++)
            for (var j = 0; j < Columns; j++)
                Ship[i, j] = original[i, j];
    }
     
     public bool IsNotValid((int x, int y) nextPosition, (int x, int y) currentPosition, IReadOnlyList<int> direction) =>
        !Utilities.CheckIfValid(nextPosition.x, nextPosition.y, Rows, Columns) ||
        Ship[nextPosition.x, nextPosition.y] == null ||
              (!IsValidRoad(nextPosition.x, nextPosition.y, direction[0] * -1, direction[1] * -1) &&
               !Ship[nextPosition.x, nextPosition.y].TryGetComponent<Room>(out _)) ||
              (!Ship[currentPosition.x, currentPosition.y].TryGetComponent<Room>(out _) &&
               !IsValidRoad(currentPosition.x, currentPosition.y, direction[0], direction[1]));
     
    public bool IsValidRoad(int x, int y, int directionX, int directionY)
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
                   (roadRotation == Quaternion.Euler(0f, 0f, 0f) || roadRotation == Quaternion.Euler(0f, 0f, 270f))))) ||
                (road is TRoad &&
                 ((directionX == -1 &&
                   roadRotation != Quaternion.Euler(0f, 0f, 0f)) ||
                  (directionX == +1 &&
                   roadRotation != Quaternion.Euler(0f, 0f, 180f))||
                  (directionY == -1 &&
                   roadRotation != Quaternion.Euler(0f, 0f, 90f)) ||
                  (directionY == +1 &&
                   roadRotation != Quaternion.Euler(0f, 0f, 270f)))));
    }

}
