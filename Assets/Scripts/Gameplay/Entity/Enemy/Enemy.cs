using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using System;

public enum AttackType
{
    Melee,
    LongRange
}

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public abstract class Enemy : Entity
{
    [SerializeField]
    protected GameObject spawnSign;
    [SerializeField]
    protected float spawnLifeTime = 1f;
    protected AIPath aiPath = null;
    protected AIDestinationSetter aiDestinationSetter = null;
    protected GameObject player = null;

    public event EventHandler EnemyDie;

    #region MonoBehaviour Methods
    protected override void Awake() { }
    protected override void Start()
    {
        base.Start();
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player");
        aiDestinationSetter.target = player.transform;
    }

    public virtual Enemy Instantiate(Vector3 spawnPosition)
    {
        if (spawnSign == null)
            return Instantiate(gameObject, spawnPosition, Quaternion.identity)?.GetComponent<Enemy>();
        var spawnSignTmp = Instantiate(spawnSign, spawnPosition, spawnSign.transform.rotation);
        var enemySpawnTmp = Instantiate(gameObject, spawnPosition, Quaternion.identity)?.GetComponent<Enemy>();
        enemySpawnTmp.GetComponent<Collider2D>().enabled = false;
        if (enemySpawnTmp.animator != null)
            enemySpawnTmp.animator.enabled = false;
        enemySpawnTmp.enabled = false;
        enemySpawnTmp.transform.DOMoveY(spawnPosition.y + 1f, spawnLifeTime);
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(spawnLifeTime, () => {
            Destroy(spawnSignTmp);
            enemySpawnTmp.enabled = true;
            if (enemySpawnTmp.animator != null)
                enemySpawnTmp.animator.enabled = true;
            enemySpawnTmp.GetComponent<Collider2D>().enabled = true;
        });
        return enemySpawnTmp;
    }
    #endregion

    protected override void OnDied()
    {
        base.OnDied();
        GetComponent<Collider2D>().enabled = false;
        EnemyDie?.Invoke(this, EventArgs.Empty);
    }

}
