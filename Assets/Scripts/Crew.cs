using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Crew : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _crewSelected;
    private bool _selectingCrew;
    private ActorManager _actorManager;
    private SpaceShipManager _spaceShipManager;
    public Room room;
    private float movementDuration = 1;
    private bool isMoving;
    
    void Start()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _actorManager = FindObjectOfType<ActorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void Update()
    {
        if (_crewSelected && Input.GetMouseButtonDown(0) && !_selectingCrew && _actorManager.curentRoom != null && _actorManager.curentRoom.CrewSpaceLeft>= _actorManager.selectedCrewNumber && !isMoving)
        {
            StartCoroutine(MoveCrew(FindShortestPath(_spaceShipManager.FindRoomPosition(room), _spaceShipManager.FindRoomPosition(_actorManager.curentRoom)), movementDuration));
            room.crews = room.crews.Where(crew => crew != this).ToList();
            _actorManager.curentRoom.crews.Add(this);
            room = _actorManager.curentRoom;
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
            
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
    
    private static readonly int[][] Directions =
    {
        new [] {-1, 0},  // Up
        new [] {1, 0},   // Down
        new [] {0, -1},  // Left
        new [] {0, 1}    // Right
    };
    
    private List<(int x, int y)> FindShortestPath((int x, int y) startingRoom, (int x, int y) arriveRoom)
    {
        var rows = _spaceShipManager.ship.GetLength(0);
        var columns = _spaceShipManager.ship.GetLength(1);

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

            foreach (var direction in Directions)
            {
                var newX = currentPosition.x + direction[0];
                var newY = currentPosition.y + direction[1];

                if (IsValidPosition(newX, newY, rows, columns) && _spaceShipManager.ship[newX, newY] != null && !visited[newX, newY] && !_spaceShipManager.ship[newX, newY].TryGetComponent<ConstructPlace>(out _))
                {
                    visited[newX, newY] = true;
                    parent[newX, newY] = currentPosition;
                    queue.Enqueue((newX, newY));
                }
            }
        }

        return null;
    }

    private IEnumerator MoveCrew(List<(int x, int y)> movements, float movementDuration)
    {
        isMoving = true;
        foreach (var move in movements)
        {
            var targetPosition = GetPositionForCoordinate(move.x, move.y);
            var startPosition = transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < movementDuration)
            {
                elapsedTime += Time.deltaTime;

                var t = Mathf.Clamp01(elapsedTime / movementDuration); 
                transform.position = Vector3.Lerp(startPosition, targetPosition, t); 

                yield return null;
            }

            transform.position = targetPosition;
        }

        isMoving = false;
    }
 
    private static Vector3 GetPositionForCoordinate(double x, double y) => new ((float)((y - 24.5) * 10), (float)((24.5 - x) * 10), -5f);
    
    private List<(int x, int y)> BacktrackPath((int x, int y)?[,] parent, (int x, int y)? end)
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

    private bool IsValidPosition(int x, int y, int rows, int columns)
    {
        return x >= 0 && x < rows && y >= 0 && y < columns;
    }
}