using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonMapRandomBuilder : MonoBehaviour
{
    // Map containers
    private int[,] randomMap;
    private Vector3 playerPosition;
    private Vector3 bossPosition;

    // Room's size
    private const int _ROOM_WIDTH_ = 16; // == _ROOM_HEIGHT_
    // private const int _ROOM_HEIGHT_ = 16;

    // Map's size
    private const int _MAP_WIDTH_ = 4;
    private const int _MAP_HEIGHT_ = 4;
    private const int _MIN_ROOM_ = 8;
    private const int _MAX_ROOM_ = 15;

    // List of room prefabs
    [SerializeField] private GameObject[] rooms;  

    // Start is called before the first frame update
    void Start()
    {
        // Init matrix
        Tuple<int[,], Vector2, Vector2> container =
            DungeonMapRandomGenerator.InitalizeMaze(
                _MAP_WIDTH_, _MAP_HEIGHT_, _MIN_ROOM_, _MAX_ROOM_);

        // Setup matrix -> containers
        randomMap = container.Item1;
        playerPosition = container.Item2;
        bossPosition = container.Item3;

        instantiateMapWithBFS();
        // StartCoroutine(instantiateMapWithBFS());
    }

    private void instantiateMapWithBFS()
    {
        Queue<Vector3> incomingDiscover = new Queue<Vector3>();
        HashSet<Vector3> visited = new HashSet<Vector3>();

        incomingDiscover.Enqueue(playerPosition);

        while(incomingDiscover.Count != 0)
        {
            Vector3 current = incomingDiscover.Dequeue();
            visited.Add(current);

            // Explicit casting type
            int x = (int)current[0], y = (int)current[1];

            // Init current room's state
            bool[] currentRoomState = new bool[4] { false, false, false, false };

            // Check 4-direction neighbours to enqueue, not duplicate
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    switch (i)
                    {
                        // Top
                        case 0:
                            if (randomMap[x - 1, y] == 1)
                            {
                                currentRoomState[0] = true;
                                addNeighbour(x - 1, y, in visited, incomingDiscover);
                            }
                            break;
                        // Right
                        case 1:
                            if (randomMap[x, y + 1] == 1)
                            {
                                currentRoomState[1] = true;
                                addNeighbour(x, y + 1, in visited, incomingDiscover);
                            }
                            break;
                        // Bottom
                        case 2:
                            if (randomMap[x + 1, y] == 1)
                            {
                                currentRoomState[2] = true;
                                addNeighbour(x + 1, y, in visited, incomingDiscover);
                            }
                            break;
                        // Left
                        case 3:
                            if (randomMap[x, y - 1] == 1)
                            {
                                currentRoomState[3] = true;
                                addNeighbour(x, y - 1, in visited, incomingDiscover);
                            }
                            break;
                    }
                } catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }

            // Instantiate
            instantiateRoom(current, currentRoomState);

            // yield return new WaitForSeconds(.5f);
        }
    }

    private void addNeighbour(
        int x, int y,
        in HashSet<Vector3> visited, Queue<Vector3> incomingQueue)
    {
        Vector3 neighbour = new Vector3(x, y);

        // Enqueue
        if (!visited.Contains(neighbour) && !incomingQueue.Contains(neighbour))
        {
            incomingQueue.Enqueue(neighbour);
        }
    }

    private void instantiateRoom(Vector3 position, bool[] roomState)
    {
        uint state = boolArrayToInt(roomState);

        // Convert coordination system from array -> Unity coordination system
        Vector3 actualPosition =
            new Vector3(position.y * _ROOM_WIDTH_, position.x * -_ROOM_WIDTH_);

        switch (state)
        {
            // No Entry
            case 0:
                Instantiate(rooms[0], actualPosition, Quaternion.identity);
                break;
            // L
            case 1:
                Instantiate(rooms[1], actualPosition, Quaternion.identity);
                break;
            // B
            case 2:
                Instantiate(rooms[2], actualPosition, Quaternion.identity);
                break;
            // BL
            case 3:
                Instantiate(rooms[3], actualPosition, Quaternion.identity);
                break;
            // R
            case 4:
                Instantiate(rooms[4], actualPosition, Quaternion.identity);
                break;
            // RL
            case 5:
                Instantiate(rooms[5], actualPosition, Quaternion.identity);
                break;
            // RB
            case 6:
                Instantiate(rooms[6], actualPosition, Quaternion.identity);
                break;
            // RBL
            case 7:
                Instantiate(rooms[7], actualPosition, Quaternion.identity);
                break;
            // T
            case 8:
                Instantiate(rooms[8], actualPosition, Quaternion.identity);
                break;
            // TL
            case 9:
                Instantiate(rooms[9], actualPosition, Quaternion.identity);
                break;
            // TB
            case 10:
                Instantiate(rooms[10], actualPosition, Quaternion.identity);
                break;
            // TBL
            case 11:
                Instantiate(rooms[11], actualPosition, Quaternion.identity);
                break;
            // TR
            case 12:
                Instantiate(rooms[12], actualPosition, Quaternion.identity);
                break;
            // TRL
            case 13:
                Instantiate(rooms[13], actualPosition, Quaternion.identity);
                break;
            // TRB
            case 14:
                Instantiate(rooms[14], actualPosition, Quaternion.identity);
                break;
            // TRBL
            case 15:
                Instantiate(rooms[15], actualPosition, Quaternion.identity);
                break;
        }
    }

    private uint boolArrayToInt(bool[] boolArray)
    {
        uint number = 0;

        for (int i = 0; i < boolArray.Length; i++)
        {
            uint one = 1;

            if (boolArray[i])
                number |= one << (boolArray.Length - (i + 1));
        }

        return number;
    }
}
