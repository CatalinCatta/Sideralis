using System;
using UnityEngine;


public class SpaceShipManager : MonoBehaviour
{
    private Room[,] ship = new Room[50,50];

    public void CreateRoom(int roomType, (int x, int y) coord)
    {
        Room room = new();
        switch (roomType)
        {
            case 0:
                room = new SmallRoom();
                break;
            case 1:
                room = new MediumRoom();
                break;
            case 2:
                room = new MediumRoom();
                break;
            case 3:
                room = new BigRoom();
                break;
            
        }
        ship[coord.x, coord.y] = room;
    }

}
