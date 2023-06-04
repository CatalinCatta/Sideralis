using System;
using System.Collections.Generic;
using UnityEngine;

public static class BreadthFirstSearch
{
    private static int[,] _distances;
    private static (int, int)[,] _prev;
    private static MappingHelper _mappingHelper;

    public static List<(int, int)> GetShortestPath((int, int) startingPosition,(int, int) endingPosition, GameObject[,] ship)
    {
        _mappingHelper = new MappingHelper(ship);
        _distances = new int[_mappingHelper.Rows, _mappingHelper.Columns];
        _prev = new (int, int)[_mappingHelper.Rows, _mappingHelper.Columns];

        if (FindShortestPath(startingPosition, endingPosition, out var shortestPath) == int.MaxValue) 
            throw new InvalidOperationException("No shortest path found.");
        
        shortestPath.Reverse();
        return shortestPath;
    }

    private static int FindShortestPath((int x, int y) startingPosition, (int x, int y) endingPosition, out List<(int, int)> shortestPath)
    {
        for (var i = 0; i < _distances.GetLength(0); i++)
        {
            for (var j = 0; j < _distances.GetLength(1); j++)
            {
                _distances[i, j] = int.MaxValue;
                _prev[i, j] = (-1, -1);
            }
        }

        var queue = new Queue<(int, int)>();
        queue.Enqueue(startingPosition);
        _distances[startingPosition.x, startingPosition.y] = 0;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (x == endingPosition.x && y == endingPosition.y)
                break;

            foreach (var direction in Utilities.Directions)
                ExploreNeighbor((x, y), (x + direction[0], y + direction[1]), queue, direction);
        }

        shortestPath = ReconstructPath(startingPosition, endingPosition);
        return _distances[endingPosition.x, endingPosition.y];
    }

    private static void ExploreNeighbor((int x, int y) startingPosition, (int x, int y) endingPosition, Queue<(int, int)> queue,IReadOnlyList<int> direction)
    {
        if (_mappingHelper.IsNotValid(endingPosition, startingPosition, direction) || _distances[endingPosition.x, endingPosition.y] != int.MaxValue)
            return;
        
        _distances[endingPosition.x, endingPosition.y] = _distances[startingPosition.x, startingPosition.y] + 1;
        _prev[endingPosition.x, endingPosition.y] = startingPosition;
        queue.Enqueue(endingPosition);
    }

    private static List<(int, int)> ReconstructPath((int x, int y) startingPosition, (int x, int y) endingPosition)
    {
        var path = new List<(int, int)>();
        
        while (endingPosition.x != startingPosition.x || endingPosition.y != startingPosition.y)
        {
            path.Add(endingPosition);
            endingPosition = _prev[endingPosition.x, endingPosition.y];
        }

        path.Add(startingPosition);
        return path;
    }
}
