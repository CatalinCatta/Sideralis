using UnityEngine;

public class MergeButton : MonoBehaviour
{
    private RoomMerger _roomMerger;
    private ActorManager _actorManager;
    
    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        _roomMerger = transform.parent.parent.GetComponent<RoomMerger>();
    }
    
    private void OnMouseDown()
    {
        _roomMerger.MergeRoom(transform);
        _actorManager.DestroyAllChildrenOf(_roomMerger.parent);
        _roomMerger.CreateMergingPoints();
    }
}
