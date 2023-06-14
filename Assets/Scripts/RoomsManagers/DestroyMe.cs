using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType;
    private RoomEditor _roomEditor;
    
    private void Awake() => 
        _roomEditor = FindObjectOfType<RoomEditor>();
    
    public void OnMouseDown() =>
        _roomEditor.RemoveRoom(transform.parent, Utilities.CheckObjectTypeIntegrity(_objectType, transform.rotation));
}
