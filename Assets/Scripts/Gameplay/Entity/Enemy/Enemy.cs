using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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

    #region MonoBehaviour Methods
    protected override void Awake() { }
    protected override void Start()
    {
        base.Start();
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

}
