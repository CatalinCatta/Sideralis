using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crew : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<RuntimeAnimatorController> animations;
    public Room room;
    private readonly float _speed = 1;
    private ActorManager _actorManager;
    private bool _crewSelected;
    private bool _isMoving;
    private bool _selectingCrew;
    private SpaceShipManager _spaceShipManager;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private GameObject _pointer;

    private void Start()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _actorManager = FindObjectOfType<ActorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    private void Update()
    {
        if (_crewSelected && Input.GetMouseButtonDown(0) && !_selectingCrew && _actorManager.currentRoom != null &&
            _actorManager.currentRoom.CrewSpaceLeft >= _actorManager.selectedCrewNumber && !_isMoving && _actorManager.currentRoom != room)
        {
            var finalRoomPosition = _spaceShipManager.FindRoomPosition(_actorManager.currentRoom);
            StartCoroutine(MoveCrew(
                FindShortestPath(_spaceShipManager.FindRoomPosition(room),
                    finalRoomPosition).ToList()));
            room.crews = room.crews.Where(crew => crew != this).ToList();
            _actorManager.currentRoom.crews.Add(this);
            room = _actorManager.currentRoom;
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
            _spaceShipManager.CreateRoom(ObjectType.Pointer, GetPositionForCoordinate(finalRoomPosition.Item1, finalRoomPosition.Item2));
            _pointer = _spaceShipManager.lastPointer;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
        }

        _selectingCrew = false;
    }

    private void OnMouseDown()
    {
        _selectingCrew = true;
        _crewSelected = true;
        _spriteRenderer.color = new Color(0.3f, 1f, 0.28f);
        _actorManager.selectedCrewNumber = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<BoxSelection>() || _crewSelected) return;
        _spriteRenderer.color = new Color(0.3f, 1f, 0.28f);
        _actorManager.selectedCrewNumber += 1;
        _crewSelected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<BoxSelection>() || !Input.GetMouseButton(0)) return;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _crewSelected = false;
        _actorManager.selectedCrewNumber = 0;
    }

    private IEnumerable<(int x, int y)> FindShortestPath((int x, int y) startingRoom, (int x, int y) arriveRoom)
    {
        var rows = _spaceShipManager.Ship.GetLength(0);
        var columns = _spaceShipManager.Ship.GetLength(1);

        var visited = new bool[rows, columns];
        visited[startingRoom.x, startingRoom.y] = true;

        var queue = new Queue<(int x, int y)>();
        queue.Enqueue(startingRoom);

        var parent = new (int x, int y)?[rows, columns];
        parent[startingRoom.x, startingRoom.y] = null;

        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();

            if (currentPosition.x == arriveRoom.x && currentPosition.y == arriveRoom.y)
            {
                var path = BacktrackPath(parent, arriveRoom);
                path.Reverse();
                return path;
            }

            foreach (var direction in Utilities.Directions)
            {
                var newX = currentPosition.x + direction[0];
                var newY = currentPosition.y + direction[1];

                if (!IsValidPosition(newX, newY, rows, columns) ||
                    visited[newX, newY] ||
                    _spaceShipManager.Ship[newX, newY] == null ||
                    (!IsValidRoad(newX, newY, direction[0] * -1, direction[1] * -1) &&
                     !_spaceShipManager.Ship[newX, newY].TryGetComponent<Room>(out _)) ||
                    (!_spaceShipManager.Ship[currentPosition.x, currentPosition.y].TryGetComponent<Room>(out _) &&
                     !IsValidRoad(currentPosition.x, currentPosition.y, direction[0], direction[1]))) 
                    continue;
            
                visited[newX, newY] = true;
                parent[newX, newY] = currentPosition;
                queue.Enqueue((newX, newY));
            }
        }

        return null;
    }

    private IEnumerator MoveCrew(IReadOnlyList<(int x, int y)> movements)
    {
        _animator.enabled = true;
        _isMoving = true;
    
        var lastPosition = movements[0];
        var crewTransform = transform;

        foreach (var move in movements.Skip(1).ToList())
        {
            var targetPosition = GetPositionForCoordinate(move.x, move.y);
            var elapsedTime = 0f;
            var crewPosition = crewTransform.position;

            var isOnRoad = _spaceShipManager.Ship[move.x, move.y].TryGetComponent<Road>(out _);
            
            var movementDuration = isOnRoad? _speed / 2 : _speed;

            _spaceShipManager.OpenDor(lastPosition, move);
            
            if (move.y > lastPosition.y)
            {
                _animator.runtimeAnimatorController = animations[isOnRoad? (int) AnimationsTypes.SlideSide : (int) AnimationsTypes.RunSide];
                crewTransform.rotation = new Quaternion(0, 180, 0, 0);
            }

            if (move.y < lastPosition.y)
                _animator.runtimeAnimatorController = animations[isOnRoad? (int) AnimationsTypes.SlideSide : (int) AnimationsTypes.RunSide];

            if (move.x < lastPosition.x)
                _animator.runtimeAnimatorController = animations[isOnRoad? (int) AnimationsTypes.SlideUp : (int) AnimationsTypes.RunUp];

            if (move.x > lastPosition.x)
                _animator.runtimeAnimatorController = animations[isOnRoad? (int) AnimationsTypes.SlideDown : (int) AnimationsTypes.RunDown];

            while (elapsedTime < movementDuration)
            {
                elapsedTime += Time.deltaTime;
                crewTransform.position = Vector3.Lerp(crewPosition, targetPosition, Mathf.Clamp01(elapsedTime / movementDuration));

                yield return null;
            }

            crewTransform.position = targetPosition;
            crewTransform.rotation = new Quaternion(0, 0, 0, 0);
            lastPosition = move;
        }

        Destroy(_pointer);
        _spriteRenderer.sprite = sprites[(int) SpritesTypes.AfkStatus];
        _animator.enabled = false;
        _isMoving = false;
    }

    private static List<(int x, int y)> BacktrackPath((int x, int y)?[,] parent, (int x, int y)? end)
    {
        var path = new List<(int x, int y)>();
        var current = end;

        while (current != null)
        {
            path.Add((current.Value.x, current.Value.y));
            current = parent[current.Value.x, current.Value.y];
        }

        return path;
    }

    private bool IsValidRoad(int x, int y, int directionX, int directionY)
    {
        var roadObject = _spaceShipManager.Ship[x, y];
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

    private static Vector3 GetPositionForCoordinate(double x, double y) => 
        new ((float)((y - 24.5) * 10), (float)((24.5 - x) * 10), -5f);

    private static bool IsValidPosition(int x, int y, int rows, int columns) =>
        x >= 0 && x < rows && y >= 0 && y < columns;

    private enum SpritesTypes
    {
        AfkStatus
    }
    
    private enum AnimationsTypes
    {
        RunSide,
        RunUp,
        RunDown,
        SlideSide,
        SlideUp,
        SlideDown
    }
}
