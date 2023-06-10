using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Crew : MonoBehaviour
{
    private PrefabStorage _prefabStorage;
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
    private BreadthFirstSearch _breadthFirstSearch;

    private void Awake()
    {
        _prefabStorage = FindObjectOfType<PrefabStorage>();
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _actorManager = FindObjectOfType<ActorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        _spriteRenderer.sprite = _prefabStorage.crewSprites[(int) SpritesTypes.AfkStatus];
        _breadthFirstSearch = new BreadthFirstSearch();
    }

    private void Update()
    {
        if (_crewSelected && Input.GetMouseButtonDown(0) && !_selectingCrew && _actorManager.currentRoom != null &&
            _actorManager.currentRoom.CrewSpaceLeft >= _actorManager.selectedCrewNumber && !_isMoving && _actorManager.currentRoom != room && !_actorManager.moveRoomMode && !_actorManager.deleteRoomMode)
        {
            var finalRoomPosition = _spaceShipManager.FindRoomPosition(_actorManager.currentRoom, false, this);
            var startRoomPosition = _spaceShipManager.FindRoomPosition(room, true, this);

            StartCoroutine(MoveCrew(
                _breadthFirstSearch.GetShortestPath(startRoomPosition,
                    finalRoomPosition, _spaceShipManager.Ship)));

            room.crews[Array.IndexOf(room.crews, this)] = null;
            _actorManager.currentRoom.crews[Array.IndexOf(_actorManager.currentRoom.crews, null)] = this;
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
            _spaceShipManager.CreateObject(ObjectType.Pointer, Utilities.GetInGameCoordinateForPosition(finalRoomPosition.Item1, finalRoomPosition.Item2, -5f));
            _pointer = _spaceShipManager.lastPointer;
            room = _actorManager.currentRoom;

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

    private List<(int x, int y)> FindShortestPath((int x, int y) startingRoom, (int x, int y) arriveRoom)
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
            var targetPosition = Utilities.GetInGameCoordinateForPosition(move.x, move.y, -5f);
            var elapsedTime = 0f;
            var crewPosition = crewTransform.position;

            var isOnRoad = _spaceShipManager.Ship[move.x, move.y].TryGetComponent<Road>(out _);
            
            var movementDuration = isOnRoad? _speed / 2 : _speed;

            _spaceShipManager.OpenDor(lastPosition, move);
            
            if (move.y > lastPosition.y)
            {
                _animator.runtimeAnimatorController = _prefabStorage.crewAnimations[isOnRoad? (int) AnimationsTypes.SlideSide : (int) AnimationsTypes.RunSide];
                crewTransform.rotation = new Quaternion(0, 180, 0, 0);
            }

            if (move.y < lastPosition.y)
                _animator.runtimeAnimatorController = _prefabStorage.crewAnimations[isOnRoad? (int) AnimationsTypes.SlideSide : (int) AnimationsTypes.RunSide];

            if (move.x < lastPosition.x)
                _animator.runtimeAnimatorController = _prefabStorage.crewAnimations[isOnRoad? (int) AnimationsTypes.SlideUp : (int) AnimationsTypes.RunUp];

            if (move.x > lastPosition.x)
                _animator.runtimeAnimatorController = _prefabStorage.crewAnimations[isOnRoad? (int) AnimationsTypes.SlideDown : (int) AnimationsTypes.RunDown];

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
        _spriteRenderer.sprite = _prefabStorage.crewSprites[(int) SpritesTypes.AfkStatus];
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
