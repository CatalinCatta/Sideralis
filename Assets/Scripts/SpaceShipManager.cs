using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpaceShipManager : MonoBehaviour
{
    private const int ShipDimension = 50;
    public readonly GameObject[,] Ship = new GameObject[ShipDimension, ShipDimension];
    private ActorManager _actorManager;
    [FormerlySerializedAs("LastPointer")] public GameObject lastPointer;
    
    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        CreateRoom(ObjectType.BigRoom, new Vector2(-10, 10));
        CreateRoom(ObjectType.MediumRoom, new Vector2(10, 15));
        CreateRoom(ObjectType.RotatedMediumRoom, new Vector2(15, 0));
        CreateRoom(ObjectType.SmallRoom, new Vector2(5, -5));
        CreateRoom(ObjectType.Crew, new Vector2(-15, 15));
    }

    public void CreateRoom(ObjectType type, Vector2 position)
    {
        var x = GetXCoordinate(position);
        var y = GetYCoordinate(position);

        switch (type)
        {
            case ObjectType.ConstructPlace:
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                break;

            case ObjectType.ConstructRotatedPlace:
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                break;

            case ObjectType.SmallRoom:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;

            case ObjectType.MediumRoom:
                RemoveRoom(((int)x, (int)(y - 0.5)), ((int)x, (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, type), (x, y - 0.5), (x, y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y), ((int)x, (int)(y - 0.5))),
                    (new Vector2(position.x + 5, position.y), ((int)x, (int)(y + 0.5))));
                break;

            case ObjectType.RotatedMediumRoom:
                RemoveRoom(((int)(x - 0.5), (int)y), ((int)(x + 0.5), (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x - 0.5, y),
                    (x + 0.5, y));
                AddConstructPlace((new Vector2(position.x, position.y + 5), ((int)(x - 0.5), (int)y)),
                    (new Vector2(position.x, position.y - 5), ((int)(x + 0.5), (int)y)));
                break;

            case ObjectType.BigRoom:
                RemoveRoom(((int)(x - 0.5), (int)(y - 0.5)), ((int)(x - 0.5), (int)(y + 0.5)),
                    ((int)(x + 0.5), (int)(y - 0.5)), ((int)(x + 0.5), (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, type), (x - 0.5, y - 0.5),
                    (x - 0.5, y + 0.5), (x + 0.5, y - 0.5), (x + 0.5, y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y + 5), ((int)(x - 0.5), (int)(y - 0.5))),
                    (new Vector2(position.x + 5, position.y + 5), ((int)(x - 0.5), (int)(y + 0.5))),
                    (new Vector2(position.x - 5, position.y - 5), ((int)(x + 0.5), (int)(y - 0.5))),
                    (new Vector2(position.x + 5, position.y - 5), ((int)(x + 0.5), (int)(y + 0.5))));
                break;

            case ObjectType.Road:
                if (IsBlocked(Ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(Ship[(int)x + 1, (int)y], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Down);
                break;

            case ObjectType.RoadRotated:
                if (IsBlocked(Ship[(int)x, (int)y - 1], 90f, 0f, 270f) &&
                    IsBlocked(Ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Right);
                break;

            case ObjectType.CrossRoad:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;

            case ObjectType.LRoad:
                if (IsBlocked(Ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(Ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Right);
                break;

            case ObjectType.LRoadRotated90:
                if (IsBlocked(Ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(Ship[(int)x, (int)y - 1], 90f, 0f, 270f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Left);
                break;

            case ObjectType.LRoadRotated180:
                if (IsBlocked(Ship[(int)x + 1, (int)y], 0f, 90f, 0f) &&
                    IsBlocked(Ship[(int)x, (int)y - 1], 90f, 0f, 270f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Down);
                break;

            case ObjectType.LRoadRotated270:
                if (IsBlocked(Ship[(int)x + 1, (int)y], 0f, 90f, 0f) &&
                    IsBlocked(Ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, type), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Down, Direction.Right);
                break;

            case ObjectType.Crew:
                var crew = _actorManager.CreateObject(position, type).GetComponent<Crew>();
                var room = Ship[(int)x, (int)y].GetComponent<Room>();
                crew.room = room;
                room.crews.Add(crew);
                break;

            case ObjectType.Pointer:
                lastPointer = _actorManager.CreateObject(position, type);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "This object cannot be created");
        }
    }

    private void AddToShip(GameObject construction, params (double x, double y)[] coords) => 
        coords.ToList().ForEach(
            coord => Ship[(int)coord.x, (int)coord.y] = construction);

    private static double GetXCoordinate(Vector2 position) => 
        (double)-Mathf.RoundToInt(position.y) / 10 + 24.5;

    private static double GetYCoordinate(Vector2 position) => 
        (double)Mathf.RoundToInt(position.x) / 10 + 24.5;

    private void RemoveRoom(params (int x, int y)[] coords) =>
        coords.ToList().ForEach(coord =>
        {
            Destroy(Ship[coord.x, coord.y]);
            Ship[coord.x, coord.y] = null;
        });

    private void AddConstructPlace(params (Vector2 inGamePosition, (int x, int y) arrayPositions)[] positions) => positions.ToList().ForEach(arrayPosition =>
            AddIndividualConstructPlace(arrayPosition.arrayPositions, arrayPosition.inGamePosition, Direction.Up, Direction.Left,
                Direction.Right, Direction.Down));

    private void AddIndividualConstructPlace((int x, int y) arrayPosition, Vector2 objectPosition,
        params Direction[] directions) => directions.ToList().ForEach(direction =>
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (Ship[arrayPosition.x - 1, arrayPosition.y] == null)
                            CreateRoom(ObjectType.ConstructRotatedPlace,
                                new Vector2(objectPosition.x, objectPosition.y + 10));
                        break;

                    case Direction.Left:
                        if (Ship[arrayPosition.x, arrayPosition.y - 1] == null)
                            CreateRoom(ObjectType.ConstructPlace, new Vector2(objectPosition.x - 10, objectPosition.y));
                        break;

                    case Direction.Right:
                        if (Ship[arrayPosition.x, arrayPosition.y + 1] == null)
                            CreateRoom(ObjectType.ConstructPlace, new Vector2(objectPosition.x + 10, objectPosition.y));
                        break;

                    case Direction.Down:
                        if (Ship[arrayPosition.x + 1, arrayPosition.y] == null)
                            CreateRoom(ObjectType.ConstructRotatedPlace,
                                new Vector2(objectPosition.x, objectPosition.y - 10));
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction,
                            "This is not a valid direction");
                }
            }
        );
    
    /// <summary>
    /// Only in emergency situations.
    /// </summary>
    public void PrintGameObjectGrid()
    {
        var output = "";

        for (var y = 20; y < 30; y++)
        {
            for (var x = 20; x < 30; x++)
            {
                var element = Ship[y, x];
                var elementString = element != null ? element.name : "  null  ";
                elementString = elementString.PadRight(20, ' ');
                output += "[" + y.ToString("D2") + " / " + x.ToString("D2") + ": " + elementString + "] ";
            }

            output += "\n";
        }

        Debug.Log(output);
    }

    /// <summary>
    /// Finds the position of a room in the spaceship grid based on its coordinates.
    /// </summary>
    /// <param name="room">The Room object to find the position for.</param>
    /// <returns>The position (row, column) of the room in the spaceship grid. Returns (-1, -1) if the room is not found.</returns>
    public (int, int) FindRoomPosition(Room room)
    {
        for (var i = 0; i < ShipDimension; i++)
        for (var j = 0; j < ShipDimension; j++)
            if (Ship[i, j] != null && Ship[i, j].TryGetComponent<Room>(out var foundRoom) && foundRoom == room)
                return (i, j);

        return (-1, -1);
    }
    
    private static bool IsBlocked(GameObject gameObject, float linearRoadRotationDeg, float lRoadRotation1Deg,
        float lRoadRotation2Deg) => 
        gameObject == null || gameObject.TryGetComponent<ConstructPlace>(out _) ||
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

    private IEnumerator DoorAnimation(GameObject door, Direction direction)
    {
        var isRotated = door.transform.parent.parent.parent.rotation == Quaternion.Euler(0, 0, 90);

        (float x, float y) movement = direction switch
        {
            Direction.Up => (1f, 0),
            Direction.Down => (1f, 0),
            Direction.Left => (0, isRotated? 1f : -1f),
            Direction.Right => (0, isRotated? 1f : -1f),
            _ => (0, 0)
        };

        var wall1 = door.transform.GetChild(0).gameObject.transform;
        var wall2 = door.transform.GetChild(1).gameObject.transform;

        var wall1Position = wall1.position;

        var elapsedTime = 0f;
        const float movementDuration = 0.5f;
        var wall2Position = wall2.position;
        
        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            wall1.position = Vector3.Lerp(wall1Position, new Vector3(wall1Position.x - movement.x, wall1Position.y - movement.y, wall1Position.z), Mathf.Clamp01(elapsedTime / movementDuration));
            wall2.position = Vector3.Lerp(wall2Position, new Vector3(wall2Position.x + movement.x, wall2Position.y + movement.y, wall2Position.z), Mathf.Clamp01(elapsedTime / movementDuration));

            yield return null;
        }

        if (elapsedTime >= movementDuration)
            StartCoroutine(CloseDoorAnimation(wall1, wall2, wall1Position, wall2Position));
    }

    private static IEnumerator CloseDoorAnimation(Transform wall1, Transform wall2, Vector3 wall1InitialPosition, Vector2 wall2InitialPosition)
    {
        var elapsedTime = 0f;
        const float movementDuration = 0.5f;
        
        while (elapsedTime < movementDuration)
        {
            elapsedTime += Time.deltaTime;
            wall1.position = Vector3.Lerp(wall1.position, wall1InitialPosition, Mathf.Clamp01(elapsedTime / movementDuration));
            wall2.position = Vector3.Lerp(wall2.position, wall2InitialPosition, Mathf.Clamp01(elapsedTime / movementDuration));

            yield return null;
        }
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
        
        if (firstRoom.x < secondRoom.x)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(1).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 0):
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Up));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(walls.transform.GetChild(2).transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case BigRoom:
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Up));
                        break;
                }
            }
            if (endRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(0).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 0):
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Up));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(walls.transform.GetChild(3).transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case BigRoom:
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Up)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Up));
                        break;
                }
            }
        }
        
        if (firstRoom.x > secondRoom.x)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(0).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down));
                        break;
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 0):
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Down));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(walls.transform.GetChild(3).transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case BigRoom:
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Down));
                        break;
                }
            }
            if (endRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(1).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down));
                        break;
                    case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 0):
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Down));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(walls.transform.GetChild(2).transform.GetChild(1).gameObject, Direction.Up));
                        break;
                    case BigRoom:
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y + 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Down)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Down));
                        break;
                }
            }
        }
        
        if (firstRoom.y > secondRoom.y)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(2).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 90):
                        StartCoroutine(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? DoorAnimation(walls.transform.GetChild(0).transform.GetChild(1).gameObject, Direction.Right)
                            : DoorAnimation(walls.transform.GetChild(0).transform.GetChild(3).gameObject, Direction.Right));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                    case BigRoom:
                        StartCoroutine(startingRoom == Ship[firstRoom.x - 1, firstRoom.y]
                            ? DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Right)
                            : DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                }
            }
            if (endRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(3).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                    case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 90):
                        StartCoroutine(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                            ? DoorAnimation(walls.transform.GetChild(1).transform.GetChild(1).gameObject, Direction.Right)
                            : DoorAnimation(walls.transform.GetChild(1).transform.GetChild(3).gameObject, Direction.Right));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                    case BigRoom:
                        StartCoroutine(endRoom == Ship[secondRoom.x - 1, secondRoom.y]
                            ? DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Right)
                            : DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                }
            }
        }
        
        if (firstRoom.y < secondRoom.y)
        {
            if (startingRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(3).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Left));
                        break;
                    case MediumRoom when startRoomRotation == Quaternion.Euler(0, 0, 90):
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y - 1]
                            ? DoorAnimation(walls.transform.GetChild(1).transform.GetChild(1).gameObject, Direction.Left)
                            : DoorAnimation(walls.transform.GetChild(1).transform.GetChild(3).gameObject, Direction.Left));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Left));
                        break;
                    case BigRoom:
                        StartCoroutine(startingRoom == Ship[firstRoom.x, firstRoom.y - 1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Left)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Left));
                        break;
                }
                
            }
            if (endRoom.TryGetComponent<Room>(out room))
            {
                walls = room.transform.GetChild(1).gameObject; 
                wall = walls.gameObject.transform.GetChild(2).gameObject;
                
                switch (room)
                {
                    case SmallRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Left));
                        break;
                    case MediumRoom when endRoomRotation == Quaternion.Euler(0, 0, 90):
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y - 1]
                            ? DoorAnimation(walls.transform.GetChild(0).transform.GetChild(1).gameObject, Direction.Right)
                            : DoorAnimation(walls.transform.GetChild(0).transform.GetChild(3).gameObject, Direction.Right));
                        break;
                    case MediumRoom:
                        StartCoroutine(DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right));
                        break;
                    case BigRoom:
                        StartCoroutine(endRoom == Ship[secondRoom.x, secondRoom.y-1]
                            ? DoorAnimation(wall.transform.GetChild(1).gameObject, Direction.Right)
                            : DoorAnimation(wall.transform.GetChild(3).gameObject, Direction.Right));
                        break;
                }
            }
        }
    }
}