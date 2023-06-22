using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] private ObjectType _objectType;
    private RoomEditor _roomEditor;
    private CameraController _cameraController;
    
    private void Awake()
    {
        _roomEditor = FindObjectOfType<RoomEditor>();
        _cameraController = FindObjectOfType<CameraController>();
    }
    
    public void OnMouseDown()
    {
        if (!_cameraController.IsPointerOverUIObject())
            _roomEditor.RemoveRoom(transform.parent, Utilities.CheckObjectTypeIntegrity(_objectType, transform.rotation));
    }
    
}
