using System;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    [SerializeField] private GameObject bigRoom;                // Prefab for a big room object
    [SerializeField] private GameObject mediumRoom;             // Prefab for a medium room object
    [SerializeField] private GameObject smallRoom;              // Prefab for a small room object
    [SerializeField] private GameObject constructPlace;         // Prefab for a construct place object
    [SerializeField] private GameObject road;                   // Prefab for a road object
    [SerializeField] private GameObject crossRoad;              // Prefab for a crossroad object
    [SerializeField] private GameObject lRoad;                  // Prefab for an L-road object
    [SerializeField] private GameObject crew;                   // Prefab for a crew object
    
    [SerializeField] private Transform constructPlaceParent;    // Parent transform for constructed places
    [SerializeField] private Transform spaceSheep;              // Parent transform for spawned objects

    /// <summary>
    /// Currently selected crew number.
    /// </summary>
    public int selectedCrewNumber;

    /// <summary>
    /// The current room where the mouse is positioned.
    /// </summary>
    public Room currentRoom;

    /// <summary>
    /// Creates and returns a game object based on the provided position and object type.
    /// </summary>
    /// <param name="position">The position where the object will be created.</param>
    /// <param name="objectType">The type of object to be created.</param>
    /// <returns>The created game object.</returns>
    public GameObject CreateObject(Vector2 position, ObjectType objectType) => objectType switch
        {
            ObjectType.ConstructPlace => Instantiate(constructPlace, position, transform.rotation,
                constructPlaceParent),

            ObjectType.ConstructRotatedPlace => Instantiate(constructPlace, position, Quaternion.Euler(0f, 0f, 90f),
                constructPlaceParent),

            ObjectType.SmallRoom => Instantiate(smallRoom, position, transform.rotation, spaceSheep),

            ObjectType.MediumRoom => Instantiate(mediumRoom, position, transform.rotation, spaceSheep),

            ObjectType.RotatedMediumRoom => Instantiate(mediumRoom, position, Quaternion.Euler(0f, 0f, 90f),
                spaceSheep),

            ObjectType.BigRoom => Instantiate(bigRoom, position, transform.rotation, spaceSheep),

            ObjectType.Road => Instantiate(road, position, transform.rotation, spaceSheep),

            ObjectType.RoadRotated => Instantiate(road, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

            ObjectType.CrossRoad => Instantiate(crossRoad, position, transform.rotation, spaceSheep),

            ObjectType.LRoad => Instantiate(lRoad, position, transform.rotation, spaceSheep),

            ObjectType.LRoadRotated90 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

            ObjectType.LRoadRotated180 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 180f), spaceSheep),

            ObjectType.LRoadRotated270 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 270f), spaceSheep),

            ObjectType.Crew => Instantiate(crew, new Vector3(position.x, position.y, -5), transform.rotation,
                spaceSheep),

            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, "This object cannot be created")
        };
}