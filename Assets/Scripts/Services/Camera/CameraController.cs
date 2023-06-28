using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Camera _cam;
    private Vector3 _dragStartPos;
    private Vector3 _velocity = Vector3.zero;
    private bool _isDragging;
    private bool _hasStartedDragging;
    private Controls _controls;
    private PinchScrollDetection _pinchScrollDetection;
    private ActorManager _actorManager;
    private UpgradeRoom _upgradeRoom;
    private CrewsOfRoom _crewsOfRoom;
    private CrewStatus _crewStatus;
    private RoomStatus _roomStatus;
    public bool anyOtherUiOnUse;
    
    private void Awake()
    {
        _cam = Camera.main;
        _controls = new Controls();
        _pinchScrollDetection = transform.GetComponent<PinchScrollDetection>();
        _actorManager = FindObjectOfType<ActorManager>();
        _upgradeRoom = FindObjectOfType<UpgradeRoom>();
        _crewsOfRoom = FindObjectOfType<CrewsOfRoom>();
        _crewStatus = FindObjectOfType<CrewStatus>();
        _roomStatus = FindObjectOfType<RoomStatus>();
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    public void MouseInsideAnyUi() =>
        anyOtherUiOnUse = true;
    
    public void MouseOutsideAnyUi() =>
        anyOtherUiOnUse = false;
    
    private void Update()
    {
        if (_pinchScrollDetection.touchCount <= 1)
            return;

        _isDragging = false;
        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        if (_controls.InGame.Move.IsPressed() && _pinchScrollDetection.touchCount < 2 && (!_actorManager.moveRoomMode || _pinchScrollDetection.touchCount == 0) && !_upgradeRoom.inUse)
        {
            _isDragging = true;
            _dragStartPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(StartDrag(_dragStartPos));
        }

        if (!_controls.InGame.Move.WasReleasedThisFrame()) return;
        
        _isDragging = false;
        StopAllCoroutines();
    }

    public bool IsPointerOverUIObject([NotNull] bool roomStupidBug = false)
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        Debug.Log(results.Count > 2 - (roomStupidBug ?1  : 0));
        Debug.Log(_upgradeRoom.inUse);
        Debug.Log(_upgradeRoom.inUse);
        Debug.Log(_crewsOfRoom.inUse);
        Debug.Log(_crewStatus.inUse);
        Debug.Log(_roomStatus.inUse);
        Debug.Log(anyOtherUiOnUse);
        return results.Count > 2 - (roomStupidBug ?1  : 0) || _upgradeRoom.inUse || _crewsOfRoom.inUse || _crewStatus.inUse || _roomStatus.inUse || anyOtherUiOnUse;
    }

    private IEnumerator StartDrag(Vector3 startPos)
    {
        yield return new WaitUntil(() => _isDragging);
        var prevPos = startPos;
        while (_isDragging)
        {
            var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
            var move = prevPos - pos;
            var dist = move.magnitude;
            var smooth = Mathf.Lerp(0.01f, 0.1f, dist / 50f);
            var position = _cam.transform.position;
            var targetPos = position + move;
            position = Vector3.SmoothDamp(position, targetPos, ref _velocity,
                smooth, Mathf.Infinity, Time.deltaTime);
            _cam.transform.position = position;
            prevPos = pos;
            yield return null;
        }
    }
}