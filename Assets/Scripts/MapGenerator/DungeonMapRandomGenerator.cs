using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonMapRandomGenerator
{
    #region Public Methods
    public static Tuple<int[,], Vector2, Vector2> InitalizeMaze(int width, int height, int minRoom, int maxRoom)
    {
        int[,] maze = new int[height, width];
        Vector2 currentNode;

        var root = Vector2.right * Random.Range(0, height) + Vector2.up * Random.Range(0, width);
        var q = new Queue<Vector2>();
        var traversalSet = new HashSet<Vector2>();
        var emptyRoom = new List<Vector2>();

        traversalSet.Add(root);
        q.Enqueue(root);
        maze[(int)root.x, (int)root.y] = 1;

        while (q.Count > 0)
        {
            currentNode = q.Dequeue();
            if (maze[(int)currentNode.x, (int)currentNode.y] == 0)
                continue;
            var adjacentNode = CreateAdjacentNode(currentNode);
            foreach (var item in adjacentNode)
            {
                if (IsValid(item, width, height) && !traversalSet.Contains(item))
                {
                    traversalSet.Add(item);
                    q.Enqueue(item);
                    emptyRoom.Add(item);
                }
            }

            var roomValue = RandomDirectionRoomForce(emptyRoom, minRoom);
            for (int i = 0; i < emptyRoom.Count; i++)
            {
                var value = roomValue[i];
                if (value == 1)
                {
                    minRoom--;
                    maxRoom--;
                }
                maze[(int)emptyRoom[i].x, (int)emptyRoom[i].y] = value;
                if (maxRoom <= 1)
                    break;
            }
            emptyRoom.Clear();
            if (maxRoom <= 1)
                break;
        }
        Vector2 bossRoom = Vector2.zero;
        if (InitializeBoss(maze, width, height, root,ref bossRoom))
            return Tuple.Create(maze, root, bossRoom);
        return InitalizeMaze(width, height, minRoom, maxRoom);
    }
    #endregion

    #region Private Methods
    // Force random room direction by condition
    private static int[] RandomDirectionRoomForce(List<Vector2> room, int minRoom)
    {
        int[] roomValue = new int[room.Count];
        if (minRoom > 1)
        {
            int noOfDirection = Random.Range(1, room.Count + 1);
            for (int i = 0; i < roomValue.Length; i++)
            {
                var rndValue = Random.Range(0, 2);
                roomValue[i] = noOfDirection >= room.Count - i ? 1 : rndValue;
                if (roomValue[i] == 1)
                    noOfDirection--;
            }
        }
        else
        {
            for (int i = 0; i < roomValue.Length; i++)
            {
                roomValue[i] = Random.Range(0, 2);
            }
        }
        return roomValue;
    }

    private static Vector2[] CreateAdjacentNode(Vector2 currentNode)
    {
        Vector2 west = currentNode - Vector2.right;
        Vector2 north = currentNode - Vector2.up;
        Vector2 east = currentNode + Vector2.right;
        Vector2 south = currentNode + Vector2.up;
        return new Vector2[] { west, north, east, south };
    }

    private static bool IsValid(Vector2 pos, int width, int height)
    {
        return pos.x >= 0 && pos.x < height && pos.y >= 0 && pos.y < width;
    }

    private static bool InitializeBoss(int[,] a, int width, int height, Vector2 root,ref Vector2 bossRoom)
    {
        var q = new Queue<Vector2>();
        var acceptableRoom = new List<Vector2>();
        var traversalSet = new HashSet<Vector2>();
        Vector2 currentNode;
        q.Enqueue(root);
        traversalSet.Add(root);
        while (q.Count > 0)
        {
            currentNode = q.Dequeue();
            var adjacentNode = CreateAdjacentNode(currentNode);
            int adjacent = 0;
            foreach (var item in adjacentNode)
            {
                if (IsValid(item, width, height) && a[(int)item.x, (int)item.y] == 1)
                {
                    if (!traversalSet.Contains(item))
                    {
                        q.Enqueue(item);
                        traversalSet.Add(item);
                    }
                    adjacent++;
                }
            }
            if (adjacent == 1 && currentNode != root)
            {
                acceptableRoom.Add(currentNode);
            }
        }
        if (acceptableRoom.Count == 0)
            return false;
        bossRoom = acceptableRoom[Random.Range(0, acceptableRoom.Count)];
        return true;
    }
    #endregion
}
