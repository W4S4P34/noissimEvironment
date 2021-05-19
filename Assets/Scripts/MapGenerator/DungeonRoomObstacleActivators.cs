using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomObstacleActivators : MonoBehaviour
{
    // List of obstacles
    [SerializeField] private GameObject[] obstacles;

    [SerializeField] private GameObject room;
    private DungeonRoomBuilder roomBuilder;

    private void Awake()
    {
        roomBuilder = room.GetComponent<DungeonRoomBuilder>();
    }

    private void Start()
    {
        if (roomBuilder.CheckRootRoom == true)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (roomBuilder.CheckBossRoom == true) return;
        
        int randomFactor = Random.Range(0, obstacles.Length);
        obstacles[randomFactor].SetActive(true);
        AstarPath.active.Scan(AstarPath.active.data.gridGraph);


        gameObject.SetActive(false);
    }
}
