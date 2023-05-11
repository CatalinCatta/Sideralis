using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public GameObject bigRoom;
    public GameObject mediumRoom;
    public GameObject smallRoom;
    public GameObject constructPlace;
    public Transform constructPlaceParent;
    public Transform spaceSheep;
    
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
}
