using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace : MonoBehaviour, IDropHandler
{
    private SpaceShipManager _spaceShipManager;

    private void Start() =>
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();

    /// <summary>
    /// Called when an object is dropped onto this construct place.
    /// Creates a room based on the dropped object's type at the position of this construct place.
    /// </summary>
    /// <param name="eventData">The pointer event data for the drop event.</param>
    public void OnDrop(PointerEventData eventData) =>
        _spaceShipManager.CreateRoom(eventData.pointerDrag.GetComponent<ConstructMaterial>().objectType,
            transform.position);
}