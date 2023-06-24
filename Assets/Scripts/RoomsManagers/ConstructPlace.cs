using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace : MonoBehaviour, IDropHandler
{
    private SpaceShipManager _spaceShipManager;
    private RoomEditor _roomEditor;
    private ConstructSelector _constructSelector;
    private CameraController _cameraController;
    private ActorManager _actorManager;
    private PrefabStorage _prefabStorage;
    private Sprite _oldSprite;
    private Sprite _oldChildSprite;
    private Quaternion _oldRotation;

    private void Awake()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _roomEditor = FindObjectOfType<RoomEditor>();
        _constructSelector = FindObjectOfType<ConstructSelector>();
        _cameraController = FindObjectOfType<CameraController>();
        _actorManager = FindObjectOfType<ActorManager>();
        _prefabStorage = FindObjectOfType<PrefabStorage>();
    }

    private void Start()
    {
        var localTransform = transform;
        _oldSprite = localTransform.GetComponent<SpriteRenderer>().sprite;
        _oldChildSprite = localTransform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        _oldRotation = localTransform.GetChild(0).rotation;
    }

    /// <summary>
    /// Called when an object is dropped onto this construct place.
    /// Creates a room based on the dropped object's type at the position of this construct place.
    /// </summary>
    /// <param name="eventData">The pointer event data for the drop event.</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent<ConstructMaterial>(out var constructMaterial))
            return;

        var position = transform.position;

        if (_actorManager.moveRoomMode)
        {
            _roomEditor.lastConstructedObjectPosition = position;
            _roomEditor.successfullyMoved = true;
        }
        else
            _spaceShipManager.CreateObject(constructMaterial.objectType, position, constructMaterial.resourceType);
    }

    public void OnMouseDown()
    {
        if (_constructSelector.currentSelectedImage == null || _cameraController.IsPointerOverUIObject())
            return;
        
        var constructMaterial = _constructSelector.currentSelectedImage.GetComponentInChildren<ConstructMaterial>();
        
        _spaceShipManager.CreateObject(constructMaterial.objectType, transform.position, constructMaterial.resourceType);
        _constructSelector.SelectMe(_constructSelector.currentSelectedImage);
    }

    public void OnMouseEnter()
    {
        if (_actorManager.currentConstructingMaterial == null || _cameraController.IsPointerOverUIObject())
            return;
        
        switch (_actorManager.currentConstructingMaterial.objectType)
        {
            case ObjectType.BigRoom:
                transform.GetComponent<SpriteRenderer>().sprite = _prefabStorage.largeSnappingPoint;
                break;   
            
            case ObjectType.MediumRoom:
            case ObjectType.RotatedMediumRoom:
                transform.GetComponent<SpriteRenderer>().sprite = _prefabStorage.mediumSnappingPoint;
                break;   
            
            default:
                transform.GetComponent<SpriteRenderer>().sprite = _prefabStorage.smallSnappingPoint;
                break;   
        }
        
        transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _actorManager.currentConstructingMaterial.draggingCloneTransform.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).transform.rotation = _actorManager.currentConstructingMaterial.draggingCloneTransform.rotation;

        switch (_actorManager.currentConstructingMaterial.objectType)
        {
            case ObjectType.LRoad:
                transform.GetChild(0).transform.localPosition = new Vector3(0.19f, 0.19f, -1);
                break;
            case ObjectType.LRoadRotated90:
                transform.GetChild(0).transform.localPosition = new Vector3(-0.19f, 0.19f, -1);
                break;
            case ObjectType.LRoadRotated180:
                transform.GetChild(0).transform.localPosition = new Vector3(-0.19f, -0.19f, -1);
                break;
            case ObjectType.LRoadRotated270:
                transform.GetChild(0).transform.localPosition = new Vector3(0.19f, -0.19f, -1);
                break;
            case ObjectType.TRoad:
                transform.GetChild(0).transform.localPosition = new Vector3(0, -0.19f, -1);
                break;
            case ObjectType.TRoadRotated90:
                transform.GetChild(0).transform.localPosition = new Vector3(0.19f, 0, -1);
                break;
            case ObjectType.TRoadRotated180:
                transform.GetChild(0).transform.localPosition = new Vector3(0, 0.19f, -1);
                break;
            case ObjectType.TRoadRotated270:
                transform.GetChild(0).transform.localPosition = new Vector3(-0.19f, 0, -1);
                break;
            default:
                transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
                break;
        }
        
        _actorManager.currentConstructingMaterial.draggingCloneTransform.GetComponent<SpriteRenderer>().color =
            new Color(1, 1, 1, 0);
    }

    public void OnMouseExit()
    {
        if (_actorManager.currentConstructingMaterial == null)
            return;
        
        transform.GetComponent<SpriteRenderer>().color = _actorManager.currentConstructingMaterial.objectType is ObjectType.BigRoom or ObjectType.MediumRoom or ObjectType.RotatedMediumRoom ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 0.25f);
        
        transform.GetComponent<SpriteRenderer>().sprite = _oldSprite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _oldChildSprite;
        transform.GetChild(0).transform.rotation = _oldRotation;
        transform.GetChild(0).transform.localPosition = new Vector3(0, 0, -1);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    
        
        _actorManager.currentConstructingMaterial.draggingCloneTransform.GetComponent<SpriteRenderer>().color =
            new Color(1, 1, 1, 0.5f);
    }
}