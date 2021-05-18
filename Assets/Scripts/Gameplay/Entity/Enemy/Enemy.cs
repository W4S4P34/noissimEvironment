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
    protected float spawnLifeTime = 2f;
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
        if(enemySpawnTmp.animator != null)
            animator.enabled = false;
        enemySpawnTmp.enabled = false;
        enemySpawnTmp.transform.DOMoveY(spawnPosition.y + 1f, spawnLifeTime);
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(spawnLifeTime, () => {
            Destroy(spawnSignTmp);
            enemySpawnTmp.enabled = true;
            if (enemySpawnTmp.animator != null)
                animator.enabled = true;
        });
        return enemySpawnTmp;
    }
    #endregion

    protected override void OnDied()
    {
        EnemyDie?.Invoke(this, EventArgs.Empty);
        base.OnDied();
    }

}
