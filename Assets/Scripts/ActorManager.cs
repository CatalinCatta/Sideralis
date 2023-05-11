using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bigRoom;
    [SerializeField]
    private GameObject mediumRoom;
    [SerializeField]
    private GameObject smallRoom;
    [SerializeField]
    private GameObject constructPlace;
    [SerializeField]
    private GameObject road;
    [SerializeField]
    private GameObject crossRoad;
    [SerializeField]
    private GameObject lRoad;
    [SerializeField]
    private Transform constructPlaceParent;
    [SerializeField]
    private Transform spaceSheep;
    
    public GameObject CreateBigRoom(Vector2 position)
    {
        return Instantiate(bigRoom,  position, transform.rotation, spaceSheep);
    }
    public GameObject CreateMediumRoom(Vector2 position)
    {
        return Instantiate(mediumRoom, position, transform.rotation, spaceSheep);
    }
    public GameObject CreateRotatedMediumRoom(Vector2 position)
    {
        return Instantiate(mediumRoom, position, Quaternion.Euler(0f, 0f, 90f), spaceSheep);
    }
    public GameObject CreateSmallRoom(Vector2 position)
    {
        return Instantiate(smallRoom,  position, transform.rotation, spaceSheep);
    }
    public GameObject CreateConstructPlace(Vector2 position)
    {
        return Instantiate(constructPlace,  position, transform.rotation, constructPlaceParent);
    }
    public GameObject CreateConstructRotatedPlace(Vector2 position)
    {
        return Instantiate(constructPlace,  position, Quaternion.Euler(0f, 0f, 90f), constructPlaceParent);
    }
    public GameObject CreateRoad(Vector2 position)
    {
        return Instantiate(road,  position, transform.rotation, spaceSheep);
    }
    public GameObject CreateRoadRotated(Vector2 position)
    {
        return Instantiate(road,  position, Quaternion.Euler(0f, 0f, 90f), spaceSheep);
    }
    public GameObject CreateCrossRoad(Vector2 position)
    {
        return Instantiate(crossRoad,  position, transform.rotation, spaceSheep);
    }
    public GameObject CreateLRoad(Vector2 position)
    {
        return Instantiate(lRoad,  position, transform.rotation, spaceSheep);
    }
    public GameObject CreateLRoadRotated90(Vector2 position)
    {
        return Instantiate(lRoad,  position, Quaternion.Euler(0f, 0f, 90f), spaceSheep);
    }
    public GameObject CreateLRoadRotated180(Vector2 position)
    {
        return Instantiate(lRoad,  position, Quaternion.Euler(0f, 0f, 180f), spaceSheep);
    }
    public GameObject CreateLRoadRotated270(Vector2 position)
    {
        return Instantiate(lRoad,  position, Quaternion.Euler(0f, 0f, 270f), spaceSheep);
    }
}
