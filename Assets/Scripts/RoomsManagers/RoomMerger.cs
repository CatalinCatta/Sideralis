using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class RoomMerger : MonoBehaviour
{
    private SpaceShipManager _shipManager;
    private ActorManager _actorManager;
    
    [SerializeField] public GameObject parent;
    
    public void Awake()
    {
        _shipManager = transform.GetComponent<SpaceShipManager>();
        _actorManager = transform.GetComponent<ActorManager>();
    }

    public void CreateMergingPoints()
    {
        foreach (var mergingPoint in GetAllMergingPoints())
            _actorManager.CreateObject(mergingPoint, ObjectType.MergeButton, parent.transform);
    }

    private IEnumerable<Vector3> GetAllMergingPoints()
    {
        var rows = _shipManager.Ship.GetLength(0);
        var cols = _shipManager.Ship.GetLength(1);
        var result = new List<Vector3>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var currentRoom = _shipManager.Ship[i, j];
                
                if (currentRoom == null || !currentRoom.TryGetComponent<Room>(out _)) continue;

                var rightRoom = j < cols - 1 ? _shipManager.Ship[i, j + 1] : null;
                var bottomRoom = i < rows - 1 ? _shipManager.Ship[i + 1, j] : null;

                if (j < cols - 1 && rightRoom != null && rightRoom.TryGetComponent<Room>(out var room) &&
                    currentRoom.GetComponent<Room>().lvl == room.lvl &&
                    currentRoom != rightRoom &&
                    currentRoom.GetComponent<Room>().roomResourcesType == room.roomResourcesType)
                {
                    if (currentRoom.TryGetComponent<SmallRoom>(out _)
                        && rightRoom.TryGetComponent<SmallRoom>(out _))
                        result.Add(Utilities.GetInGameCoordinateForPosition(i, j + 0.5, -7f));

                    else if (currentRoom.TryGetComponent<MediumRoom>(out _) &&
                             rightRoom.TryGetComponent<MediumRoom>(out _) &&
                             currentRoom.transform.rotation == rightRoom.transform.rotation &&
                             currentRoom.transform.rotation == Quaternion.Euler(0, 0, 90)
                             && i < rows - 1 && j < cols - 1 &&
                                 _shipManager.Ship[i + 1, j] == currentRoom &&
                                 _shipManager.Ship[i + 1, j + 1] == rightRoom)
                        result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j + 0.5, -7f));
                }

                if (i >= rows - 1 || bottomRoom == null || !bottomRoom.TryGetComponent<Room>(out var room2)
                    || currentRoom.GetComponent<Room>().lvl != room2.lvl ||
                    currentRoom == bottomRoom || 
                    currentRoom.GetComponent<Room>().roomResourcesType != room2.roomResourcesType) 
                    continue;
                
                if (currentRoom.TryGetComponent<SmallRoom>(out _)
                    && bottomRoom.TryGetComponent<SmallRoom>(out _))
                    result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j, -7f));
                
                else if (currentRoom.TryGetComponent<MediumRoom>(out _) &&
                         bottomRoom.TryGetComponent<MediumRoom>(out _) &&
                         currentRoom.transform.rotation == bottomRoom.transform.rotation &&
                         currentRoom.transform.rotation == Quaternion.Euler(0, 0, 0) &&
                         j < cols - 1 && i < rows - 1 &&
                             _shipManager.Ship[i, j + 1] == currentRoom &&
                             _shipManager.Ship[i + 1, j + 1] == bottomRoom)
                        result.Add(Utilities.GetInGameCoordinateForPosition(i + 0.5, j + 0.5, -7f));
            }
        }

        return result;
    }

    public void MergeRoom(Transform mergePoint)
    {
        var (x, y) = Utilities.GetPositionInArrayOfCoordinate(mergePoint.position);
        var (oldRoom1, oldRoom2) = FixCrewsPosition(_shipManager.Ship[(int)x, (int)y],
            _shipManager.Ship[(int)(x + 0.5), (int)(y + 0.5)]); 

        if (Math.Round(x) == x)
        {
            _shipManager.RemoveObjectFrom(((int)x, (int)y), ObjectType.SmallRoom);
            _shipManager.RemoveObjectFrom(((int)(x + 0.5), (int)(y + 0.5)), ObjectType.SmallRoom);
            _shipManager.CreateObject(ObjectType.MediumRoom, Utilities.GetInGameCoordinateForPosition(x, y, 0),
                oldRoom1.roomResourcesType);
        }
        else if (Math.Round(y) == y)
        {
            _shipManager.RemoveObjectFrom(((int)x, (int)y), ObjectType.SmallRoom);
            _shipManager.RemoveObjectFrom(((int)(x + 0.5), (int)(y + 0.5)), ObjectType.SmallRoom);
            _shipManager.CreateObject(ObjectType.RotatedMediumRoom, Utilities.GetInGameCoordinateForPosition(x, y, 0),
                oldRoom1.roomResourcesType);
        }
        else
        {
            _shipManager.RemoveObjectFrom((x - 0.5, y), ObjectType.MediumRoom);
            _shipManager.RemoveObjectFrom((x + 0.5, y), ObjectType.MediumRoom);
            _shipManager.CreateObject(ObjectType.BigRoom, Utilities.GetInGameCoordinateForPosition(x, y, 0),
                oldRoom1.roomResourcesType);
        }
        var newRoom = _shipManager.Ship[(int)x, (int)y].GetComponent<Room>();

        StartCoroutine(MoveCrew(newRoom, oldRoom1, oldRoom2));
        
        newRoom.actualCapacity = oldRoom1.actualCapacity + oldRoom2.actualCapacity;
        newRoom.lvl = oldRoom1.lvl;
    }

    private static IEnumerator MoveCrew(Room newRoom, Room oldRoom1, Room oldRoom2)
    {
        yield return new WaitForSeconds(0.1f);
        newRoom.crews = oldRoom1.crews.Concat(oldRoom2.crews).ToArray();
        newRoom.crews.Where(c => c != null).ToList().ForEach(crew => crew.GetComponent<CrewMovement>().room = newRoom);
    }

    private static (Room, Room) FixCrewsPosition(GameObject room1, GameObject room2)
    {
        var oldRoom1 = room1.GetComponent<Room>();
        var oldRoom2 = room2.GetComponent<Room>();

        if (room1.transform.rotation != Quaternion.Euler(0, 0, 0))
            (oldRoom1.crews[1], oldRoom2.crews[0]) = (oldRoom2.crews[0], oldRoom1.crews[1]);

        return (oldRoom1, oldRoom2);
    }
}
