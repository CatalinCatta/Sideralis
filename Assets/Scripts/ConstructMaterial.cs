using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [RequireComponent(typeof(Image))]
[RequireComponent(typeof(BoxCollider2D))]
public class ConstructMaterial  : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private GameObject _draggingClone;
	public GameObject parent;

	public void OnBeginDrag(PointerEventData eventData)
	{
		// Make draggable object invisible
		var canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;

		// Create dragging clone
		_draggingClone = new GameObject("Dragging Clone");
		_draggingClone.transform.SetParent(parent.transform);
		_draggingClone.transform.SetAsLastSibling(); // Render on top
		_draggingClone.transform.localScale = Vector3.one;

		var rb = _draggingClone.AddComponent<Rigidbody2D>();
		rb.isKinematic = true;
		
		var draggingImage = _draggingClone.AddComponent<SpriteRenderer>();

		draggingImage.sprite = GetComponent<Image>().sprite;
		// Debug.Log(GetComponent<RectTransform>().sizeDelta);
		draggingImage.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent

		var cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_draggingClone.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -5f);
		// draggingImage.transform.position.z = -5f;
		_draggingClone.transform.localScale  = new Vector3(5.5f, 5.5f, 1f);
		//_draggingClone.transform.position = Input.mousePosition;
		// _draggingClone.maskable = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		var cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_draggingClone.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -5f);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		// Make draggable object visible again
		var canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1;

		// Destroy dragging clone
		Destroy(_draggingClone);

	}
}
