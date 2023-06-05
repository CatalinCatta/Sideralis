using UnityEngine;

public class RoomEditor : MonoBehaviour
{
    private ActorManager _actorManager;
    private SpaceShipManager _shipManager;
    private DepthFirstSearch _depthFirstSearch;

    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        _shipManager = transform.GetComponent<SpaceShipManager>();
        _depthFirstSearch = new DepthFirstSearch();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || _actorManager.currentRoom == null) 
            return;
        
        if (_actorManager.MoveRoomMode)
            MoveRoom();

        if (_actorManager.DeleteRoomMode)
            DeleteRoom();
    }

    private void MoveRoom()
    {
        Debug.Log(_depthFirstSearch.IsSafeToRemove(_actorManager.currentRoom.transform.gameObject, _shipManager.Ship));
    }

    private void DeleteRoom()
    {
        _depthFirstSearch.IsSafeToRemove(_actorManager.currentRoom.transform.gameObject, _shipManager.Ship);
    }
}
