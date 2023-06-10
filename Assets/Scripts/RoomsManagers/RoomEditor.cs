using JetBrains.Annotations;
using UnityEngine;

public class RoomEditor : MonoBehaviour
{
    private ActorManager _actorManager;
    private SpaceShipManager _shipManager;
    private DepthFirstSearch _depthFirstSearch;
    private PrefabStorage _prefabStorage;
    public Vector2 lastConstructedObjectPosition;
    public bool successfullyMoved;
    
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
        Utilities.SetTransparency(transformObject, 0.5f);
        DeactivateAllHighlights(transformObject.gameObject);
        successfullyMoved = false;
    }

    public void EndMoveRoom(Transform transformObject, ObjectType objectType)
    {
        if (successfullyMoved)
        {
            if (transformObject.gameObject.TryGetComponent<Room>(out var oldRoom))
            {
                var (x, y) = Utilities.GetPositionInArrayOfCoordinate(lastConstructedObjectPosition);
                var newRoom = _shipManager.Ship[(int)x, (int)y].GetComponent<Room>();

                newRoom.actualCapacity = oldRoom.actualCapacity;
                newRoom.lvl = oldRoom.lvl;
            }

            _shipManager.RemoveObjectFrom(Utilities.GetPositionInArrayOfCoordinate(transformObject.position),
                objectType);
        }
        
        _prefabStorage.constructPlacesParent.SetActive(false);
        Utilities.SetTransparency(transformObject, 1);
        transformObject.GetChild(2).gameObject.SetActive(false);
        HighlightMovableObjects();
        successfullyMoved = false;
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

    public void DeactivateHighlights() =>
        DeactivateAllHighlights();
    
    private void DeactivateAllHighlights([CanBeNull] GameObject exception = null)
    {
        if (_shipManager == null)
            return;
        
        for (var i = 0; i < _shipManager.Ship.GetLength(0); i++)
        for (var j = 0; j < _shipManager.Ship.GetLength(1); j++)
        {
            if (_shipManager.Ship[i, j] == null || _shipManager.Ship[i, j].TryGetComponent<ConstructPlace>(out _)) 
                continue;
            
            if (exception != null && _shipManager.Ship[i, j] == exception)
                Utilities.SetTransparency(exception.transform.GetChild(2).transform, 0);
            
            else
                _shipManager.Ship[i, j].transform.GetChild(2).gameObject.SetActive(false);
        }

        if (exception == null)
            _actorManager.StopEditor();
    }
    
    private void DeleteRoom()
    {
        _depthFirstSearch.IsSafeToRemove(_actorManager.currentObject, _shipManager.Ship);
    }
}
