using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomDoorActivators : MonoBehaviour
{
    [SerializeField] private GameObject doors;

    [SerializeField] private GameObject room;
    private DungeonRoomBuilder roomBuilder;

    // Combat system
    public event EventHandler OnPlayerEnterTrigger;

    private void Awake()
    {
        roomBuilder = room.GetComponent<DungeonRoomBuilder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (roomBuilder.CheckRootRoom == true)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        doors.SetActive(true);
        OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);

        gameObject.SetActive(false);
    }
}
