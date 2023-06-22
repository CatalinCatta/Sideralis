using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float _zoomLerpSpeed = 10;
    private const float ZoomFactor = 3f;
    private Camera _cam;
    private Vector3 _dragStartPos;
    private Vector3 _velocity = Vector3.zero;
    private bool _isDragging;
    private bool _hasStartedDragging;
    private float _targetZoom;
    public bool isMouseOverUI;
    private Controls _controls;
    
    
    private void Awake()
    {
        _cam = Camera.main;
        _targetZoom = _cam!.orthographicSize;
        _controls = new Controls();
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    private void LateUpdate()
    {
        if (_controls.InGame.Move.triggered)
            _hasStartedDragging = !_hasStartedDragging;
        
        if (_hasStartedDragging)
        {
            _isDragging = true;
            _dragStartPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(StartDrag(_dragStartPos));
            return;
        }

        _isDragging = false;
        StopAllCoroutines();
    }


    /// <summary>
    /// Sets the isMouseOverUI flag to indicate whether the mouse is over a UI element.
    /// </summary>
    public void OnPointerEnterUI() => 
        isMouseOverUI = true;

    /// <summary>
    /// Clears the isMouseOverUI flag to indicate that the mouse is not over a UI element.
    /// </summary>
    public void OnPointerExitUI() => 
        isMouseOverUI = false;

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