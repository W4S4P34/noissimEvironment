using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImmovableState
{
    Attack,
    Guarding
}

public abstract class ImmovableMinion : Enemy
{
    #region Protected Fields
    [Header("GENERAL FIELDS")]
    [SerializeField, Range(1f,10f)]
    protected float attackRange = 2f;
    [SerializeField]
    protected float fireRate = 1f;
    [SerializeField]
    protected LayerMask targetLayerMask;
    [SerializeField]
    protected Bullet pfBullet = null;

    protected bool isOnAction = false;
    protected ImmovableState currentState = ImmovableState.Guarding;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        aiPath.canMove = false;
        if (!pfBullet)
            Debug.LogError("This enemy need bullet to active");
    }
    protected void FixedUpdate()
    {
        if (!isOnAction)
        {
            switch (currentState)
            {
                case ImmovableState.Guarding:
                    StartCoroutine(Guarding());
                    break;
                case ImmovableState.Attack:
                    StartCoroutine(Attack());
                    break;
                default:
                    break;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion

    #region Protected Methods
    protected virtual void Shoot(Vector3 shootDirection)
    {
        var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
        _bullet.SetActive(true);
        _bullet.transform.position = transform.position + shootDirection;
        _bullet.GetComponent<Bullet>().Setup(shootDirection);
    }
    #endregion

    #region Abstract Methods
    protected abstract IEnumerator Attack();
    protected abstract IEnumerator Guarding();
    #endregion
}
