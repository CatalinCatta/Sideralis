using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector3 _initialMousePosition, _currentMousePosition;
    private BoxCollider2D _boxCollider;
    private ActorManager _actorManager;
    private CameraController _camera;

    void Start()
    {
        _camera = FindObjectOfType<CameraController>();
        _actorManager = FindObjectOfType<ActorManager>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _actorManager.selectedCrewNumber == 0 && !_camera.isMouseOverUI)
        {
            _lineRenderer.positionCount = 4;
            _initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector3(_initialMousePosition.x, _initialMousePosition.y, -8f));
            _lineRenderer.SetPosition(1, new Vector3(_initialMousePosition.x, _initialMousePosition.y, -8f));
            _lineRenderer.SetPosition(2, new Vector3(_initialMousePosition.x, _initialMousePosition.y, -8f));
            _lineRenderer.SetPosition(3, new Vector3(_initialMousePosition.x, _initialMousePosition.y, -8f));

            _boxCollider = gameObject.AddComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
            _boxCollider.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButton(0) && _lineRenderer.positionCount != 0)
        {
            _currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector3(_initialMousePosition.x, _initialMousePosition.y, -8f));
            _lineRenderer.SetPosition(1, new Vector3(_initialMousePosition.x, _currentMousePosition.y, -8f));
            _lineRenderer.SetPosition(2, new Vector3(_currentMousePosition.x, _currentMousePosition.y, -8f));
            _lineRenderer.SetPosition(3, new Vector3(_currentMousePosition.x, _initialMousePosition.y, -8f));

            transform.position = (_currentMousePosition + _initialMousePosition) / 2;

            _boxCollider.size = new Vector2(
                Mathf.Abs(_initialMousePosition.x - _currentMousePosition.x),
                Mathf.Abs(_initialMousePosition.y - _currentMousePosition.y));
        }

        if (Input.GetMouseButtonUp(0))
        { 
            _lineRenderer.positionCount = 0;
            Destroy(_boxCollider);
            transform.position = Vector3.zero;
        }
    }
}
