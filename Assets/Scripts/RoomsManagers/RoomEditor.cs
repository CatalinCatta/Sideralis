using JetBrains.Annotations;
using UnityEngine;
using System.Linq;

public class RoomEditor : MonoBehaviour
{
    private ActorManager _actorManager;
    private SpaceShipManager _shipManager;
    private DepthFirstSearch _depthFirstSearch;
    public Vector2 lastConstructedObjectPosition;
    public bool successfullyMoved;
    
    private void Awake()
    {
        _actorManager = transform.GetComponent<ActorManager>();
        _shipManager = transform.GetComponent<SpaceShipManager>();
        _depthFirstSearch = new DepthFirstSearch();
    }

    public void StartMoveRoom(Transform transformObject)
    {
         transformObject.GetComponentsInChildren<BoxCollider2D>()
            .ToList()
            .ForEach(boxCollider2D => boxCollider2D.enabled = false);
        
        DeactivateAllHighlights(transformObject.gameObject);
        successfullyMoved = false;
    }

    public void EndMoveRoom(Transform transformObject, ObjectType objectType)
    {

        if (successfullyMoved)
        {
            double oldCapacity = 0;
            var oldLvl = 0;
            var oldResourceType = Resource.None;
            
            if (transformObject.gameObject.TryGetComponent<Room>(out var oldRoom))
            {
                oldCapacity = oldRoom.actualCapacity;
                oldLvl = oldRoom.lvl;
                oldResourceType = oldRoom.roomResourcesType;
            }
            
            _shipManager.RemoveObjectFrom(Utilities.GetPositionInArrayOfCoordinate(transformObject.position),
                objectType);
            
            
            var (x, y) = Utilities.GetPositionInArrayOfCoordinate(lastConstructedObjectPosition);
            _shipManager.CreateObject(objectType, lastConstructedObjectPosition, oldResourceType);
            
            if (_shipManager.Ship[(int)x, (int)y].TryGetComponent<Room>(out var newRoom))
            {
                newRoom.actualCapacity = oldCapacity;
                newRoom.lvl = oldLvl;
            }

        }
        else
        {
            transformObject.GetComponentsInChildren<BoxCollider2D>()
                .ToList()
                .ForEach(boxCollider2D => boxCollider2D.enabled = true);

            Utilities.SetTransparency(transformObject, 1);
            transformObject.GetChild(2).gameObject.SetActive(false);
        }
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
                if (_shipManager.Ship[i, j] != null &&
                    _depthFirstSearch.IsSafeToRemove(_shipManager.Ship[i, j], _shipManager.Ship))
                {
                    _shipManager.Ship[i, j].transform.GetChild(2).gameObject.SetActive(true);
                    Utilities.SetColor(_shipManager.Ship[i, j].transform, new Color(0.35f, 1, 0.35f, 1));
                }
    }
    
    public void HighlightRemovableObjects()
    {
        _actorManager.deleteRoomMode = true;
        
        if (_shipManager == null)
            return;
        
        for (var i = 0; i < _shipManager.Ship.GetLength(0); i++)
            for (var j = 0; j < _shipManager.Ship.GetLength(1); j++)
                if (_shipManager.Ship[i, j] != null &&
                    _depthFirstSearch.IsSafeToRemove(_shipManager.Ship[i, j], _shipManager.Ship))
                {
                    _shipManager.Ship[i, j].transform.GetChild(3).gameObject.SetActive(true);
                    Utilities.SetColor(_shipManager.Ship[i, j].transform, new Color(1f, 0.35f, 0.35f, 1));
                }
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
                if (_shipManager.Ship[i, j] == null) 
                    continue;
                
                Utilities.SetColor(_shipManager.Ship[i, j].transform, new Color(1, 1, 1, 1));
              
                if (exception != null && _shipManager.Ship[i, j] == exception)
                {
                    Utilities.SetTransparency(exception.transform, 0.5f);
                    Utilities.SetTransparency(exception.transform.GetChild(2).transform, 0);
                }
                else
                {
                    _shipManager.Ship[i, j].transform.GetChild(2).gameObject.SetActive(false);
                    _shipManager.Ship[i, j].transform.GetChild(3).gameObject.SetActive(false);
                }
            }

        if (exception == null)
            _actorManager.StopEditor();
    }

    public void RemoveRoom(Transform objectTransform, ObjectType objectType)
    {
        _shipManager.RemoveObjectFrom(Utilities.GetPositionInArrayOfCoordinate(objectTransform.position),
            objectType);
        DeactivateAllHighlights();
        HighlightRemovableObjects();
    } 
}
