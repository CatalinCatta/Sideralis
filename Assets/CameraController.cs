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
    
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool isDragging = false;
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
        ResetCamera = Camera.main.transform.position;
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
        Vector3 prevPos = startPos;
        while (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = prevPos - pos;
            float dist = move.magnitude;
            float smooth = Mathf.Lerp(0.01f, 0.1f, dist / 50f);
            Vector3 targetPos = Camera.main.transform.position + move;
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