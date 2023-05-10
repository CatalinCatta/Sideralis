using System;
using UnityEngine;


public class SpaceShipManager : MonoBehaviour
{
    private GameObject[,] _ship = new GameObject[50,50];
    private ActorManager _actorManager;
    
    public void CreateRoom(int roomType, (int x, int y) coord)
    {
        GameObject room = new();
        switch (roomType)
        {
            case 0:
                room = _actorManager.CreateSmallRoom(coord.x, coord.y);
                break;
            case 1:
                room = _actorManager.CreateMediumRoom(coord.x, coord.y);
                break;
            case 2:
                room = _actorManager.CreateMediumRoom(coord.x, coord.y);
                break;
            case 3:
                room = _actorManager.CreateBigRoom(coord.x, coord.y);
                break;
            
        }
        _ship[coord.x, coord.y] = room;
    }

}
