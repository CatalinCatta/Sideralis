using UnityEngine;

public class ActorManager : MonoBehaviour
{
    [SerializeField] private GameObject bigRoom;

    [SerializeField] private GameObject mediumRoom;

    [SerializeField] private GameObject smallRoom;

    [SerializeField] private GameObject constructPlace;

    [SerializeField] private GameObject road;

    [SerializeField] private GameObject crossRoad;

    [SerializeField] private GameObject lRoad;
    
    [SerializeField] private GameObject crew;

    [SerializeField] private Transform constructPlaceParent;

    [SerializeField] private Transform spaceSheep;

    public int selectedCrewNumber;

    public Room curentRoom;

    public GameObject CreateObject(Vector2 position, ObjectType objectType) => objectType switch
    {
        ObjectType.ConstructPlace => Instantiate(constructPlace, position, transform.rotation, constructPlaceParent),

        ObjectType.ConstructRotatedPlace => Instantiate(constructPlace, position, Quaternion.Euler(0f, 0f, 90f),
            constructPlaceParent),

        ObjectType.SmallRoom => Instantiate(smallRoom, position, transform.rotation, spaceSheep),

        ObjectType.MediumRoom => Instantiate(mediumRoom, position, transform.rotation, spaceSheep),

        ObjectType.RotatedMediumRoom => Instantiate(mediumRoom, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        ObjectType.BigRoom => Instantiate(bigRoom, position, transform.rotation, spaceSheep),

        ObjectType.Road => Instantiate(road, position, transform.rotation, spaceSheep),

        ObjectType.RoadRotated => Instantiate(road, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        ObjectType.CrossRoad => Instantiate(crossRoad, position, transform.rotation, spaceSheep),

        ObjectType.LRoad => Instantiate(lRoad, position, transform.rotation, spaceSheep),

        ObjectType.LRoadRotated90 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        ObjectType.LRoadRotated180 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 180f), spaceSheep),

        ObjectType.LRoadRotated270 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 270f), spaceSheep),
        
        ObjectType.Crew => Instantiate(crew, new Vector3(position.x, position.y, -5), transform.rotation, spaceSheep)
    };
}