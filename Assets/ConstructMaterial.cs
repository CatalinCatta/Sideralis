using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ConstructMaterial  : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private Vector3 startPosition;
	private GameObject draggingClone;

	public void OnBeginDrag(PointerEventData eventData)
	{
		startPosition = transform.position;

		// Make draggable object invisible
		var canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;

		// Create dragging clone
		draggingClone = new GameObject("DraggingClone");
		draggingClone.transform.SetParent(transform.parent);
		draggingClone.transform.SetAsLastSibling(); // Render on top
		draggingClone.transform.localScale = Vector3.one;

		var draggingImage = draggingClone.AddComponent<Image>();
		draggingImage.maskable = false;
		draggingImage.sprite = GetComponent<Image>().sprite;
		// Debug.Log(GetComponent<RectTransform>().sizeDelta);
		draggingImage.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent

		var mainCamera = Camera.main;
		var cameraSize = mainCamera.orthographicSize / 42;
		
		draggingImage.rectTransform.sizeDelta = new Vector2(70f / cameraSize, 28f / cameraSize);
		
		draggingClone.transform.position = Input.mousePosition;
		// draggingClone.maskable = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		draggingClone.transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		// Make draggable object visible again
		CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1;

		// Destroy dragging clone
		Destroy(draggingClone);

		// Check if dropped onto a drop zone in the game
		// Ray ray = Camera.main.ScreenPointToRay(eventData.position);
		// RaycastHit hit;
		// if (Physics.Raycast(ray, out hit))
		// {
		// 	GameObject dropZone = hit.collider.gameObject;
		// 	if (dropZone.GetComponent<DropZone>() != null)
		// 	{
		// 		// Send drop event to drop zone object
		// 		ExecuteEvents.Execute<IDropHandler>(dropZone, eventData, ExecuteEvents.dropHandler);
		// 	}
		// }
	}
}
