using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public abstract class Enemy : Entity
{
    protected AIPath aiPath = null;
    protected AIDestinationSetter aiDestinationSetter = null;

    #region MonoBehaviour Methods
    protected override void Start()
    {
        base.Start();
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }
    #endregion

}
