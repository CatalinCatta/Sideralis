using System.Linq;
using UnityEngine;

public class SpaceShipManager : MonoBehaviour
{
    private readonly GameObject[,] _ship = new GameObject[50, 50];
    private ActorManager _actorManager;

    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        CreateRoom(RoomType.BigRoom, new Vector2(-10, 10));
        CreateRoom(RoomType.MediumRoom, new Vector2(10, 15));
        CreateRoom(RoomType.RotatedMediumRoom, new Vector2(15, 0));
        CreateRoom(RoomType.SmallRoom, new Vector2(5, -5));
    }

    public void CreateRoom(RoomType roomType, Vector2 position)
    {
        var x = GetXCoordinate(position);
        var y = GetYCoordinate(position);

        switch (roomType)
        {
            case RoomType.ConstructPlace:
                AddToShip(_actorManager.CreateObject(position, RoomType.ConstructPlace), (x, y));
                break;
       
            case RoomType.ConstructRotatedPlace:
                AddToShip(_actorManager.CreateObject(position, RoomType.ConstructRotatedPlace), (x, y));
                break;
         
            case RoomType.SmallRoom:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.SmallRoom), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;
         
            case RoomType.MediumRoom:
                RemoveRoom(((int)x, (int)(y - 0.5)), ((int)x, (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, RoomType.MediumRoom), (x, y - 0.5), (x, y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y), ((int)x, (int)(y - 0.5))), (new Vector2(position.x + 5, position.y), ((int)x, (int)(y + 0.5))));
                break;
         
            case RoomType.RotatedMediumRoom:
                RemoveRoom(((int)(x - 0.5), (int)y), ((int)(x + 0.5), (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.RotatedMediumRoom), (x - 0.5, y), (x + 0.5, y));
                AddConstructPlace((new Vector2(position.x, position.y + 5), ((int)(x - 0.5), (int)y)), (new Vector2(position.x, position.y - 5), ((int)(x + 0.5), (int)y)));
                break;
          
            case RoomType.BigRoom:
                RemoveRoom(((int)(x - 0.5), (int)(y - 0.5)), ((int)(x - 0.5), (int)(y + 0.5)),
                    ((int)(x + 0.5), (int)(y - 0.5)), ((int)(x + 0.5), (int)(y + 0.5)));
                AddToShip(_actorManager.CreateObject(position, RoomType.BigRoom), (x - 0.5, y - 0.5), (x - 0.5 ,y + 0.5), (x + 0.5, y - 0.5), (x + 0.5 ,y + 0.5));
                AddConstructPlace((new Vector2(position.x - 5, position.y + 5), ((int)(x - 0.5), (int)(y - 0.5))),(new Vector2(position.x + 5, position.y + 5), ((int)(x - 0.5), (int)(y + 0.5))),(new Vector2(position.x - 5, position.y - 5), ((int)(x + 0.5), (int)(y - 0.5))), (new Vector2(position.x + 5, position.y - 5), ((int)(x + 0.5), (int)(y + 0.5))));
                break;
          
            case RoomType.Road:
                if (IsBlocked(_ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x + 1, (int)y], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.Road), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Down);
                break;
         
            case RoomType.RoadRotated:
                if (IsBlocked(_ship[(int)x, (int)y - 1], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x, (int)y + 1], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.RoadRotated), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Right);
                break;
          
            case RoomType.CrossRoad:
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.CrossRoad), (x, y));
                AddConstructPlace((position, ((int)x, (int)y)));
                break;
         
            case RoomType.LRoad:
                if (IsBlocked(_ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x, (int)y + 1], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.LRoad), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Right);
                break;
       
            case RoomType.LRoadRotated90:
                if (IsBlocked(_ship[(int)x - 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x, (int)y - 1], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.LRoadRotated90), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Up, Direction.Left);
                break;
           
            case RoomType.LRoadRotated180:
                if (IsBlocked(_ship[(int)x + 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x, (int)y - 1], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.LRoadRotated180), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Left, Direction.Down);
                break;
          
            case RoomType.LRoadRotated270:
                if (IsBlocked(_ship[(int)x + 1, (int)y], 0f, 180f, 270f) &&
                    IsBlocked(_ship[(int)x, (int)y + 1], 0f, 90f, 0f)) return;
                RemoveRoom(((int)x, (int)y));
                AddToShip(_actorManager.CreateObject(position, RoomType.LRoadRotated270), (x, y));
                AddIndividualConstructPlace(((int)x, (int)y), position, Direction.Down, Direction.Right);
                break;
        }
    }

    private void AddToShip(GameObject construction, params (double x, double y)[] coords)
    {
        foreach (var coord in coords)
        {
            _ship[(int)coord.x, (int)coord.y] = construction;
        }
    }
    
    private static double GetXCoordinate(Vector2 position) => (double)-Mathf.RoundToInt(position.y) / 10 + 24.5;

    private static double GetYCoordinate(Vector2 position) => (double)Mathf.RoundToInt(position.x) / 10 + 24.5;

    private void RemoveRoom(params (int x, int y)[] coords) => coords.ToList().ForEach(coord =>
    {
        Destroy(_ship[coord.x, coord.y]);
        _ship[coord.x, coord.y] = null;
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
                    if (_ship[arrayPosition.x - 1, arrayPosition.y] == null)
                        CreateRoom(RoomType.ConstructRotatedPlace,
                            new Vector2(objectPosition.x, objectPosition.y + 10));
                    break;
                
                case Direction.Left:
                    if (_ship[arrayPosition.x, arrayPosition.y - 1] == null)
                        CreateRoom(RoomType.ConstructPlace, new Vector2(objectPosition.x - 10, objectPosition.y));
                    break;
                
                case Direction.Right:
                    if (_ship[arrayPosition.x, arrayPosition.y + 1] == null)
                        CreateRoom(RoomType.ConstructPlace, new Vector2(objectPosition.x + 10, objectPosition.y));
                    break;
                
                case Direction.Down:
                    if (_ship[arrayPosition.x + 1, arrayPosition.y] == null)
                        CreateRoom(RoomType.ConstructRotatedPlace,
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
                var element = _ship[y, x];
                var elementString = element != null ? element.name : "  null  ";
                elementString = elementString.PadRight(20, ' ');
                output += "[" + y.ToString("D2") + " / " + x.ToString("D2") + ": " + elementString + "] ";
            }

            output += "\n";
        }

        Debug.Log(output);
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