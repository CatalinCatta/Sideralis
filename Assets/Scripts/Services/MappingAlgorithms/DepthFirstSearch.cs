using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DepthFirstSearch
{
    private bool[,] _visited;
    private MappingHelper _mappingHelper;
    private List<(int, int)> _destroyedPositions;

    public bool IsSafeToRemove(GameObject room, GameObject[,] ship)
    {
        if (room.TryGetComponent<Room>(out var roomInstance) && roomInstance.CrewsNumber() != 0)
            return false;
        
        _mappingHelper = new MappingHelper(ship);
        _visited = new bool[_mappingHelper.Rows, _mappingHelper.Columns];
        _destroyedPositions = new List<(int, int)>();
            
        MapAllPositions();
        var tempBucket = _visited;
        
        DestroyObject(room);
        _visited = new bool[_mappingHelper.Rows, _mappingHelper.Columns];

        MapAllPositions();

        foreach (var (x, y) in _destroyedPositions)
            _visited[x, y] = true;

        return ComparePositions(tempBucket);
    }
  
    private void MapAllPositions()
    {
        var (x, y) = GetFirstPosition();
        ConnectionMapper(x, y);
    }

    private bool ComparePositions(bool[,] oldVisitedList)
    {
        for (var i = 0; i < _visited.GetLength(0); i++)
            for (var j = 0; j < _visited.GetLength(1); j++)
                if (_visited[i,j] != oldVisitedList[i,j])
                    return false;

        return true;
    }
    
    private void DestroyObject(Object room)
    {
        for (var i = 0; i < _mappingHelper.Rows; i++)
        {
            for (var j = 0; j < _mappingHelper.Columns; j++)
            {
                if (_mappingHelper.Ship[i, j] != room) continue;
                _mappingHelper.Ship[i, j] = null;
                _destroyedPositions.Add((i, j));
            }
        }
    }

    private (int, int) GetFirstPosition()
    {
        for (var i = 0; i < _mappingHelper.Rows; i++)
            for (var j = 0; j < _mappingHelper.Columns; j++)
                if (_mappingHelper.Ship[i, j] != null)
                    return (i, j);

        throw new Exception("Cannot find any rooms or road in the Ship.");
    }

    private void ConnectionMapper(int x, int y)
    {
        _visited[x, y] = true;

        foreach (var direction in Utilities.Directions)
        {
            var newX = x + direction[0];
            var newY = y + direction[1];
            if (!_mappingHelper.IsNotValid((newX, newY), (x, y), direction) && !_visited[newX, newY])
                ConnectionMapper(newX, newY);
        }
    }
    
}