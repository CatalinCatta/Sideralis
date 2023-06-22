using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace : MonoBehaviour, IDropHandler
{
    private SpaceShipManager _spaceShipManager;
    private RoomEditor _roomEditor;
    private ConstructSelector _constructSelector;

    private void Awake()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _roomEditor = FindObjectOfType<RoomEditor>();
        _constructSelector = FindObjectOfType<ConstructSelector>();
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

        _spaceShipManager.CreateObject(constructMaterial.objectType, position, constructMaterial.resourceType);
        _roomEditor.lastConstructedObjectPosition = position;
        _roomEditor.successfullyMoved = true;
    }

    public void OnMouseDown()
    {
        if (_constructSelector.currentSelectedImage == null)
            return;
        
        var constructMaterial = _constructSelector.currentSelectedImage.GetComponentInChildren<ConstructMaterial>();
        
        var position = transform.position;

        _spaceShipManager.CreateObject(constructMaterial.objectType, position, constructMaterial.resourceType);
        _roomEditor.lastConstructedObjectPosition = position;
        _roomEditor.successfullyMoved = true;
        
        _constructSelector.SelectMe(_constructSelector.currentSelectedImage);
    }
}