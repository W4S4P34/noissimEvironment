using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    /*
     * 1 --> Need Bottom door
     * 2 --> Need Left door
     * 3 --> Need Top door
     * 4 --> Need Right door
     */

    private RoomTemplates roomTemplates;
    private int randomSeed;

    private bool created;

    private void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.15f);
    }

    private void SpawnRoom()
    {
        if (!created)
        {
            switch (openingDirection)
            {
                case 1:
                    randomSeed = Random.Range(0, roomTemplates.bottomRoom.Length);
                    Instantiate(roomTemplates.bottomRoom[randomSeed],
                        transform.position,
                        Quaternion.identity);
                    break;
                case 2:
                    randomSeed = Random.Range(0, roomTemplates.leftRoom.Length);
                    Instantiate(roomTemplates.leftRoom[randomSeed],
                        transform.position,
                        Quaternion.identity);
                    break;
                case 3:
                    randomSeed = Random.Range(0, roomTemplates.topRoom.Length);
                    Instantiate(roomTemplates.topRoom[randomSeed],
                        transform.position,
                        Quaternion.identity);
                    break;
                case 4:
                    randomSeed = Random.Range(0, roomTemplates.rightRoom.Length);
                    Instantiate(roomTemplates.rightRoom[randomSeed],
                        transform.position,
                        Quaternion.identity);
                    break;
            }

            created = true;
        }
    }
}
