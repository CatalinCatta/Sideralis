using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class ConstructMaterial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public GameObject parent;
    [SerializeField] public ObjectType objectType;
    
    private GameObject _draggingClone;
    private Transform _draggingCloneTransform;
    private PrefabStorage _prefabStorage;

    private void Start() =>
        _prefabStorage = FindObjectOfType<PrefabStorage>();

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
        draggingImage.sprite = GetImageForClone();
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

    private Sprite GetImageForClone() =>
        objectType switch
        {
            ObjectType.SmallRoom =>
                _prefabStorage.smallRoomSprite,

            ObjectType.MediumRoom =>
                _prefabStorage.mediumRoomSprite,

            ObjectType.RotatedMediumRoom =>
                _prefabStorage.mediumRotatedRoomSprite,

            ObjectType.BigRoom =>
                _prefabStorage.bigRoomSprite,

            ObjectType.Road =>
                _prefabStorage.roadSprite,

            ObjectType.RoadRotated =>
                _prefabStorage.roadRotatedSprite,

            ObjectType.CrossRoad =>
                _prefabStorage.crossRoadSprite,

            ObjectType.LRoad =>
                _prefabStorage.lRoadSprite,

            ObjectType.LRoadRotated90 =>
                _prefabStorage.lRoadRotated90Sprite,

            ObjectType.LRoadRotated180 =>
                _prefabStorage.lRoadRotated180Sprite,

            ObjectType.LRoadRotated270 =>
                _prefabStorage.lRoadRotated270Sprite,

            _ => throw new Exception("Invalid object type")
        };
}