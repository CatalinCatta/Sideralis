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

    public void HighlightMovableObjects()
    {
        if (_shipManager == null)
            return;
        
        for (var i = 0; i < _shipManager.Ship.GetLength(0); i++)
            for (var j = 0; j < _shipManager.Ship.GetLength(1); j++)
                if (_shipManager.Ship[i, j] != null && !_shipManager.Ship[i, j].TryGetComponent<ConstructPlace>(out _))
                    if (_depthFirstSearch.IsSafeToRemove(_shipManager.Ship[i, j], _shipManager.Ship))
                        _shipManager.Ship[i, j].transform.GetChild(2).gameObject.SetActive(true);
    }

    public void DeactivateAllHighLights()
    {
        if (_shipManager == null)
            return;
        
        for (var i = 0; i < _shipManager.Ship.GetLength(0); i++) 
            for (var j = 0; j < _shipManager.Ship.GetLength(1); j++)
                if (_shipManager.Ship[i, j] != null && !_shipManager.Ship[i, j].TryGetComponent<ConstructPlace>(out _))
                    _shipManager.Ship[i, j].transform.GetChild(2).gameObject.SetActive(false);
   
        _actorManager.StopEditor();
    }
    
    private void DeleteRoom()
    {
        _depthFirstSearch.IsSafeToRemove(_actorManager.currentObject, _shipManager.Ship);
    }
}
