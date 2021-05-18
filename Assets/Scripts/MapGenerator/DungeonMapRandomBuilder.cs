using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapRandomBuilder : MonoBehaviour
{
    // Map containers
    private int[,] randomMap;
    private Vector3 rootPosition;
    private Vector3 bossPosition;

    // Room's size
    [SerializeField] private GameObject room;

    private const int _ROOM_WIDTH_ = 34; // == _ROOM_HEIGHT_
    // private const int _ROOM_HEIGHT_ = 16;

    // Scriptable Object Level
    [SerializeField] private Level levelInformation;

    // Map's size
    private const int _MAP_WIDTH_ = 4;
    private const int _MAP_HEIGHT_ = 4;
    private int _MIN_ROOM_ = 0;
    private int _MAX_ROOM_ = 0;

    // Room prefab
    [SerializeField] private GameObject roomPrefab;

    #region Properties

    public Level LevelInformation
    {
        get { return levelInformation; }
    }

    public int[,] RandomMap
    {
        get { return randomMap; }
    }

    public Vector3 RootPosition
    {
        get { return rootPosition; }
    }

    public Vector3 BossPosition
    {
        get { return bossPosition; }
    }

    #endregion

    /**
     * Player Initialization
     */
    [SerializeField] private GameObject player;

    private void Awake()
    {
        _MIN_ROOM_ = levelInformation.minRoom;
        _MAX_ROOM_ = levelInformation.maxRoom;

        // Init matrix
        Tuple<int[,], Vector2, Vector2> container =
            DungeonMapRandomGenerator.InitalizeMaze(
                _MAP_WIDTH_, _MAP_HEIGHT_, _MIN_ROOM_, _MAX_ROOM_);

        // Setup matrix -> containers
        randomMap = container.Item1;
        rootPosition = container.Item2;
        bossPosition = container.Item3;
    }

    // Start is called before the first frame update
    void Start()
    {
        /**
         * Player Initialization
         */

        // Convert coordination system from array -> Unity coordination system
        Vector3 playerPosition =
            new Vector3(rootPosition.y * _ROOM_WIDTH_, rootPosition.x * -_ROOM_WIDTH_);

        player.transform.position = playerPosition;

        instantiateMapWithBFS();
        // StartCoroutine(instantiateMapWithBFS());
    }

    private void instantiateMapWithBFS()
    {
        Queue<Vector3> incomingDiscover = new Queue<Vector3>();
        HashSet<Vector3> visited = new HashSet<Vector3>();

        incomingDiscover.Enqueue(rootPosition);

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
        ActionEventHandler.Invoke(GameDungeonEvent.PrepareDugeon);
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

        bool isRoot = false, isBoss = false;
        if (position == rootPosition) isRoot = true;
        if (position == bossPosition) isBoss = true;

        // Convert coordination system from array -> Unity coordination system
        Vector3 actualPosition =
            new Vector3(position.y * _ROOM_WIDTH_, position.x * -_ROOM_WIDTH_);

        actualPosition += this.room.transform.position;

        GameObject room = Instantiate(roomPrefab, actualPosition, Quaternion.identity);
        room.transform.parent = this.room.transform;

        room.GetComponent<DungeonRoomBuilder>()
            .updateRoomInformation(state, isRoot, isBoss, position);
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
