using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    #endregion

    protected override void OnDied()
    {
        EnemyDie?.Invoke(this, EventArgs.Empty);
        base.OnDied();
    }

}
