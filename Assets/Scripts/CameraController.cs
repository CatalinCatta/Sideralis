using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomLerpSpeed = 10;
    private const float ZoomFactor = 3f;
    private Camera _cam;
    private Vector3 _dragStartPos;
    private Vector3 _velocity = Vector3.zero;
    private bool _isDragging;
    private float _targetZoom;
    public bool isMouseOverUI;

    private void Start()
    {
        _cam = Camera.main;
        _targetZoom = _cam!.orthographicSize;
    }

    private void Update()
    {
        if (isMouseOverUI) return;
        var scrollData = Input.GetAxis("Mouse ScrollWheel");
        _targetZoom -= scrollData * ZoomFactor;
        _targetZoom = Mathf.Clamp(_targetZoom, 1f, 50f);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isDragging = true;
            _dragStartPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(StartDrag(_dragStartPos));
        }

        if (!Input.GetMouseButtonUp(1)) return;
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