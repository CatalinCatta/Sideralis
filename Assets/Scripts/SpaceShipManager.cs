using System;
using UnityEngine;


public class SpaceShipManager : MonoBehaviour
{
    private GameObject[,] _ship = new GameObject[50,50];
    private ActorManager _actorManager;
    
    
    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        CreateRoom(3, new Vector2(-10,10));
        CreateRoom(1, new Vector2(10,15));
        CreateRoom(2, new Vector2(15,0));
        CreateRoom(0, new Vector2(5,-5));
        
        CreateRoom(4, new Vector2(-25, 5));
        CreateRoom(4, new Vector2(-25, 15));
        CreateRoom(6, new Vector2(-15, 25));
        CreateRoom(6, new Vector2(-5, 25));
        CreateRoom(6, new Vector2(5, 25));
        CreateRoom(6, new Vector2(15, 25));
        CreateRoom(4, new Vector2(25, 15));
        CreateRoom(4, new Vector2(25, 5));
        CreateRoom(4, new Vector2(25, -5));
    }
    
    public void CreateRoom(int roomType, Vector2 position)
    {
        // Take the inGame position and transform it in coord for list
        var x = (double)-Mathf.RoundToInt(position.y) / 10 + 24.5;
        var y = (double)Mathf.RoundToInt(position.x) / 10 + 24.5;
        GameObject room;
        switch (roomType)
        {
            case 0:
                RemoveRoom((int)x, (int)y);
                room = _actorManager.CreateSmallRoom(position);
                _ship[(int)x, (int)y] = room;
                AddConstructPlace(position, ((int)x, (int)y));
                
                break;
            case 1:
                room = _actorManager.CreateMediumRoom(position);
                _ship[(int)x, (int)(y - 0.5)] = room;
                
                _ship[(int)x, (int)(y + 0.5)] = room;
                
                break;
            case 2:
                room = _actorManager.CreateRotatedMediumRoom(position);
                _ship[(int)(x - 0.5), (int)y] = room;
                
                _ship[(int)(x + 0.5), (int)y] = room;
                
                break;
            case 3:
                room = _actorManager.CreateBigRoom(position);
                _ship[(int)(x - 0.5), (int)(y - 0.5)] = room;
                
                _ship[(int)(x - 0.5), (int)(y + 0.5)] = room;
                
                _ship[(int)(x + 0.5), (int)(y - 0.5)] = room;
                
                _ship[(int)(x + 0.5), (int)(y + 0.5)] = room;
                
                break;
            case 4:
                room = _actorManager.CreateConstructRotatedPlace(position);
                _ship[(int)x, (int)y] = room;
                
                break;
            case 6:
                room = _actorManager.CreateConstructPlace(position);
                _ship[(int)x, (int)y] = room;
                break;
        }
        PrintGameObjectList();
    }

    private void RemoveRoom(int x, int y)
    {
        Destroy(_ship[x, y]);
        _ship[x, y] = null;
    }

    private void AddConstructPlace(Vector2 objectPosition, (int x, int y) positionInArray)
    {
        if (_ship[positionInArray.x-1, positionInArray.y] == null)
        {
            CreateRoom(6, new Vector2(objectPosition.x, objectPosition.y+10));
        }
        if (_ship[positionInArray.x+1, positionInArray.y] == null)
        {
            CreateRoom(6, new Vector2(objectPosition.x, objectPosition.y -10));
        }
        if (_ship[positionInArray.x, positionInArray.y-1] == null)
        {
            CreateRoom(4, new Vector2(objectPosition.x -10 , objectPosition.y));
        }
        if (_ship[positionInArray.x, positionInArray.y+1] == null)
        {
            CreateRoom(4, new Vector2(objectPosition.x +10 , objectPosition.y));
        }
    }

    
    private void PrintGameObjectList()
    {
        var output = "";

        for (var y = 20; y < 30; y++)
        {
            for (var x = 20; x < 30; x++)
            {
                var element = _ship[y, x];
                var elementString = element != null ? element.name : "  null  ";
                elementString = elementString.PadRight(20, ' ');
                output += "[" + y.ToString("D2") + " / " + x.ToString("D2") + ": " + elementString + "] ";
            }
            output += "\n";
        }

        Debug.Log(output);
    }
}
