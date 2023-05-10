using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructPlace: MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped item: " + eventData.pointerDrag.name + " onto drop zone: " + gameObject.name);
    }
}
