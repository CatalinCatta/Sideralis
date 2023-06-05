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
        if (!Input.GetMouseButtonDown(0) || _actorManager.currentObject == null) 
            return;
        
        if (_actorManager.moveRoomMode)
            MoveRoom();

        if (_actorManager.deleteRoomMode)
            DeleteRoom();
    }

    private void MoveRoom()
    {
        Debug.Log(_depthFirstSearch.IsSafeToRemove(_actorManager.currentObject, _shipManager.Ship));
    }

    private void DeleteRoom()
    {
        _depthFirstSearch.IsSafeToRemove(_actorManager.currentObject, _shipManager.Ship);
    }
}
