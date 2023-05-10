using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public GameObject bigRoom;
    public GameObject mediumRoom;
    public GameObject smallRoom;
    public GameObject constructPlace;

    public GameObject CreateBigRoom(int x, int y)
    {
        return Instantiate(bigRoom,  new Vector3(x, y), transform.rotation);
    }
    public GameObject CreateMediumRoom(int x, int y)
    {
        return Instantiate(mediumRoom,  new Vector3(x, y), transform.rotation);
    }
    public GameObject CreateSmallRoom(int x, int y)
    {
        return Instantiate(smallRoom,  new Vector3(x, y), transform.rotation);
    }
    public GameObject CreateConstructPlace(int x, int y)
    {
        return Instantiate(constructPlace,  new Vector3(x, y), transform.rotation);
    }
}
