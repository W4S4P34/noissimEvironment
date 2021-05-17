using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomBuilder : MonoBehaviour
{
    private uint state;

    // List of rooms
    [SerializeField] private GameObject[] rooms;

    private bool isRootRoom;
    private bool isBossRoom;

    #region Properties

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

    public void updateRoomInformation(uint state, bool isRoot, bool isBoss)
    {
        this.state = state;
        this.isRootRoom = isRoot;
        this.isBossRoom = isBoss;
    }
}
