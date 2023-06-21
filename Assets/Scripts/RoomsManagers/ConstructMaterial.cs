using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class ConstructMaterial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public ObjectType objectType;
    [SerializeField] public Resource resourceType;
    
    private GameObject _draggingClone;
    private Transform _draggingCloneTransform;
    private PrefabStorage _prefabStorage;
    private ActorManager _actorManager;
    private RoomEditor _roomEditor;
    private SpaceShipManager _spaceShip;

    private void Awake()
    { 
        _prefabStorage = FindObjectOfType<PrefabStorage>();
        _actorManager = FindObjectOfType<ActorManager>();
        _roomEditor = FindObjectOfType<RoomEditor>();
        _spaceShip = FindObjectOfType<SpaceShipManager>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        objectType = Utilities.CheckObjectTypeIntegrity(objectType, transform.rotation);
        if (TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
        }
        else if (transform.parent.TryGetComponent<Room>(out var room))
            resourceType = room.roomResourcesType;

        _draggingClone = new GameObject("Dragging Clone");

        _draggingCloneTransform = _draggingClone.transform;
        _draggingCloneTransform.SetParent(_prefabStorage.constructMaterialCloneParent);
        _draggingCloneTransform.SetAsLastSibling();
        _draggingCloneTransform.rotation = transform.rotation;
        
        var rb = _draggingClone.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        var draggingImage = _draggingClone.AddComponent<SpriteRenderer>();
        draggingImage.sprite = GetImageForClone();
        draggingImage.color = new Color(1f, 1f, 1f, 0.5f);

        var cameraPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _draggingCloneTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, -5f);
        _draggingCloneTransform.localScale = transform.localScale * 10;

        GameObject movingObject = null;
        
        if (_actorManager.moveRoomMode)
        {
            var parent = transform.parent;
            
            _roomEditor.StartMoveRoom(parent);
            movingObject = parent.gameObject;
        }
        
        _spaceShip.CreateConstructPlacesFor(Utilities.GetSizeOfObject(objectType), objectType , movingObject);
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
        
        if (_actorManager.moveRoomMode)
            _roomEditor.EndMoveRoom(transform.parent, objectType);

        if (TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
        }
        
        _actorManager.DestroyAllChildrenOf(_prefabStorage.constructPlacesParent.gameObject);
        Destroy(_draggingClone);
    }

    private Sprite GetImageForClone() =>
        objectType switch
        {
            ObjectType.SmallRoom =>
                _prefabStorage.smallRoomSprites[(int)resourceType],

            ObjectType.MediumRoom =>
                _prefabStorage.mediumRoomSprites[(int)resourceType],

            ObjectType.RotatedMediumRoom =>
                _prefabStorage.mediumRoomSprites[(int)resourceType],

            ObjectType.BigRoom =>
                _prefabStorage.largeRoomSprites[(int)resourceType],

            ObjectType.Road =>
                _prefabStorage.roadSprite,

            ObjectType.RoadRotated =>
                _prefabStorage.roadSprite,

            ObjectType.CrossRoad =>
                _prefabStorage.crossRoadSprite,

            ObjectType.LRoad =>
                _prefabStorage.lRoadSprite,

            ObjectType.LRoadRotated90 =>
                _prefabStorage.lRoadSprite,

            ObjectType.LRoadRotated180 =>
                _prefabStorage.lRoadSprite,

            ObjectType.LRoadRotated270 =>
                _prefabStorage.lRoadSprite,

            _ => throw new Exception("Invalid object type")
        };

}