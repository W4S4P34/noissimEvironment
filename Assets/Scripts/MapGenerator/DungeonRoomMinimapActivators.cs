using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomMinimapActivators : MonoBehaviour
{
    [SerializeField] private GameObject room;
    private DungeonRoomBuilder roomBuilder;

    // Event
    [SerializeField] private GameEvent OnUpdateMinimap;

    private const int _ROOM_MINIMAP_WIDTH_ = 1; // == _ROOM_MINIMAP_HEIGHT_
    // private const int _ROOM_MINIMAP_HEIGHT_ = 1;

    private void Awake()
    {
        roomBuilder = room.GetComponent<DungeonRoomBuilder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        GameObject[,] minimapRealization = DungeonMinimapBuilder.GetMapRealization();
        int x = (int)roomBuilder.Position.x, y = (int)roomBuilder.Position.y;

        DungeonMinimapRoomBuilder minimapRoomBuilder =
            minimapRealization[x, y].gameObject.GetComponent<DungeonMinimapRoomBuilder>();
        minimapRoomBuilder.IsVisited = true;
        minimapRoomBuilder.IsCurrentlyVisited = true;

        for (int i = 0, neighbours = 4; i < neighbours; i++)
        {
            if (minimapRoomBuilder.Neighbours[i] == null)
            {
                continue;
            }

            DungeonMinimapRoomBuilder neighbourMinimapRoomBuilder =
                minimapRoomBuilder.Neighbours[i].GetComponent<DungeonMinimapRoomBuilder>();
            neighbourMinimapRoomBuilder.IsCurrentlyVisited = false;
        }

        DungeonMinimapBuilder.Camera.transform.position =
            minimapRealization[x, y].gameObject.transform.position + new Vector3(0, 0, -10);

        OnUpdateMinimap.Raise();
    }
}
