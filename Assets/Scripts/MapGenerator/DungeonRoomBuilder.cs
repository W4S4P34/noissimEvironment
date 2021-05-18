using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomBuilder : MonoBehaviour
{
    private uint state;

    // List of rooms
    [SerializeField] private GameObject[] rooms;

    private Vector3 position;

    private bool isRootRoom;
    private bool isBossRoom;

    #region Properties

    public Vector3 Position
    {
        get { return position; }
    }

    public bool CheckRootRoom
    {
        get { return isRootRoom; }
    }

    public bool CheckBossRoom
    {
        get { return isBossRoom; }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rooms[state].SetActive(true);
    }

    public void updateRoomInformation(uint state, bool isRoot, bool isBoss, Vector3 position)
    {
        this.state = state;
        this.isRootRoom = isRoot;
        this.isBossRoom = isBoss;
        this.position = position;
    }
}
