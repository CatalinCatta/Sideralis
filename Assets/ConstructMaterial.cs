using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ConstructMaterial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
	
	private readonly Dictionary<int,GameObject> _mDraggingIcons = new();
	private readonly Dictionary<int, RectTransform> _mDraggingPlanes = new();

	public void OnBeginDrag(PointerEventData eventData)
	{
		var canvas = FindInParents<Canvas>(gameObject);
		if (canvas == null)
			return;

		// We have clicked something that can be dragged.
		// What we want to do is create an icon for this.
		_mDraggingIcons[eventData.pointerId] = new GameObject("icon");

		_mDraggingIcons[eventData.pointerId].transform.SetParent (canvas.transform, false);
		_mDraggingIcons[eventData.pointerId].transform.SetAsLastSibling();
		
		var image = _mDraggingIcons[eventData.pointerId].AddComponent<Image>();
		// The icon will be under the cursor.
		// We want it to be ignored by the event system.
		var group = _mDraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
		group.blocksRaycasts = false;

		image.sprite = GetComponent<Image>().sprite;
		image.SetNativeSize();
		
		if (dragOnSurfaces)
			_mDraggingPlanes[eventData.pointerId] = transform as RectTransform;
		else
			_mDraggingPlanes[eventData.pointerId]  = canvas.transform as RectTransform;
		
		SetDraggedPosition(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_mDraggingIcons[eventData.pointerId] != null)
			SetDraggedPosition(eventData);
	}

	private void SetDraggedPosition(PointerEventData eventData)
	{
		if (dragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
			_mDraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;
		
		var rt = _mDraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
		if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(_mDraggingPlanes[eventData.pointerId],
			    eventData.position, eventData.pressEventCamera, out var globalMousePos)) return;
		rt.position = globalMousePos;
		rt.rotation = _mDraggingPlanes[eventData.pointerId].rotation;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_mDraggingIcons[eventData.pointerId] != null)
			Destroy(_mDraggingIcons[eventData.pointerId]);

		_mDraggingIcons[eventData.pointerId] = null;
	}

	private static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;
		
		var t = go.transform.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}
}
