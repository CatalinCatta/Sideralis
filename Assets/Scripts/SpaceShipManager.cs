using UnityEngine;

public class SpaceShipManager : MonoBehaviour
{
    private readonly GameObject[,] _ship = new GameObject[50, 50];
    private ActorManager _actorManager;

    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        CreateRoom(3, new Vector2(-10, 10));
        CreateRoom(1, new Vector2(10, 15));
        CreateRoom(2, new Vector2(15, 0));
        CreateRoom(0, new Vector2(5, -5));
    }

    public void CreateRoom(int roomType, Vector2 position)
    {
        double x = GetXCoordinate(position);
        double y = GetYCoordinate(position);
        GameObject room;
        switch (roomType)
        {
            case -2:
                room = _actorManager.CreateConstructPlace(position);
                _ship[(int)x, (int)y] = room;
                break;
            case -1:
                room = _actorManager.CreateConstructRotatedPlace(position);
                _ship[(int)x, (int)y] = room;
                break;
            case 0:
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateSmallRoom(position);
                _ship[(int)x, (int)y] = room;
                AddConstructPlace(position, ((int)x, (int)y));
                break;
            case 1:
                RemoveRoom((int)x, (int)(y - 0.5));
                RemoveRoom((int)x, (int)(y + 0.5));
                room = _actorManager.CreateMediumRoom(position);
                _ship[(int)x, (int)(y - 0.5)] = room;
                _ship[(int)x, (int)(y + 0.5)] = room;
                AddConstructPlace(new Vector2(position.x - 5, position.y), ((int)x, (int)(y - 0.5)));
                AddConstructPlace(new Vector2(position.x + 5, position.y), ((int)x, (int)(y + 0.5)));
                break;
            case 2:
                RemoveRoom((int)(x - 0.5), (int)y);
                RemoveRoom((int)(x + 0.5), (int)y);
                room = _actorManager.CreateRotatedMediumRoom(position);
                _ship[(int)(x - 0.5), (int)y] = room;
                _ship[(int)(x + 0.5), (int)y] = room;
                AddConstructPlace(new Vector2(position.x, position.y + 5), ((int)(x - 0.5), (int)y));
                AddConstructPlace(new Vector2(position.x, position.y - 5), ((int)(x + 0.5), (int)y));
                break;
            case 3:
                RemoveRoom((int)(x - 0.5), (int)(y - 0.5));
                RemoveRoom((int)(x - 0.5), (int)(y + 0.5));
                RemoveRoom((int)(x + 0.5), (int)(y - 0.5));
                RemoveRoom((int)(x + 0.5), (int)(y + 0.5));
                room = _actorManager.CreateBigRoom(position);
                _ship[(int)(x - 0.5), (int)(y - 0.5)] = room;
                _ship[(int)(x - 0.5), (int)(y + 0.5)] = room;
                _ship[(int)(x + 0.5), (int)(y - 0.5)] = room;
                _ship[(int)(x + 0.5), (int)(y + 0.5)] = room;
                AddConstructPlace(new Vector2(position.x - 5, position.y + 5), ((int)(x - 0.5), (int)(y - 0.5)));
                AddConstructPlace(new Vector2(position.x + 5, position.y + 5), ((int)(x - 0.5), (int)(y + 0.5)));
                AddConstructPlace(new Vector2(position.x - 5, position.y - 5), ((int)(x + 0.5), (int)(y - 0.5)));
                AddConstructPlace(new Vector2(position.x + 5, position.y - 5), ((int)(x + 0.5), (int)(y + 0.5)));
                break;
            case 4:
                if(
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad) &&
                     linearRoad.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LRoad>(out var lRoad) &&
                     (lRoad.transform.rotation != Quaternion.Euler(0f, 0f, 180f) &&
                      lRoad.transform.rotation != Quaternion.Euler(0f, 0f, 270f))) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad2) &&
                     linearRoad2.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LRoad>(out var lRoad2) &&
                     (lRoad2.transform.rotation != Quaternion.Euler(0f, 0f, 90f) &&
                      lRoad2.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    ((_ship[(int)x - 1, (int)y] == null ||
                      _ship[(int)x - 1, (int)y].TryGetComponent<ConstructPlace>(out var _)) &&
                     (_ship[(int)x + 1, (int)y] == null ||
                      _ship[(int)x + 1, (int)y].TryGetComponent<ConstructPlace>(out var _2))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateRoad(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x - 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y + 10));
                if (_ship[(int)x + 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y - 10));
                break;
            case 5:
                if ((_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LinearRoad>(out var linearRoad1) &&
                     linearRoad1.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LRoad>(out var lRoad1) &&
                     (lRoad1.transform.rotation != Quaternion.Euler(0f, 0f, 0f) &&
                      lRoad1.transform.rotation != Quaternion.Euler(0f, 0f, 270f))) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LinearRoad>(out var linearRoad21) &&
                     linearRoad21.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LRoad>(out var lRoad21) &&
                     (lRoad21.transform.rotation != Quaternion.Euler(0f, 0f, 180f) &&
                      lRoad21.transform.rotation != Quaternion.Euler(0f, 0f, 90f))) ||
                    ((_ship[(int)x, (int)y - 1] == null ||
                      _ship[(int)x, (int)y - 1].TryGetComponent<ConstructPlace>(out var _1)) &&
                     (_ship[(int)x, (int)y + 1] == null ||
                      _ship[(int)x, (int)y + 1].TryGetComponent<ConstructPlace>(out var _21))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateRoadRotated(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x, (int)y - 1] == null) CreateRoom(-2, new Vector2(position.x - 10, position.y));
                if (_ship[(int)x, (int)y + 1] == null) CreateRoom(-2, new Vector2(position.x + 10, position.y));
                break;
            case 6:
                if ((_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LinearRoad>(out var linearRoad12) &&
                     linearRoad12.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LRoad>(out var lRoad12) &&
                     (lRoad12.transform.rotation != Quaternion.Euler(0f, 0f, 270f) &&
                      lRoad12.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LinearRoad>(out var linearRoad212) &&
                     linearRoad212.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LRoad>(out var lRoad212) &&
                     (lRoad212.transform.rotation != Quaternion.Euler(0f, 0f, 180f) &&
                      lRoad212.transform.rotation != Quaternion.Euler(0f, 0f, 90f))) ||
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad112) &&
                     linearRoad112.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LRoad>(out var lRoad112) &&
                     (lRoad112.transform.rotation != Quaternion.Euler(0f, 0f, 270f) &&
                      lRoad112.transform.rotation != Quaternion.Euler(0f, 0f, 180f))) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad22) &&
                     linearRoad22.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LRoad>(out var lRoad22) &&
                     (lRoad22.transform.rotation != Quaternion.Euler(0f, 0f, 90f) &&
                      lRoad22.transform.rotation != Quaternion.Euler(0f, 0f, 0f))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateCrossRoad(position);
                _ship[(int)x, (int)y] = room;
                AddConstructPlace(position, ((int)x, (int)y));
                break;
            case 7:
                if (
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad1123) &&
                     linearRoad1123.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LRoad>(out var lRoad1123) &&
                     (lRoad1123.transform.rotation != Quaternion.Euler(0f, 0f, 270f) &&
                      lRoad1123.transform.rotation != Quaternion.Euler(0f, 0f, 180f))) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LinearRoad>(out var linearRoad2123) &&
                     linearRoad2123.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LRoad>(out var lRoad2123) &&
                     (lRoad2123.transform.rotation != Quaternion.Euler(0f, 0f, 180f) &&
                      lRoad2123.transform.rotation != Quaternion.Euler(0f, 0f, 90f))) ||
                    ((_ship[(int)x - 1, (int)y] == null ||
                      _ship[(int)x - 1, (int)y].TryGetComponent<ConstructPlace>(out var _13)) &&
                     (_ship[(int)x, (int)y + 1] == null ||
                      _ship[(int)x, (int)y + 1].TryGetComponent<ConstructPlace>(out var _213))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateLRoad(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x - 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y + 10));
                if (_ship[(int)x, (int)y + 1] == null) CreateRoom(-2, new Vector2(position.x + 10, position.y));
                break;
            case 8:
                if (
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad11234) &&
                     linearRoad11234.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x - 1, (int)y] != null &&
                     _ship[(int)x - 1, (int)y].TryGetComponent<LRoad>(out var lRoad11234) &&
                     (lRoad11234.transform.rotation != Quaternion.Euler(0f, 0f, 270f) ||
                      lRoad11234.transform.rotation != Quaternion.Euler(0f, 0f, 180f))) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LinearRoad>(out var linearRoad21234) &&
                     linearRoad21234.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LRoad>(out var lRoad21234) &&
                     (lRoad21234.transform.rotation != Quaternion.Euler(0f, 0f, 270f) &&
                      lRoad21234.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    ((_ship[(int)x - 1, (int)y] == null ||
                      _ship[(int)x - 1, (int)y].TryGetComponent<ConstructPlace>(out var _134)) &&
                     (_ship[(int)x, (int)y - 1] == null ||
                      _ship[(int)x, (int)y - 1].TryGetComponent<ConstructPlace>(out var _2134))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateLRoadRotated90(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x - 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y + 10));
                if (_ship[(int)x, (int)y - 1] == null) CreateRoom(-2, new Vector2(position.x - 10, position.y));
                break;
            case 9:
                if (
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad112341) &&
                     linearRoad112341.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LRoad>(out var lRoad112341) &&
                     (lRoad112341.transform.rotation != Quaternion.Euler(0f, 0f, 90f) &&
                      lRoad112341.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LinearRoad>(out var linearRoad212341) &&
                     linearRoad212341.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y - 1] != null &&
                     _ship[(int)x, (int)y - 1].TryGetComponent<LRoad>(out var lRoad212341) &&
                     (lRoad212341.transform.rotation != Quaternion.Euler(0f, 0f, 270f) &&
                      lRoad212341.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    ((_ship[(int)x + 1, (int)y] == null ||
                      _ship[(int)x + 1, (int)y].TryGetComponent<ConstructPlace>(out var _1341)) &&
                     (_ship[(int)x, (int)y - 1] == null ||
                      _ship[(int)x, (int)y - 1].TryGetComponent<ConstructPlace>(out var _21341))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateLRoadRotated180(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x, (int)y - 1] == null) CreateRoom(-2, new Vector2(position.x - 10, position.y));
                if (_ship[(int)x + 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y - 10));
                break;
            case 10:
                if (
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LinearRoad>(out var linearRoad1123412) &&
                     linearRoad1123412.transform.rotation != Quaternion.Euler(0f, 0f, 0f)) ||
                    (_ship[(int)x + 1, (int)y] != null &&
                     _ship[(int)x + 1, (int)y].TryGetComponent<LRoad>(out var lRoad1123412) &&
                     (lRoad1123412.transform.rotation != Quaternion.Euler(0f, 0f, 90f) &&
                      lRoad1123412.transform.rotation != Quaternion.Euler(0f, 0f, 0f))) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LinearRoad>(out var linearRoad2123412) &&
                     linearRoad2123412.transform.rotation != Quaternion.Euler(0f, 0f, 90f)) ||
                    (_ship[(int)x, (int)y + 1] != null &&
                     _ship[(int)x, (int)y + 1].TryGetComponent<LRoad>(out var lRoad2123412) &&
                     (lRoad2123412.transform.rotation != Quaternion.Euler(0f, 0f, 180f) &&
                      lRoad2123412.transform.rotation != Quaternion.Euler(0f, 0f, 90f))) ||
                    ((_ship[(int)x + 1, (int)y] == null ||
                      _ship[(int)x + 1, (int)y].TryGetComponent<ConstructPlace>(out var _13412)) &&
                     (_ship[(int)x, (int)y + 1] == null ||
                      _ship[(int)x, (int)y + 1].TryGetComponent<ConstructPlace>(out var _213412))))
                    break;
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateLRoad(position);
                _ship[(int)x, (int)y] = room;
                if (_ship[(int)x + 1, (int)y] == null) CreateRoom(-2, new Vector2(position.x, position.y - 10));
                if (_ship[(int)x, (int)y + 1] == null) CreateRoom(-2, new Vector2(position.x + 10, position.y));
                break;
        }
    }

    private double GetXCoordinate(Vector2 position)
    {
        return (double)-Mathf.RoundToInt(position.y) / 10 + 24.5;
    }

    private double GetYCoordinate(Vector2 position)
    {
        return (double)Mathf.RoundToInt(position.x) / 10 + 24.5;
    }

    private void RemoveRoom(int x, int y)
    {
        Destroy(_ship[x, y]);
        _ship[x, y] = null;
    }

    private void AddConstructPlace(Vector2 objectPosition, (int x, int y) positionInArray)
    {
        if (_ship[positionInArray.x - 1, positionInArray.y] == null)
            CreateRoom(-2, new Vector2(objectPosition.x, objectPosition.y + 10));
        if (_ship[positionInArray.x + 1, positionInArray.y] == null)
            CreateRoom(-2, new Vector2(objectPosition.x, objectPosition.y - 10));
        if (_ship[positionInArray.x, positionInArray.y - 1] == null)
            CreateRoom(-1, new Vector2(objectPosition.x - 10, objectPosition.y));
        if (_ship[positionInArray.x, positionInArray.y + 1] == null)
            CreateRoom(-1, new Vector2(objectPosition.x + 10, objectPosition.y));
    }


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
    
    
}