using UnityEngine;

public class MergeButton : MonoBehaviour
{
    private RoomMerger _roomMerger;
    
    private void Start() =>
        _roomMerger = transform.parent.parent.GetComponent<RoomMerger>();

    private void OnMouseDown() =>
        _roomMerger.MergeRoom(transform);
}
