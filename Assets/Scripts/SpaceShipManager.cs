using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

public class SpaceShipManager : MonoBehaviour
{
    private const int ShipDimension = 50;
    public readonly GameObject[,] Ship = new GameObject[ShipDimension, ShipDimension];
    private ActorManager _actorManager;
    public GameObject lastPointer;
    private readonly Dictionary<GameObject, (IEnumerator, (Vector3 half1, Vector3 half2))> _doorsInAction = new();
    
    private void Start()
    {
        _actorManager = transform.GetComponent<ActorManager>();
        StartCoroutine(Constructor());
    }

    private IEnumerator Constructor()
    {
        CreateObject(ObjectType.BigRoom, new Vector2(-10, 10), Resource.Energy);
        CreateObject(ObjectType.MediumRoom, new Vector2(10, 15), Resource.Oxygen);
        CreateObject(ObjectType.RotatedMediumRoom, new Vector2(15, 0), Resource.Water);
        CreateObject(ObjectType.SmallRoom, new Vector2(5, -5), Resource.Food);

        yield return new WaitForSeconds(0.5f);
        
        CreateObject(ObjectType.Crew, new Vector2(-15, 15), Resource.None);
        CreateObject(ObjectType.Crew, new Vector2(5, -5), Resource.None);
    }
    
    /// <summary>
    /// Creates an object of the specified type at the given position in the spaceship.
    /// </summary>
    /// <param name="type">The type of the object to create.</param>
    /// <param name="position">The position of the room.</param>
    public void CreateObject(ObjectType type, Vector2 position, Resource resourceType)
    {
        var (x, y) = Utilities.GetPositionInArrayOfCoordinate(position);
        GameObject currentObject;

        switch (type)
        {
            case ObjectType.SmallRoom:
                RemoveObjectFrom((x, y), ObjectType.SmallRoom);
                currentObject = _actorManager.CreateObject(position, type);
                currentObject.GetComponent<SmallRoom>().roomResourcesType = resourceType;
                AddToShip(currentObject, (x, y));
                break;

            case ObjectType.MediumRoom:
                RemoveObjectFrom((x, y), ObjectType.MediumRoom);
                currentObject = _actorManager.CreateObject(position, type);
                currentObject.GetComponent<MediumRoom>().roomResourcesType = resourceType;
                AddToShip(currentObject, (x, y - 0.5), (x, y + 0.5));
                break;

            case ObjectType.RotatedMediumRoom:
                RemoveObjectFrom((x, y), ObjectType.RotatedMediumRoom);
                var rotatedMediumRoom = _actorManager.CreateObject(position, type);
                rotatedMediumRoom.transform.GetChild(3).transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                rotatedMediumRoom.transform.GetChild(3).transform.GetChild(0).transform.localScale =
                    new Vector3(0.3f, 0.15f, 1);
                rotatedMediumRoom.GetComponent<MediumRoom>().roomResourcesType = resourceType;
                AddToShip(rotatedMediumRoom, (x - 0.5, y),
                    (x + 0.5, y));
                break;

            case ObjectType.BigRoom:
                RemoveObjectFrom((x, y), ObjectType.BigRoom);
                currentObject = _actorManager.CreateObject(position, type);
                currentObject.GetComponent<BigRoom>().roomResourcesType = resourceType;
                AddToShip(currentObject, (x - 0.5, y - 0.5),
                    (x - 0.5, y + 0.5), (x + 0.5, y - 0.5), (x + 0.5, y + 0.5));
                break;

            case ObjectType.Road:
                RemoveObjectFrom((x, y), ObjectType.Road);
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                break;

            case ObjectType.RoadRotated:
                RemoveObjectFrom((x, y), ObjectType.RoadRotated);
                var rotatedRoad = _actorManager.CreateObject(position, type);
                rotatedRoad.transform.GetChild(3).transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                rotatedRoad.transform.GetChild(3).transform.GetChild(0).transform.localScale =
                    new Vector3(0.15f, 0.5f, 1);
                AddToShip(rotatedRoad, (x, y));
                break;

            case ObjectType.CrossRoad:
                RemoveObjectFrom((x, y), ObjectType.CrossRoad);
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                break;

            case ObjectType.LRoad:
                RemoveObjectFrom((x, y), ObjectType.LRoad);
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                break;

            case ObjectType.LRoadRotated90:
                RemoveObjectFrom((x, y), ObjectType.LRoadRotated90);
                var rotatedLRoad90 = _actorManager.CreateObject(position, type);
                rotatedLRoad90.transform.GetChild(3).transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                AddToShip(rotatedLRoad90, (x, y));
                break;

            case ObjectType.LRoadRotated180:
                RemoveObjectFrom((x, y), ObjectType.LRoadRotated180);
                var rotatedLRoad180 = _actorManager.CreateObject(position, type);
                rotatedLRoad180.transform.GetChild(3).transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                AddToShip(rotatedLRoad180, (x, y));
                break;

            case ObjectType.LRoadRotated270:
                RemoveObjectFrom((x, y), ObjectType.LRoadRotated270);
                var rotatedLRoad270 = _actorManager.CreateObject(position, type);
                rotatedLRoad270.transform.GetChild(3).transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
                AddToShip(rotatedLRoad270, (x, y));
                break;

            case ObjectType.Crew:
                var crew = _actorManager.CreateObject(position, type).GetComponent<Crew>();
                var room = Ship[(int)x, (int)y].GetComponent<Room>();
                crew.room = room;
                room.crews[Array.IndexOf(room.crews, null)] = crew;
                break;

            case ObjectType.Pointer:
                lastPointer = _actorManager.CreateObject(position, type);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "This object cannot be created");
        }
    }

    public void CreateConstructPlacesFor(ObjectSize objectSize, ObjectType objectType, [CanBeNull] GameObject movingObject = null)
    {
        foreach (var (x, y) in GetListOfConstructPlacesPositionsFor(objectSize, objectType, movingObject))
            _actorManager.CreateObject(Utilities.GetInGameCoordinateForPosition(x, y, 0),
                ObjectType.ConstructRotatedPlace);
    }
    
    private List<(double, double)> GetListOfConstructPlacesPositionsFor(ObjectSize objectSize, ObjectType objectType, [CanBeNull] Object movingObject)
    {
        var places = new List<(double, double)>();
        var mappingHelper = new MappingHelper(Ship);
        
        for (var i = 0; i < ShipDimension; i++)
        {
            for (var j = 0; j < ShipDimension; j++)
            {
                if (Ship[i, j] == null) continue;

                foreach (var direction in Utilities.Directions)
                {
                    var newDirectionI = i + direction[0];
                    var newDirectionJ = j + direction[1];

                    if (Utilities.CheckIfValid(newDirectionI, newDirectionJ, ShipDimension, ShipDimension) &&
                        Ship[newDirectionI, newDirectionJ] == null &&
                        (Ship[i, j].TryGetComponent<Room>(out _) ||
                         mappingHelper.IsValidRoad(i, j, direction[0], direction[1])) &&
                        (movingObject == null ||
                         Ship[i, j] != movingObject))
                    {
                        switch (objectSize)
                        {
                            case ObjectSize.Small:
                                if (!places.Contains((newDirectionI, newDirectionJ)))
                                {
                                    switch (objectType)
                                    {
                                        case ObjectType.SmallRoom:
                                        case ObjectType.CrossRoad:
                                            places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.Road:
                                            if (!IsBlocked(Ship[newDirectionI - 1, newDirectionJ], 0f, 180f, 270f) ||
                                                !IsBlocked(Ship[newDirectionI + 1, newDirectionJ], 0f, 90f, 0f)) 
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.RoadRotated:
                                            if (!IsBlocked(Ship[newDirectionI, newDirectionJ - 1], 90f, 0f, 270f) ||
                                                !IsBlocked(Ship[newDirectionI, newDirectionJ + 1], 90f, 90f, 180f)) 
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.LRoad:
                                            if (!IsBlocked(Ship[newDirectionI - 1, newDirectionJ], 0f, 180f, 270f) ||
                                                !IsBlocked(Ship[newDirectionI, newDirectionJ + 1], 90f, 90f, 180f)) 
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.LRoadRotated90:
                                            if (!IsBlocked(Ship[newDirectionI - 1, newDirectionJ], 0f, 180f, 270f) ||
                                                !IsBlocked(Ship[newDirectionI, newDirectionJ - 1], 90f, 0f, 270f)) 
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.LRoadRotated180:
                                            if (!IsBlocked(Ship[newDirectionI + 1, newDirectionJ], 0f, 90f, 0f) ||
                                                !IsBlocked(Ship[newDirectionI, newDirectionJ - 1], 90f, 0f, 270f)) 
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                        
                                        case ObjectType.LRoadRotated270:
                                            if (!IsBlocked(Ship[newDirectionI + 1, newDirectionJ], 0f, 90f, 0f) ||
                                                !IsBlocked(Ship[newDirectionI, newDirectionJ + 1], 90f, 90f, 180f))
                                                places.Add((newDirectionI, newDirectionJ));
                                            break;
                                    }
                                }
                                break;
                        
                            case ObjectSize.Medium:
                                if (Utilities.CheckIfValid(newDirectionI, newDirectionJ + 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ + 1] == null &&
                                    !places.Contains((newDirectionI, newDirectionJ + 0.5)))
                                    places.Add((newDirectionI, newDirectionJ + 0.5));
                                
                                if (Utilities.CheckIfValid(newDirectionI, newDirectionJ - 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ - 1] == null &&
                                    !places.Contains((newDirectionI, newDirectionJ - 0.5)))
                                    places.Add((newDirectionI, newDirectionJ - 0.5));
                                break;
                        
                            case ObjectSize.MediumRotated:
                                if (Utilities.CheckIfValid(newDirectionI + 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI + 1, newDirectionJ] == null &&
                                    !places.Contains((newDirectionI + 0.5, newDirectionJ)))
                                    places.Add((newDirectionI + 0.5, newDirectionJ));
                                
                                if (Utilities.CheckIfValid(newDirectionI - 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI - 1, newDirectionJ] == null &&
                                    !places.Contains((newDirectionI - 0.5, newDirectionJ)))
                                    places.Add((newDirectionI - 0.5, newDirectionJ));
                                break;
                    
                            case ObjectSize.Large:
                                if (Utilities.CheckIfValid(newDirectionI + 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI + 1, newDirectionJ] == null &&
                                    Utilities.CheckIfValid(newDirectionI, newDirectionJ + 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ + 1] == null &&
                                    Utilities.CheckIfValid(newDirectionI + 1, newDirectionJ + 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI + 1, newDirectionJ + 1] == null &&
                                    !places.Contains((newDirectionI + 0.5, newDirectionJ + 0.5)))
                                    places.Add((newDirectionI + 0.5, newDirectionJ + 0.5));
                               
                                if (Utilities.CheckIfValid(newDirectionI - 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI - 1, newDirectionJ] == null &&
                                    Utilities.CheckIfValid(newDirectionI, newDirectionJ - 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ - 1] == null &&
                                    Utilities.CheckIfValid(newDirectionI - 1, newDirectionJ - 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI - 1, newDirectionJ - 1] == null &&
                                    !places.Contains((newDirectionI - 0.5, newDirectionJ - 0.5)))
                                    places.Add((newDirectionI - 0.5, newDirectionJ - 0.5));
                               
                                if (Utilities.CheckIfValid(newDirectionI + 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI + 1, newDirectionJ] == null &&
                                    Utilities.CheckIfValid(newDirectionI, newDirectionJ - 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ - 1] == null &&
                                    Utilities.CheckIfValid(newDirectionI + 1, newDirectionJ - 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI + 1, newDirectionJ - 1] == null &&
                                    !places.Contains((newDirectionI + 0.5, newDirectionJ - 0.5)))
                                    places.Add((newDirectionI + 0.5, newDirectionJ - 0.5));
                               
                                if (Utilities.CheckIfValid(newDirectionI - 1, newDirectionJ, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI - 1, newDirectionJ] == null &&
                                    Utilities.CheckIfValid(newDirectionI, newDirectionJ + 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI, newDirectionJ + 1] == null &&
                                    Utilities.CheckIfValid(newDirectionI - 1, newDirectionJ + 1, ShipDimension, ShipDimension) &&
                                    Ship[newDirectionI - 1, newDirectionJ + 1] == null &&
                                    !places.Contains((newDirectionI - 0.5, newDirectionJ + 0.5)))
                                    places.Add((newDirectionI - 0.5, newDirectionJ + 0.5));
                                break;
                        }
                    }
                }
            }
        }

        return places;
    }
    
    
    private void AddToShip(GameObject construction, params (double x, double y)[] coords) => 
        coords.ToList().ForEach(
            coord => Ship[(int)coord.x, (int)coord.y] = construction);

    private void RemoveRoom(params (int x, int y)[] coords) =>
        coords.ToList().ForEach(coord =>
        {
            Destroy(Ship[coord.x, coord.y]);
            Ship[coord.x, coord.y] = null;
        });

    public void RemoveObjectFrom((double x, double y) position, ObjectType type)
    {
        switch (type)
        {
            case ObjectType.MediumRoom:
                RemoveRoom(((int)position.x, (int)(position.y - 0.5)), ((int)position.x, (int)(position.y + 0.5)));
                break;

            case ObjectType.RotatedMediumRoom:
                RemoveRoom(((int)(position.x - 0.5), (int)position.y), ((int)(position.x + 0.5), (int)position.y));
                break;

            case ObjectType.BigRoom:
                RemoveRoom(((int)(position.x - 0.5), (int)(position.y - 0.5)), ((int)(position.x - 0.5), (int)(position.y + 0.5)),
                    ((int)(position.x + 0.5), (int)(position.y - 0.5)), ((int)(position.x + 0.5), (int)(position.y + 0.5)));
                break;

            default:
                RemoveRoom(((int)position.x, (int)position.y));
                break;
        }
    }

    /// <summary>
    /// Finds the position of a room in the spaceship grid based on its coordinates.
    /// </summary>
    /// <param name="room">The Room object to find the position for.</param>
    /// <param name="isStartingRoom">If true, returns the crew position. Otherwise, returns the first empty position.</param>
    /// <param name="crew">Crew that leave the room.</param>
    /// <returns>The position (row, column) of the room in the spaceship grid. Returns (-1, -1) if the room is not found.</returns>
    public (int, int) FindRoomPosition(Room room, bool isStartingRoom, Crew crew)
    {
        var counter = Array.IndexOf(room.crews, (isStartingRoom ? crew : null));
        for (var i = 0; i < ShipDimension; i++)
        for (var j = 0; j < ShipDimension; j++)
            if (Ship[i, j] != null && Ship[i, j].TryGetComponent<Room>(out var foundRoom) && foundRoom == room)
            {
                if (counter == 0)
                    return (i, j);
                counter--;
            }

        return (-1, -1);
    }
    
    private static bool IsBlocked(GameObject gameObject, float linearRoadRotationDeg, float lRoadRotation1Deg, float lRoadRotation2Deg) => 
        gameObject == null ||
        (gameObject.TryGetComponent<LinearRoad>(out var linearRoad) &&
         linearRoad.transform.rotation != Quaternion.Euler(0f, 0f, linearRoadRotationDeg)) ||
        (gameObject.TryGetComponent<LRoad>(out var lRoad) &&
         lRoad.transform.rotation != Quaternion.Euler(0f, 0f, lRoadRotation1Deg) &&
         lRoad.transform.rotation != Quaternion.Euler(0f, 0f, lRoadRotation2Deg));

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private void DoorAnimation(GameObject door, Direction direction)
    {
        var half1 = door.transform.GetChild(0).gameObject.transform;
        var half2 = door.transform.GetChild(1).gameObject.transform;
        
        var half1Position = _doorsInAction.ContainsKey(door)? _doorsInAction[door].Item2.half1 :half1.position;
        var half2Position = _doorsInAction.ContainsKey(door)? _doorsInAction[door].Item2.half2 :half2.position;
        
        var isRotated = door.transform.parent.parent.parent.rotation == Quaternion.Euler(0, 0, 90);
 
        (float x, float y) movement = direction switch
        {
            Direction.Up => (1f, 0),
            Direction.Down => (1f, 0),
            Direction.Left => (0, isRotated? 1f : -1f),
            Direction.Right => (0, isRotated? 1f : -1f),
            _ => (0, 0)
        };
        
        var newHalf1Position =
            new Vector3(half1Position.x - movement.x, half1Position.y - movement.y, half1Position.z);
        
        var newHalf2Position =
            new Vector3(half2Position.x + movement.x, half2Position.y + movement.y, half2Position.z);
        
        if (_doorsInAction.ContainsKey(door))
        {
            StopCoroutine(_doorsInAction[door].Item1);
            _doorsInAction.Remove(door);
        }
        
        _doorsInAction.Add(door,
            (OpenDoorAnimation(half1, half2, half1Position, half2Position, newHalf1Position, newHalf2Position, door), (half1Position, half2Position)));

        StartCoroutine(_doorsInAction[door].Item1);
    }


    private IEnumerator OpenDoorAnimation(Transform half1, Transform half2, Vector3 half1Position, Vector3 half2Position, Vector3 newHalf1Position, Vector3 newHalf2Position, GameObject parent)
    {
        var elapsedTime = 0f;
        const float movementDuration = 0.5f;
        var actualHalf1Position = half1.position;
        var actualHalf2Position = half2.position;

        while (elapsedTime < movementDuration)
        {
            if (half1 == null)
            {
                _doorsInAction.Remove(parent);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            half1.position = Vector3.Lerp(actualHalf1Position, newHalf1Position, Mathf.Clamp01(elapsedTime / movementDuration));
            half2.position = Vector3.Lerp(actualHalf2Position, newHalf2Position, Mathf.Clamp01(elapsedTime / movementDuration));

            yield return null;
        }

        _doorsInAction[parent] = (CloseDoorAnimation(half1, half2, half1Position, half2Position, parent),(half1Position ,half2Position));
        StartCoroutine(_doorsInAction[parent].Item1);
    }
    
    private IEnumerator CloseDoorAnimation(Transform wall1, Transform wall2, Vector3 wall1InitialPosition, Vector2 wall2InitialPosition, GameObject parent)
    {
        var elapsedTime = 0f;
        const float movementDuration = 0.5f;
        
        while (elapsedTime < movementDuration)
        {
            if (wall1 == null)
            {
                _doorsInAction.Remove(parent);
                yield break;
            }
            
            elapsedTime += Time.deltaTime;
            wall1.position = Vector3.Lerp(wall1.position, wall1InitialPosition, Mathf.Clamp01(elapsedTime / movementDuration));
            wall2.position = Vector3.Lerp(wall2.position, wall2InitialPosition, Mathf.Clamp01(elapsedTime / movementDuration));

            yield return null;
        }
        
        _doorsInAction.Remove(parent);
    }
    
    public void OpenDor((int x, int y)firstRoom, (int x, int y)secondRoom)
    {
        if (Ship[firstRoom.x, firstRoom.y] == Ship[secondRoom.x, secondRoom.y])
            return;
        
        var startingRoom = Ship[firstRoom.x, firstRoom.y];
        var endRoom = Ship[secondRoom.x, secondRoom.y];
        var startRoomRotation = startingRoom.transform.rotation;
        var endRoomRotation = endRoom.transform.rotation;
        
        Room room;
        GameObject walls;
        GameObject wall;
        GameObject door;
        
        if (firstRoom.x < secondRoom.x)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(1).gameObject;
                door = wall.transform.GetChild(1).gameObject;
                
                switch (room) 
                { 
                    case SmallRoom:
                        DoorAnimation(door, Direction.Up);
                        break;
                        
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 0):
                        DoorAnimation(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? door
                            : wall.transform.GetChild(3).gameObject,
                            Direction.Up);
                        break;
                        
                    case MediumRoom:
                        DoorAnimation(walls.transform.GetChild(2).transform.GetChild(1).gameObject, Direction.Up);
                        break;
                        
                    case BigRoom:
                        DoorAnimation(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? door
                            : wall.transform.GetChild(3).gameObject,
                            Direction.Up);
                        break;
                }
            }

            if (!endRoom.TryGetComponent<Room>(out room)) return;
            walls = room.transform.GetChild(1).gameObject; 
            wall = walls.gameObject.transform.GetChild(0).gameObject;
            door = wall.transform.GetChild(1).gameObject;

            switch (room) 
            {
                case SmallRoom:
                    DoorAnimation(door.gameObject, Direction.Up);
                    break;
                    
                case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 0):
                    DoorAnimation(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                        ? door
                        : wall.transform.GetChild(3).gameObject, Direction.Up);
                    break;
                 
                case MediumRoom:
                    DoorAnimation(walls.transform.GetChild(3).transform.GetChild(1).gameObject, Direction.Up);
                    break;
                    
                case BigRoom:
                    DoorAnimation(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                        ? door
                        : wall.transform.GetChild(3).gameObject, Direction.Up);
                    break;
            }
        }
        
        else if (firstRoom.x > secondRoom.x)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(0).gameObject;
                door = wall.transform.GetChild(1).gameObject;

                switch (room) 
                {
                    case SmallRoom:
                        DoorAnimation(door, Direction.Down);
                        break;
                    
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 0):
                        DoorAnimation(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? door
                            : wall.transform.GetChild(3).gameObject, Direction.Down);
                        break;
                    
                    case MediumRoom:
                        DoorAnimation(walls.transform.GetChild(3).transform.GetChild(1).gameObject, Direction.Up);
                        break;
                   
                    case BigRoom:
                        DoorAnimation(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? door
                            : wall.transform.GetChild(3).gameObject, Direction.Down);
                        break;
                }
            }

            if (!endRoom.TryGetComponent<Room>(out room)) return;
            walls = room.transform.GetChild(1).gameObject; 
            wall = walls.gameObject.transform.GetChild(1).gameObject;
            door = wall.transform.GetChild(1).gameObject;

            switch (room) 
            {
                case SmallRoom:
                    DoorAnimation(door, Direction.Down);
                    break;
                    
                case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 0):
                    DoorAnimation(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                        ? door
                        : wall.transform.GetChild(3).gameObject, Direction.Down);
                    break;
                    
                case MediumRoom:
                    DoorAnimation(walls.transform.GetChild(2).transform.GetChild(1).gameObject, Direction.Up);
                    break;
                   
                case BigRoom:
                    DoorAnimation(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                        ? door
                        : wall.transform.GetChild(3).gameObject, Direction.Down);
                    break;
                    
            }

        }
        
        else if (firstRoom.y > secondRoom.y)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(2).gameObject;
                door = wall.transform.GetChild(1).gameObject;

                switch (room) 
                {
                    case SmallRoom:
                        DoorAnimation(door, Direction.Right);
                        break;
                    
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 90):
                        DoorAnimation(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? walls.transform.GetChild(0).transform.GetChild(1).gameObject
                            : walls.transform.GetChild(0).transform.GetChild(3).gameObject,
                                Direction.Right);
                        break;
                    
                    case MediumRoom:
                        DoorAnimation(door, Direction.Right);
                        break;
                    
                    case BigRoom:
                        DoorAnimation(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? wall.transform.GetChild(3).gameObject
                            : door, Direction.Right);
                        break;
                        
                }
            }

            if (!endRoom.TryGetComponent<Room>(out room)) return;
            walls = room.transform.GetChild(1).gameObject; 
            wall = walls.gameObject.transform.GetChild(3).gameObject;
            door = wall.transform.GetChild(1).gameObject;

            switch (room) 
            {
                case SmallRoom:
                    DoorAnimation(door, Direction.Right);
                    break;
                   
                case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 90):
                    DoorAnimation(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                            ? walls.transform.GetChild(1).transform.GetChild(1).gameObject
                            : walls.transform.GetChild(1).transform.GetChild(3).gameObject,
                        Direction.Right);
                    break;
                    
                case MediumRoom:
                    DoorAnimation(door, Direction.Right);
                    break;
                   
                case BigRoom:
                    DoorAnimation(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                        ? wall.transform.GetChild(3).gameObject
                        : door, Direction.Right);
                    break;
            }
        }
        
        else if (firstRoom.y < secondRoom.y)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(3).gameObject;
                door = wall.transform.GetChild(1).gameObject;

                switch (room) 
                {
                    case SmallRoom:
                        DoorAnimation(door, Direction.Left);
                        break;
                    
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 90):
                        DoorAnimation(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? walls.transform.GetChild(1).transform.GetChild(1).gameObject
                            : walls.transform.GetChild(1).transform.GetChild(3).gameObject,
                                Direction.Left);
                        break;
                    
                    case MediumRoom:
                        DoorAnimation(door, Direction.Left);
                        break;
                    
                    case BigRoom:
                        DoorAnimation(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? wall.transform.GetChild(3).gameObject
                            : door, Direction.Left);
                        break;
                }
            }

            if (!endRoom.TryGetComponent<Room>(out room)) return;
            walls = room.transform.GetChild(1).gameObject; 
            wall = walls.gameObject.transform.GetChild(2).gameObject;
            door = wall.transform.GetChild(1).gameObject;
 
            switch (room) 
            {
                case SmallRoom:
                    DoorAnimation(door, Direction.Left);
                    break;
                    
                case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 90):
                    DoorAnimation(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                            ? walls.transform.GetChild(0).transform.GetChild(1).gameObject
                            : walls.transform.GetChild(0).transform.GetChild(3).gameObject,
                        Direction.Right);
                    break;
                  
                case MediumRoom:
                    DoorAnimation(door, Direction.Right);
                    break;
                    
                case BigRoom:
                    DoorAnimation(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                        ? wall.transform.GetChild(3).gameObject
                        : door, Direction.Right);
                    break;
            }
        }
    }
}