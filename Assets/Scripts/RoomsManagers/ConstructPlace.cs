using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace : MonoBehaviour, IDropHandler
{
    private SpaceShipManager _spaceShipManager;
    private RoomEditor _roomEditor;

    private void Awake()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _roomEditor = FindObjectOfType<RoomEditor>();
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
        
        _spaceShipManager.CreateObject(constructMaterial.objectType, position);
        _roomEditor.lastConstructedObjectPosition = position;
        _roomEditor.successfullyMoved = true;
    }

}