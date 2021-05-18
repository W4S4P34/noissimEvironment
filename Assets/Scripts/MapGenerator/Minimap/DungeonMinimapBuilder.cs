using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMinimapBuilder : MonoBehaviour
{
    [SerializeField] private GameObject dungeonManager;
    private DungeonMapRandomBuilder mapBuilder;

    [SerializeField] private GameObject minimap;
    [SerializeField] private GameObject minimapRoom;

    private static GameObject minimapCamera;

    private int[,] mapMatrix;
    private static GameObject[,] minimapRealization;
    private Vector3 rootPosition;
    private Vector3 bossPosition;

    private const int _ROOM_MINIMAP_WIDTH_ = 1; // == _ROOM_MINIMAP_HEIGHT_
    // private const int _ROOM_MINIMAP_HEIGHT_ = 1;

    // Start is called before the first frame update
    void Start()
    {
        mapBuilder = dungeonManager.GetComponent<DungeonMapRandomBuilder>();

        mapMatrix = mapBuilder.RandomMap;
        rootPosition = mapBuilder.RootPosition;
        bossPosition = mapBuilder.BossPosition;

        minimapCamera = GameObject.Find("Minimap Camera");

        minimapRealization =
            new GameObject[mapMatrix.GetLength(0), mapMatrix.GetLength(1)];

        instantiateMinimapWithBFS();

        for (int i = 0; i < minimapRealization.GetLength(0); i++)
        {
            for (int j = 0; j < minimapRealization.GetLength(1); j++)
            {
                if (minimapRealization[i, j] == null) continue;

                DungeonMinimapRoomBuilder minimapRoomBuilder =
                    minimapRealization[i, j].GetComponent<DungeonMinimapRoomBuilder>();
                minimapRoomBuilder.UpdateNeighbours();
            }
        }

        for (int i = 0; i < minimapRealization.GetLength(0); i++)
        {
            for (int j = 0; j < minimapRealization.GetLength(1); j++)
            {
                if (minimapRealization[i, j] == null) continue;

                DungeonMinimapRoomBuilder minimapRoomBuilder =
                    minimapRealization[i, j].GetComponent<DungeonMinimapRoomBuilder>();
                minimapRoomBuilder.UpdateMinimapRoomAppearance();
            }
        }
    }

    private void instantiateMinimapWithBFS()
    {
        Queue<Vector3> incomingDiscover = new Queue<Vector3>();
        HashSet<Vector3> visited = new HashSet<Vector3>();

        incomingDiscover.Enqueue(rootPosition);

        while (incomingDiscover.Count != 0)
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
                            if (mapMatrix[x - 1, y] == 1)
                            {
                                currentRoomState[0] = true;
                                addNeighbour(x - 1, y, in visited, incomingDiscover);
                            }
                            break;
                        // Right
                        case 1:
                            if (mapMatrix[x, y + 1] == 1)
                            {
                                currentRoomState[1] = true;
                                addNeighbour(x, y + 1, in visited, incomingDiscover);
                            }
                            break;
                        // Bottom
                        case 2:
                            if (mapMatrix[x + 1, y] == 1)
                            {
                                currentRoomState[2] = true;
                                addNeighbour(x + 1, y, in visited, incomingDiscover);
                            }
                            break;
                        // Left
                        case 3:
                            if (mapMatrix[x, y - 1] == 1)
                            {
                                currentRoomState[3] = true;
                                addNeighbour(x, y - 1, in visited, incomingDiscover);
                            }
                            break;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }

            // Instantiate
            instantiateMinimapRoom(current, currentRoomState);

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

    private void instantiateMinimapRoom(Vector3 position, bool[] roomState)
    {
        bool isRoot = false, isBoss = false;
        if (position == rootPosition) isRoot = true;
        if (position == bossPosition) isBoss = true;

        // Convert coordination system from array -> Unity coordination system
        Vector3 actualPosition =
            new Vector3(position.y * _ROOM_MINIMAP_WIDTH_, position.x * -_ROOM_MINIMAP_WIDTH_);

        actualPosition += minimap.transform.position;

        if (isRoot)
            minimapCamera.transform.position =
                actualPosition + new Vector3(0, 0, -10);

        // Room created
        GameObject room = Instantiate(minimapRoom, actualPosition, Quaternion.identity);
        room.transform.parent = minimap.transform;

        minimapRealization[(int)position.x, (int)position.y] = room;

        // Transfer data to the room itself
        DungeonMinimapRoomBuilder minimapRoomBuilder =
            room.GetComponent<DungeonMinimapRoomBuilder>();
        minimapRoomBuilder.InitRoomState(position, roomState, isRoot, isBoss);
    }

    public static GameObject[,] GetMapRealization()
    {
        return minimapRealization;
    }

    public static GameObject Camera
    {
        get { return minimapCamera; }
    }
}
