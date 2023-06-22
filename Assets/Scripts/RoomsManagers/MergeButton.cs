using UnityEngine;

public class MergeButton : MonoBehaviour
{
    private RoomMerger _roomMerger;
    private ActorManager _actorManager;
    private CameraController _cameraController;
    
    private void Awake()
    {
        var grandParent = transform.parent.parent;
        _actorManager = grandParent.GetComponent<ActorManager>();
        _roomMerger = grandParent.GetComponent<RoomMerger>();
        _cameraController = FindObjectOfType<CameraController>();
    }
    
    private void OnMouseDown()
    {
        if (_cameraController.IsPointerOverUIObject())
            return;
        
        _roomMerger.MergeRoom(transform);
        _actorManager.DestroyAllChildrenOf(_roomMerger.parent);
        _roomMerger.CreateMergingPoints();
    }
}
