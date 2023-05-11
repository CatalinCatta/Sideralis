using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace: MonoBehaviour, IDropHandler
{
    private SpaceShipManager _spaceShipManager;

    private void Start()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        _spaceShipManager.CreateRoom(eventData.pointerDrag.GetComponent<ConstructMaterial>().roomType, transform.position);
    }
}
