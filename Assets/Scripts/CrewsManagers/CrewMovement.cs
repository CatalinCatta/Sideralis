using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class CrewMovement : MonoBehaviour
{
    private PrefabStorage _prefabStorage;
    public Room room;
    private readonly float _speed = 1;
    private ActorManager _actorManager;
    private SpaceShipResources _shipResources;
    private bool _crewSelected;
    private bool _isMoving;
    private bool _selectingCrew;
    private SpaceShipManager _spaceShipManager;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private GameObject _pointer;
    private BreadthFirstSearch _breadthFirstSearch;
    private Controls _controls;
    private CameraController _cameraController;
    
    private void Awake()
    {
        _prefabStorage = FindObjectOfType<PrefabStorage>();
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _shipResources = FindObjectOfType<SpaceShipResources>();
        _actorManager = FindObjectOfType<ActorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        _spriteRenderer.sprite = _prefabStorage.crewSprites[(int) SpritesTypes.AfkStatus];
        _breadthFirstSearch = new BreadthFirstSearch();
        _controls = new Controls();
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        _animator.enabled = true;
        _animator.runtimeAnimatorController = _prefabStorage.crewWorkAnimations[(int) WorkAnimation.Work];
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    private void Update()
    {
        if (_crewSelected && _controls.InGame.Interact.triggered && !_selectingCrew && _actorManager.currentRoom != null &&
            _actorManager.currentRoom.CrewSpaceLeft >= _actorManager.selectedCrewNumber && !_isMoving && _actorManager.currentRoom != room && !_actorManager.toolInAction && !_cameraController.IsPointerOverUIObject())
        {
            var finalRoomPosition = _spaceShipManager.FindRoomPosition(_actorManager.currentRoom, false, this);
            var startRoomPosition = _spaceShipManager.FindRoomPosition(room, true, this);

            StartCoroutine(MoveCrew(
                _breadthFirstSearch.GetShortestPath(startRoomPosition,
                    finalRoomPosition, _spaceShipManager.Ship)));

            room.crews[Array.IndexOf(room.crews, this)] = null;
            _actorManager.currentRoom.crews[Array.IndexOf(_actorManager.currentRoom.crews, null)] = transform.GetComponent<Crew>();
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
            _spaceShipManager.CreateObject(ObjectType.Pointer, Utilities.GetInGameCoordinateForPosition(finalRoomPosition.Item1, finalRoomPosition.Item2, -5f), Resource.None);
            _pointer = _spaceShipManager.lastPointer;
            room = _actorManager.currentRoom;

        }

        if (_controls.InGame.Deselect.triggered)
        {
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
        }

        _selectingCrew = false;
    }

    private void OnMouseDown()
    {
        if (_cameraController.IsPointerOverUIObject())
            return;
        
        if (_crewSelected)
        {
            _selectingCrew = false;
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f);
            _actorManager.selectedCrewNumber -= 1;
        }
        else
        {
            _selectingCrew = true;
            _crewSelected = true;
            _spriteRenderer.color = new Color(0.3f, 1f, 0.28f);
            _actorManager.selectedCrewNumber += 1;
        }
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
        if (!collision.gameObject.GetComponent<BoxSelection>() || !_controls.InGame.BoxControll.IsPressed()) return;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _crewSelected = false;
        _actorManager.selectedCrewNumber -= 0;
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

            bool isOnRoad;
            var voidSpeedBoost = 1;
            
            if (_spaceShipManager.Ship[move.x, move.y] == null)
            {
                isOnRoad = true;
                voidSpeedBoost = 2;
            }
            else
                isOnRoad = _spaceShipManager.Ship[move.x, move.y].TryGetComponent<Road>(out _);
                
            var movementDuration = isOnRoad? _speed / 2 : _speed * voidSpeedBoost;

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
        _animator.runtimeAnimatorController = _prefabStorage.crewWorkAnimations[(int) WorkAnimation.Work];
        _isMoving = false;
    }
    
    private enum SpritesTypes
    {
        AfkStatus
    }
    
    private enum WorkAnimation
    {
        Work
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

    private void OnDestroy()
    {
        _shipResources.foodConsumption -= 0.02;
        _shipResources.oxygenConsumption -= 0.02;
        _shipResources.waterConsumption -= 0.02;
    }
}
