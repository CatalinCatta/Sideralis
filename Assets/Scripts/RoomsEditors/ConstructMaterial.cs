using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(BoxCollider2D))]
public class ConstructMaterial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public GameObject parent;
    [SerializeField] public ObjectType objectType;
    private GameObject _draggingClone;
    private Transform _draggingCloneTransform;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        _draggingClone = new GameObject("Dragging Clone");

        _draggingCloneTransform = _draggingClone.transform;
        _draggingCloneTransform.SetParent(parent.transform);
        _draggingCloneTransform.SetAsLastSibling();

        var rb = _draggingClone.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        var draggingImage = _draggingClone.AddComponent<SpriteRenderer>();
        draggingImage.sprite = GetComponent<Image>().sprite;
        draggingImage.color = new Color(1f, 1f, 1f, 0.5f);

        var cameraPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _draggingCloneTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, -5f);
        _draggingCloneTransform.localScale = new Vector3(5.5f, 5.5f, 1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var cameraPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _draggingCloneTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, -5f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;

        Destroy(_draggingClone);
    }
}