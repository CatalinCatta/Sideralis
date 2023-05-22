using System.Linq;
using UnityEngine;

public class SpaceShipManager : MonoBehaviour
{
    public readonly GameObject[,] ship = new GameObject[shipDimension, shipDimension];
    private static int shipDimension = 50;
    private ActorManager _actorManager;

    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        CreateRoom(ObjectType.BigRoom, new Vector2(-10, 10));
        CreateRoom(ObjectType.MediumRoom, new Vector2(10, 15));
        CreateRoom(ObjectType.RotatedMediumRoom, new Vector2(15, 0));
        CreateRoom(ObjectType.SmallRoom, new Vector2(5, -5));
        CreateRoom(ObjectType.Crew, new Vector2(-15, 15));
    }

    public void CreateRoom(ObjectType objectType, Vector2 position)
    {
        var x = GetXCoordinate(position);
        var y = GetYCoordinate(position);

        switch (objectType)
        {
            case ObjectType.ConstructPlace:
                AddToShip(_actorManager.CreateObject(position, ObjectType.ConstructPlace), (x, y));
                break;
       
            case ObjectType.ConstructRotatedPlace:
                AddToShip(_actorManager.CreateObject(position, ObjectType.ConstructRotatedPlace), (x, y));
                break;
         
            case ObjectType.SmallRoom:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.SmallRoom), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;
         
            case ObjectType.MediumRoom:
                RemoveRoom(((int)x, (int)(y - 0.5)), ((int)x, (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, ObjectType.MediumRoom), (x, y - 0.5), (x, y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y), ((int)x, (int)(y - 0.5))), (new Vector2(position.x + 5, position.y), ((int)x, (int)(y + 0.5))));
                break;
         
            case ObjectType.RotatedMediumRoom:
                RemoveRoom(((int)(x - 0.5), (int)y), ((int)(x + 0.5), (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.RotatedMediumRoom), (x - 0.5, y), (x + 0.5, y));
                AddConstructPlace((new Vector2(position.x, position.y + 5), ((int)(x - 0.5), (int)y)), (new Vector2(position.x, position.y - 5), ((int)(x + 0.5), (int)y)));
                break;
          
            case ObjectType.BigRoom:
                RemoveRoom(((int)(x - 0.5), (int)(y - 0.5)), ((int)(x - 0.5), (int)(y + 0.5)),
                    ((int)(x + 0.5), (int)(y - 0.5)), ((int)(x + 0.5), (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, ObjectType.BigRoom), (x - 0.5, y - 0.5), (x - 0.5 ,y + 0.5), (x + 0.5, y - 0.5), (x + 0.5 ,y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y + 5), ((int)(x - 0.5), (int)(y - 0.5))),(new Vector2(position.x + 5, position.y + 5), ((int)(x - 0.5), (int)(y + 0.5))),(new Vector2(position.x - 5, position.y - 5), ((int)(x + 0.5), (int)(y - 0.5))), (new Vector2(position.x + 5, position.y - 5), ((int)(x + 0.5), (int)(y + 0.5))));
                break;
          
            case ObjectType.Road:
                if (IsBlocked(ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(ship[(int)x + 1, (int)y], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.Road), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Down);
                break;
         
            case ObjectType.RoadRotated:
                if (IsBlocked(ship[(int)x, (int)y - 1], 90f, 0f, 270f) &&
                    IsBlocked(ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.RoadRotated), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Right);
                break;
          
            case ObjectType.CrossRoad:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.CrossRoad), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;
         
            case ObjectType.LRoad:
                if (IsBlocked(ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.LRoad), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Right);
                break;
       
            case ObjectType.LRoadRotated90:
                if (IsBlocked(ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(ship[(int)x, (int)y - 1], 90f, 0f, 270f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.LRoadRotated90), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Left);
                break;
           
            case ObjectType.LRoadRotated180:
                if (IsBlocked(ship[(int)x + 1, (int)y], 0f, 90f, 0f) &&
                    IsBlocked(ship[(int)x, (int)y - 1], 90f, 0f, 270f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.LRoadRotated180), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Down);
                break;
          
            case ObjectType.LRoadRotated270:
                if (IsBlocked(ship[(int)x + 1, (int)y], 0f, 90f, 0f) &&
                    IsBlocked(ship[(int)x, (int)y + 1], 90f, 90f, 180f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, ObjectType.LRoadRotated270), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Down, Direction.Right);
                break;
            
            case ObjectType.Crew:
                var crew = _actorManager.CreateObject(position, ObjectType.Crew).GetComponent<Crew>();
                var room = ship[(int)x, (int)y].GetComponent<Room>();
                crew.room = room;
                room.crews.Add(crew);
                break;
        }
    }

    private void AddToShip(GameObject construction, params (double x, double y)[] coords) => coords.ToList().ForEach(
        coord => ship[(int)coord.x, (int)coord.y] = construction);
    
    private static double GetXCoordinate(Vector2 position) => (double)-Mathf.RoundToInt(position.y) / 10 + 24.5;

    private static double GetYCoordinate(Vector2 position) => (double)Mathf.RoundToInt(position.x) / 10 + 24.5;

    private void RemoveRoom(params (int x, int y)[] coords) => coords.ToList().ForEach(coord =>
    {
        Destroy(ship[coord.x, coord.y]);
        ship[coord.x, coord.y] = null;
    });

    private void AddConstructPlace(params (Vector2 objectPosition, (int x, int y) positionInArray)[] positions) =>
        positions.ToList().ForEach(position =>
            AddIndividualConstructPlace(position.positionInArray, position.objectPosition, Direction.Up, Direction.Left,
                Direction.Right, Direction.Down));

    private void AddIndividualConstructPlace((int x, int y) arrayPosition, Vector2 objectPosition,
        params Direction[] directions) => directions.ToList().ForEach(direction =>
        {
            switch (direction)
            {
                case Direction.Up:
                    if (ship[arrayPosition.x - 1, arrayPosition.y] == null)
                        CreateRoom(ObjectType.ConstructRotatedPlace,
                            new Vector2(objectPosition.x, objectPosition.y + 10));
                    break;
                
                case Direction.Left:
                    if (ship[arrayPosition.x, arrayPosition.y - 1] == null)
                        CreateRoom(ObjectType.ConstructPlace, new Vector2(objectPosition.x - 10, objectPosition.y));
                    break;
                
                case Direction.Right:
                    if (ship[arrayPosition.x, arrayPosition.y + 1] == null)
                        CreateRoom(ObjectType.ConstructPlace, new Vector2(objectPosition.x + 10, objectPosition.y));
                    break;
                
                case Direction.Down:
                    if (ship[arrayPosition.x + 1, arrayPosition.y] == null)
                        CreateRoom(ObjectType.ConstructRotatedPlace,
                            new Vector2(objectPosition.x, objectPosition.y - 10));
                    break;
            }
        }
    );

    private void PrintGameObjectList()
    {
        var output = "";

        for (var y = 20; y < 30; y++)
        {
            for (var x = 20; x < 30; x++)
            {
                var element = ship[y, x];
                var elementString = element != null ? element.name : "  null  ";
                elementString = elementString.PadRight(20, ' ');
                output += "[" + y.ToString("D2") + " / " + x.ToString("D2") + ": " + elementString + "] ";
            }

            output += "\n";
        }

        Debug.Log(output);
    }

    public (int, int) FindRoomPosition(Room room)
    {
        for (var i = 0; i < shipDimension; i++)
        {
            for (var j = 0; j < shipDimension; j++)
            {
                if (ship[i, j] != null && ship[i,j].TryGetComponent<Room>(out var foundedRoom) && foundedRoom == room)
                {
                    return (i, j);
                }
            }
        }

        // Element not found
        return (-1, -1);
    }
    
    private static bool IsBlocked(GameObject structure, float linearRoadRotation, float lRoadRotation1,
        float lRoadRotation2) => structure == null || structure.TryGetComponent<ConstructPlace>(out _) ||
               (structure.TryGetComponent<LinearRoad>(out var linearRoad) &&
                linearRoad.transform.rotation != Quaternion.Euler(0f, 0f, linearRoadRotation)) ||
               (structure.TryGetComponent<LRoad>(out var lRoad) &&
                lRoad.transform.rotation != Quaternion.Euler(0f, 0f, lRoadRotation1) &&
                lRoad.transform.rotation != Quaternion.Euler(0f, 0f, lRoadRotation2));
    
    private enum Direction
    {   
        Up,
        Left,
        Right,
        Down
    }
}