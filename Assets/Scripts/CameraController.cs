using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraController : MonoBehaviour
{
    private bool isMouseOverUI;
 
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10;
    
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f;
    
    private bool isDragging;
    private Vector3 dragStartPos;
    
    public void OnPointerEnterUI()
    {
        isMouseOverUI = true;
    }

    public void OnPointerExitUI()
    {
        isMouseOverUI = false;
    }
 
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }
 
    void Update()
    {
        if (isMouseOverUI) return;
        var scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 1f, 50f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
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
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPos, ref velocity, smooth, Mathf.Infinity, Time.deltaTime);
            prevPos = pos;
            yield return null;
        }
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
}