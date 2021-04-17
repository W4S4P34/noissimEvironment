using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Garbageinator : Enemy
{
    #region Fields

    #region SerializeFields
    [Header("Stats")]
    [SerializeField, Range(0f,2f)]
    private List<float> phaseFireRate = null;
    [SerializeField]
    private List<Color> healthPhaseColor = null;

    

    [Header("Phase 1")]
    [SerializeField]
    private Bullet pfSimpleBullet = null;
    [Header("Spiral Attack")]
    [SerializeField, Range(5, 13), Tooltip("Odd number only")]
    private int bulletPerClip = 5;
    [SerializeField]
    private float bulletAngle = 15f;


    
    [Header("Phase 2")]

    [SerializeField]
    private List<Enemy> listMinionSpawn = null;
    [Header("Bomb"), SerializeField]
    private Bomb pfBomb = null;
    [SerializeField]
    private bool bombSign;
    [SerializeField, DrawIf("bombSign", true)]
    private float spawnSignLiveTime = 1f;

    private const ObjectPoolCode spawnPositionSignCode = ObjectPoolCode.SpawnPositionSign_1;
    private const int spawnPositionSignIndex = 0;

    [SerializeField]
    private float minSpawnDistance = 5f;


    
    [Header("Phase 3")]
    [SerializeField]
    private GuidedBullet pfGuidedBullet = null;
    #endregion

    private const int numOfPhase = 3;
    private Action phaseAttack = null;
    private bool isOnAction = false;
    private int nextAttackPhase = 0;
    private List<List<Action>> listPhaseAttack = new List<List<Action>>();
    private LinkedList<Vector3> destructionSpawnPosition = new LinkedList<Vector3>();

    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        listPhaseAttack.Add(new List<Action>() { DiagonalAttack, CircleWaveAttack });
        listPhaseAttack.Add(new List<Action>() { DestructiveAttack, SpawnMinions });
        listPhaseAttack.Add(new List<Action>() {  });
        ObjectPool.RegisterObjectPoolItem(pfSimpleBullet.GetBulletCode(), pfSimpleBullet.gameObject, 50);
        ObjectPool.RegisterObjectPoolItem(pfBomb.GetBulletCode(), pfBomb.gameObject, 10);
        ObjectPool.RegisterObjectPoolItem(spawnPositionSignCode, GameAssets.i.pfSpawnPositionSign[spawnPositionSignIndex], 20);

        //ObjectPool.RegisterObjectPoolItem(pfGuidedBullet.GetBulletCode(), pfBomb.gameObject, 10);
        phaseAttack = Phase_2;
    }

    private void Update()
    {
        if(!isOnAction)
            phaseAttack.Invoke();
    }
    #endregion

    #region Private Methods
    private void Phase_1()
    {
        var _randomAttack = listPhaseAttack[0][Random.Range(0, listPhaseAttack[0].Count)];
        _randomAttack.Invoke();
    }
    private void Phase_2()
    {
        var _randomAttack = listPhaseAttack[nextAttackPhase][Random.Range(0, listPhaseAttack[nextAttackPhase].Count)];
        nextAttackPhase = Random.Range(0, numOfPhase - 1);
        _randomAttack.Invoke();
        
    }
    private void Phase_3()
    {
        nextAttackPhase = Random.Range(0, numOfPhase - 1);
    }
    private Vector3 CalculateDirectionFromAngle(Vector3 positionA, Vector3 positionB, float angle)
    {
        angle += 180;
        var direction = (positionB - positionA).normalized;
        var angleAxisX = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newPosition = new Vector3(positionA.x + Mathf.Cos((angleAxisX - angle) * Mathf.Deg2Rad), positionA.y + Mathf.Sin((angleAxisX - angle) * Mathf.Deg2Rad));
        return (positionA - newPosition).normalized;
    }
    private void Shoot(Vector3 shootDirection, Bullet pfBullet)
    {
        var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
        _bullet.SetActive(true);
        _bullet.transform.position = transform.position + shootDirection;
        _bullet.GetComponent<Bullet>().Setup(shootDirection);
    }





    /// <summary>
    /// Attack diagonal shape, boss call randomly in phase 1.
    /// </summary>

    private void DiagonalAttack()
    {
        isOnAction = true;
        TimeManipulator.GetInstance().InvokeActionWithPromise(phaseFireRate[0], () => {
            Shoot((Vector2.right + Vector2.up).normalized, pfSimpleBullet);
            Shoot((Vector2.right - Vector2.up).normalized, pfSimpleBullet);
            Shoot((-Vector2.right + Vector2.up).normalized, pfSimpleBullet);
            Shoot((-Vector2.right - Vector2.up).normalized, pfSimpleBullet);
        }, () => isOnAction = false);
    }






    /// <summary>
    /// Attack spiral shape, boss call randomly in phase 1.
    /// </summary>

    private void SpiralAttack()
    {
        
    }





    /// <summary>
    /// Attack circle shape, boss call randomly in phase 1.
    /// </summary>

    private void CircleWaveAttack()
    {
        isOnAction = true;
        TimeManipulator.GetInstance().InvokeActionWithPromise(phaseFireRate[0], () => {
            var targetPosition = aiDestinationSetter.target.position;
            var shootDirection = (targetPosition - transform.position).normalized;
            Shoot(shootDirection, pfSimpleBullet);
            for (int i = 1; i <= bulletPerClip / 2; i++)
            {
                shootDirection = CalculateDirectionFromAngle(transform.position, targetPosition, i * bulletAngle);
                Shoot(shootDirection, pfSimpleBullet);
                shootDirection = CalculateDirectionFromAngle(transform.position, targetPosition, -i * bulletAngle);
                Shoot(shootDirection, pfSimpleBullet);
            }
        }, () => isOnAction = false);
    }





    /// <summary>
    /// Attack with laser, boss call randomly in phase 1.
    /// </summary>
    
    private void LaserBeamWipeAttack()
    {
        
    }





    /// <summary>
    /// Attack with exploded objecy, boss call randomly in phase 2.
    /// </summary>
    
    private void DestructiveAttack()
    {
        isOnAction = true;
        TimeManipulator.GetInstance().InvokeActionWithPromise(phaseFireRate[nextAttackPhase] + spawnSignLiveTime, () =>
        {
            // Random number of bomb
            int bombCount = Random.Range(1, 6);
            var spawnPositions = GenerateSpawnPosition(bombCount, minSpawnDistance);
            foreach (var item in spawnPositions)
            {
                var spawnPositionSign = ObjectPool.GetObject(spawnPositionSignCode);
                spawnPositionSign.transform.position = item;
                spawnPositionSign.SetActive(true);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(spawnSignLiveTime, () => {
                    spawnPositionSign.SetActive(false);
                    ObjectPool.ReturnObject(spawnPositionSignCode, spawnPositionSign);
                    Bomb destruction = ObjectPool.GetObject(pfBomb.GetBulletCode())?.GetComponent<Bomb>();
                    destruction.gameObject.SetActive(true);
                    destruction.transform.position = item;
                    destruction.Trigger();
                });
            }
            
        }, () => isOnAction = false);
    }





    /// <summary>
    /// Spawn minions method, boss call randomly in phase 2.
    /// </summary>
    
    private void SpawnMinions()
    {
        isOnAction = true;
        TimeManipulator.GetInstance().InvokeActionWithPromise(phaseFireRate[nextAttackPhase] + spawnSignLiveTime, () =>
        {
            // Random number of bomb
            int minionCount = Random.Range(1, 6);
            var spawnPositions = GenerateSpawnPosition(minionCount, minSpawnDistance);
            foreach (var item in spawnPositions)
            {
                var spawnPositionSign = ObjectPool.GetObject(spawnPositionSignCode);
                spawnPositionSign.transform.position = item;
                spawnPositionSign.SetActive(true);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(spawnSignLiveTime, () => {
                    spawnPositionSign.SetActive(false);
                    Instantiate(listMinionSpawn[Random.Range(0, listMinionSpawn.Count)], item, Quaternion.identity);
                });
            }

        }, () => isOnAction = false);
    }





    /// <summary>
    /// Random spawn position method within camera view
    /// </summary>
    /// <param name="size"> Size of spawned objects </param>
    /// <param name="minBoundDistance"> Min distance between spawned objects </param>
    /// <returns> Array of spawned position </returns>
    
    private Vector3[] GenerateSpawnPosition(int size, float minBoundDistance)
    {
        Vector3[] spawnPositions = new Vector3[size];
        bool acceptable;
        while (size > 0)
        {
            var position = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
            position.z = 0;
            acceptable = true;
            foreach (var item in destructionSpawnPosition)
            {
                if (Vector2.Distance(item, position) < minBoundDistance)
                {
                    acceptable = false;
                    break;
                }
            }
            if (acceptable)
            {
                size--;
                spawnPositions[size] = position;
                destructionSpawnPosition.AddLast(position);
            }
        }
        destructionSpawnPosition.Clear();
        return spawnPositions;
    }
    #endregion

}
