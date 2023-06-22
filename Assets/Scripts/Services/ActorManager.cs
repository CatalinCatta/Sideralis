#nullable enable
using System;
using UnityEngine;
using System.Linq;

public class ActorManager : MonoBehaviour
{
    
    private PrefabStorage _prefabStorage;
    
    /// <summary>
    /// Currently selected crew number.
    /// </summary>
    public int selectedCrewNumber;

    /// <summary>
    /// The current room where the mouse is positioned.
    /// </summary>
    public Room? currentRoom;

    /// <summary>
    /// The current object(Road/Room) where the mouse is positioned.
    /// </summary>
    public GameObject? currentObject;
    
    public bool moveRoomMode;
    
    public bool deleteRoomMode;

    public void EnterMoveRoomMode() =>
        moveRoomMode = true;
    
    public void EnterDeleteRoomMode() =>
        deleteRoomMode = true;

    private void Awake() => 
        _prefabStorage = transform.GetComponent<PrefabStorage>();
    
    public void StopEditor()
    {
        moveRoomMode = false;
        deleteRoomMode = false;
    }
    
    /// <summary>
    /// Creates and returns a game object based on the provided position and object type.
    /// </summary>
    /// <param name="position">The position where the object will be created.</param>
    /// <param name="objectType">The type of object to be created.</param>
    /// <param name="parentLocation">*Optional parameter*. Represent other location for spawn.</param>
    /// <returns>The created game object.</returns>
    public GameObject CreateObject(Vector3 position, ObjectType objectType, Transform? parentLocation = null)
    {
        var rotation = transform.rotation;
        parentLocation = parentLocation == null ? transform : parentLocation;
        
        return objectType switch
        {
            ObjectType.ConstructPlace => Instantiate(_prefabStorage.constructPlace, position, rotation,
                _prefabStorage.constructPlacesParent),

            ObjectType.ConstructRotatedPlace => Instantiate(_prefabStorage.constructPlace, position, Quaternion.Euler(0f, 0f, 90f),
                _prefabStorage.constructPlacesParent),

            ObjectType.SmallRoom => Instantiate(_prefabStorage.smallRoom, position, rotation, parentLocation),

            ObjectType.MediumRoom => Instantiate(_prefabStorage.mediumRoom, position, rotation, parentLocation),

            ObjectType.RotatedMediumRoom => Instantiate(_prefabStorage.mediumRoom, position, Quaternion.Euler(0f, 0f, 90f),
                parentLocation),

            ObjectType.BigRoom => Instantiate(_prefabStorage.bigRoom, position, rotation, parentLocation),

            ObjectType.Road => Instantiate(_prefabStorage.road, position, rotation, parentLocation),

            ObjectType.RoadRotated => Instantiate(_prefabStorage.road, position, Quaternion.Euler(0f, 0f, 90f), parentLocation),

            ObjectType.CrossRoad => Instantiate(_prefabStorage.crossRoad, position, rotation, parentLocation),

            ObjectType.LRoad => Instantiate(_prefabStorage.lRoad, position, rotation, parentLocation),

            ObjectType.LRoadRotated90 => Instantiate(_prefabStorage.lRoad, position, Quaternion.Euler(0f, 0f, 90f), parentLocation),

            ObjectType.LRoadRotated180 => Instantiate(_prefabStorage.lRoad, position, Quaternion.Euler(0f, 0f, 180f), parentLocation),

            ObjectType.LRoadRotated270 => Instantiate(_prefabStorage.lRoad, position, Quaternion.Euler(0f, 0f, 270f), parentLocation),

            ObjectType.TRoad => Instantiate(_prefabStorage.tRoad, position, rotation, parentLocation),

            ObjectType.TRoadRotated90 => Instantiate(_prefabStorage.tRoad, position, Quaternion.Euler(0f, 0f, 90f), parentLocation),

            ObjectType.TRoadRotated180 => Instantiate(_prefabStorage.tRoad, position, Quaternion.Euler(0f, 0f, 180f), parentLocation),

            ObjectType.TRoadRotated270 => Instantiate(_prefabStorage.tRoad, position, Quaternion.Euler(0f, 0f, 270f), parentLocation),

            ObjectType.Crew => Instantiate(_prefabStorage.crew, new Vector3(position.x, position.y, -5), rotation,
                parentLocation),

            ObjectType.Pointer => Instantiate(_prefabStorage.pointer, new Vector3(position.x, position.y, -5), rotation,
                parentLocation),

            ObjectType.MergeButton => Instantiate(_prefabStorage.mergeButton, position, rotation,
                parentLocation),

            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, "This object cannot be created")
        };
    }

    public void DestroyAllChildrenOf(GameObject parent) =>
        parent.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
}