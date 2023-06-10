using UnityEngine;

public class MergeButton : MonoBehaviour
{
    private RoomMerger _roomMerger;
    private ActorManager _actorManager;
    
    private void Awake()
    {
        var grandParent = transform.parent.parent;
        _actorManager = grandParent.GetComponent<ActorManager>();
        _roomMerger = grandParent.GetComponent<RoomMerger>();
    }
    
    private void OnMouseDown()
    {
        _roomMerger.MergeRoom(transform);
        _actorManager.DestroyAllChildrenOf(_roomMerger.parent);
        _roomMerger.CreateMergingPoints();
    }
}
