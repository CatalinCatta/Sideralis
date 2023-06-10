using UnityEngine;

public class RoomEditor : MonoBehaviour
{
    private ActorManager _actorManager;
    private SpaceShipManager _shipManager;
    private DepthFirstSearch _depthFirstSearch;
    private PrefabStorage _prefabStorage;

    private void Awake()
    {
        _actorManager = transform.GetComponent<ActorManager>();
        _shipManager = transform.GetComponent<SpaceShipManager>();
        _prefabStorage = transform.GetComponent<PrefabStorage>();
        _depthFirstSearch = new DepthFirstSearch();
    }

    public void StartMoveRoom(Transform transformObject)
    {
        _prefabStorage.constructPlacesParent.SetActive(true);
        Utilities.SetTransparencyRecursive(transformObject, 5);
    }

    public void EndMoveRoom()
    {
        _prefabStorage.constructPlacesParent.SetActive(false);
    }

    public void HighlightMovableObjects()
    {
        _actorManager.moveRoomMode = true;
        
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
