using System.Collections.Generic;
using UnityEngine;

public class RoomMerger : MonoBehaviour
{
    private SpaceShipManager _shipManager;
    [SerializeField] private GameObject mergeButton;
    [SerializeField] private GameObject _parent;
    
    public void Start()
    {
        _shipManager = transform.GetComponent<SpaceShipManager>();
    }

    public void CreateMergingPoints()
    {

        var mergingPoints = GetAllMergingPoints();

        foreach (var mergingPoint in mergingPoints)
        {
            Instantiate(mergeButton, mergingPoint, transform.rotation, _parent.transform);
        }
    }
    
    private List<Vector3> GetAllMergingPoints()
    {
        var rows = _shipManager.Ship.GetLength(0);
        var cols = _shipManager.Ship.GetLength(1);
        var result = new List<Vector3>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var currentRoom = _shipManager.Ship[i, j];
                var rightRoom = j < cols - 1 ? _shipManager.Ship[i, j + 1] : null;
                var bottomRoom = i < rows - 1 ? _shipManager.Ship[i + 1, j] : null;
                if (currentRoom == null || !currentRoom.TryGetComponent<Room>(out _)) continue;

                if (j < cols - 1 && rightRoom != null && rightRoom.TryGetComponent<Room>(out var room)
                    && currentRoom.GetComponent<Room>().Lvl == room.Lvl &&
                    currentRoom != rightRoom)
                {
                    if (currentRoom.TryGetComponent<SmallRoom>(out _)
                        && rightRoom.TryGetComponent<SmallRoom>(out _))
                    {
                        result.Add(Utilities.GetInGameCoordinateForPosition(i, j + 0.5, -7f));
                    }
                    else if (currentRoom.TryGetComponent<MediumRoom>(out _) &&
                             rightRoom.TryGetComponent<MediumRoom>(out _) &&
                             currentRoom.transform.rotation == rightRoom.transform.rotation &&
                             currentRoom.transform.rotation == Quaternion.Euler(0, 0, 90))
                    {
                        if (i == 0 ||
                            (_shipManager.Ship[i - 1, j] == currentRoom &&
                             _shipManager.Ship[i - 1, j + 1] == rightRoom))
                        {
                            result.Add(Utilities.GetInGameCoordinateForPosition(i - 0.5, j + 0.5, -7f));
                        }
                        else if (i == rows - 1 ||
                                 (_shipManager.Ship[i + 1, j] == currentRoom &&
                                  _shipManager.Ship[i + 1, j + 1] == rightRoom))
                        {
                            result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j + 0.5, -7f));
                        }
                    }
                }

                if (i >= rows - 1 || bottomRoom == null || !bottomRoom.TryGetComponent<Room>(out var room2)
                    || currentRoom.GetComponent<Room>().Lvl != room2.Lvl ||
                    currentRoom == bottomRoom) 
                    continue;
                
                if (currentRoom.TryGetComponent<SmallRoom>(out _)
                    && bottomRoom.TryGetComponent<SmallRoom>(out _))
                {
                    result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j, -7f));
                }
                else if (currentRoom.TryGetComponent<MediumRoom>(out _) &&
                         bottomRoom.TryGetComponent<MediumRoom>(out _) &&
                         currentRoom.transform.rotation == bottomRoom.transform.rotation &&
                         currentRoom.transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    if (i == 0 ||
                        (_shipManager.Ship[i, j - 1] == currentRoom &&
                         _shipManager.Ship[i + 1, j - 1] == bottomRoom))
                    {
                        result.Add(Utilities.GetInGameCoordinateForPosition(i - 0.5, j + 0.5, -7f));
                    }
                    else if (i == rows - 1 ||
                             (_shipManager.Ship[i, j + 1] == currentRoom &&
                              _shipManager.Ship[i + 1, j + 1] == bottomRoom))
                    {
                        result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j + 0.5, -7f));
                    }
                }
            }
        }

        return result;
    }
}
