using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomCombatManager : MonoBehaviour
{
    private enum State
    {
        Idle, Active
    }

    private State state;

    private GameObject dungeonManager;
    private Level levelInformation;

    [SerializeField] private GameObject room;
    private DungeonRoomBuilder roomBuilder;

    [SerializeField] private GameObject doors;
    [SerializeField] private GameObject entrance;

    // Activate combat
    [SerializeField] private DungeonRoomDoorActivators activator;

    private GameObject boss;

    private GameObject[] enemyTypes;
    private GameObject[] enemies;
    private Enemy[] enemyEvents;

    private const int _MIN_ENEMY_NUMBER_ = 3;
    private const int _MAX_ENEMY_NUMBER_ = 6;

    private int _NUMBER_ENEMIES_;

    [SerializeField] private float scouterColliderRadius;
    // [SerializeField] private LayerMask scouterLayerMask;

    private const int _ALIGN_FACTOR_ = 15;
    // private const int _ROW_ = 15;
    // private const int _COLUMN_ = 15;

    private void Awake()
    {
        state = State.Idle;

        dungeonManager = GameObject.Find("DungeonManager");

        DungeonMapRandomBuilder mapBuilder =
            dungeonManager.GetComponent<DungeonMapRandomBuilder>();
        levelInformation = mapBuilder.LevelInformation;

        enemyTypes = levelInformation.listEnemies.ToArray();
        enemies =
            new GameObject[Random.Range(_MIN_ENEMY_NUMBER_, _MAX_ENEMY_NUMBER_ + 1)];
        enemyEvents = new Enemy[enemies.Length];

        for (int i = 0, size = enemies.Length; i < size; i++)
        {
            enemies[i] = enemyTypes[Random.Range(0, enemyTypes.Length)];
        }

        boss = levelInformation.boss;

        roomBuilder = room.GetComponent<DungeonRoomBuilder>();
    }

    void Start()
    {
        _NUMBER_ENEMIES_ = enemies.Length;
        _NUMBER_ENEMIES_ += roomBuilder.CheckBossRoom == true ? 1 : 0;

        /* Subscribe event handler */
        activator.OnPlayerEnterTrigger += Activator_OnPlayerEnterTrigger;
    }

    private void Activator_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        if (state == State.Idle)
        {
            StartBattle();
            /* Unsubscribe event handler */
            activator.OnPlayerEnterTrigger -= Activator_OnPlayerEnterTrigger;
        }
    }

    private void DungeonRoomCombatManager_EnemyDie(object sender, System.EventArgs e)
    {
        _NUMBER_ENEMIES_--;

        Garbageinator boss = ((Enemy)sender).gameObject.GetComponent<Garbageinator>();
        if (boss != null)
        {
            _NUMBER_ENEMIES_ = 0;

            for (int i = 0, enemyListSize = enemies.Length; i < enemyListSize; i++)
            {
                if (enemies[i] == null) continue;
                enemies[i].gameObject.SetActive(false);
                enemyEvents[i].EnemyDie -= DungeonRoomCombatManager_EnemyDie;
            }
        }

        if (_NUMBER_ENEMIES_ == 0)
        {
            EndBattle();
        }

        ((Enemy)sender).EnemyDie -= DungeonRoomCombatManager_EnemyDie;
    }

    private void StartBattle()
    {
        state = State.Active;

        if (roomBuilder.CheckBossRoom == true)
        {
            boss = Instantiate(boss, room.transform.position, Quaternion.identity);
            Enemy bossEvent = boss.gameObject.GetComponent<Enemy>();

            bossEvent.EnemyDie += DungeonRoomCombatManager_EnemyDie;
        }

        int toleranceCoefficient = _ALIGN_FACTOR_ / 2;
        for (int i = 0, size = enemies.Length; i < size; i++)
        {
            Vector3 spawnPoint;
            Collider2D obstacle = null;
            do
            {
                spawnPoint = calcSpawnPosition(toleranceCoefficient);
                obstacle = 
                    Physics2D.OverlapCircle(
                        spawnPoint, scouterColliderRadius
                    );
            } while (obstacle != null);

            enemyEvents[i] = enemies[i].gameObject.GetComponent<Enemy>();
            enemyEvents[i] = enemyEvents[i].Instantiate(spawnPoint);
            enemies[i] = enemyEvents[i].gameObject;
            enemyEvents[i].EnemyDie += DungeonRoomCombatManager_EnemyDie;
        }
    }

    private Vector3 calcSpawnPosition(int tolerance)
    {
        float row = Random.Range(-tolerance, tolerance + 1);
        float column = Random.Range(-tolerance, tolerance + 1);

        row = row < 0 ? row -= 0.5f : row += 0.5f;
        column = column < 0 ? column -= 0.5f : column += 0.5f;

        Vector3 spawnPosition = new Vector3(row, column);
        spawnPosition += room.transform.position;

        return spawnPosition;
    }

    private void EndBattle()
    {
        doors.gameObject.SetActive(false);
    }
}
