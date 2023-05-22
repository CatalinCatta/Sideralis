using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomLerpSpeed = 10;

    private Camera cam;
    private Vector3 dragStartPos;

    private bool isDragging;
    public bool isMouseOverUI;
    private float targetZoom;

    private Vector3 velocity = Vector3.zero;
    private readonly float zoomFactor = 3f;

    private void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {
        if (isMouseOverUI) return;
        var scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 1f, 50f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(StartDrag(dragStartPos));
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            StopAllCoroutines();
        }
    }

    public void OnPointerEnterUI()
    {
        isMouseOverUI = true;
    }

    public void OnPointerExitUI()
    {
        isMouseOverUI = false;
    }

    private IEnumerator StartDrag(Vector3 startPos)
    {
        yield return new WaitUntil(() => isDragging);
        var prevPos = startPos;
        while (isDragging)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var move = prevPos - pos;
            var dist = move.magnitude;
            var smooth = Mathf.Lerp(0.01f, 0.1f, dist / 50f);
            var targetPos = Camera.main.transform.position + move;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPos, ref velocity,
                smooth, Mathf.Infinity, Time.deltaTime);
            prevPos = pos;
            yield return null;
        }
    }
}