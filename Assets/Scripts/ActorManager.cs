using UnityEngine;
using UnityEngine.Serialization;

public class ActorManager : MonoBehaviour
{
    [SerializeField] private GameObject bigRoom;

    [SerializeField] private GameObject mediumRoom;

    [SerializeField] private GameObject smallRoom;

    [SerializeField] private GameObject constructPlace;

    [SerializeField] private GameObject road;

    [SerializeField] private GameObject crossRoad;

    [SerializeField] private GameObject lRoad;

    [SerializeField] private Transform constructPlaceParent;

    [SerializeField] private Transform spaceSheep;

    public int selectedCrewNumber;

    public Room curentRoom;

    public GameObject CreateObject(Vector2 position, RoomType roomType) => roomType switch
    {
        RoomType.ConstructPlace => Instantiate(constructPlace, position, transform.rotation, constructPlaceParent),

        RoomType.ConstructRotatedPlace => Instantiate(constructPlace, position, Quaternion.Euler(0f, 0f, 90f),
            constructPlaceParent),

        RoomType.SmallRoom => Instantiate(smallRoom, position, transform.rotation, spaceSheep),

        RoomType.MediumRoom => Instantiate(mediumRoom, position, transform.rotation, spaceSheep),

        RoomType.RotatedMediumRoom => Instantiate(mediumRoom, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        RoomType.BigRoom => Instantiate(bigRoom, position, transform.rotation, spaceSheep),

        RoomType.Road => Instantiate(road, position, transform.rotation, spaceSheep),

        RoomType.RoadRotated => Instantiate(road, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        RoomType.CrossRoad => Instantiate(crossRoad, position, transform.rotation, spaceSheep),

        RoomType.LRoad => Instantiate(lRoad, position, transform.rotation, spaceSheep),

        RoomType.LRoadRotated90 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep),

        RoomType.LRoadRotated180 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 180f), spaceSheep),

        RoomType.LRoadRotated270 => Instantiate(lRoad, position, Quaternion.Euler(0f, 0f, 270f), spaceSheep)
    };
}