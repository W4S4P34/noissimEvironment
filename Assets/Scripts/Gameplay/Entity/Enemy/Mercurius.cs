using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercurius : ChasingMinion
{
    #region Private Fields
    [Header("UNIQUE FIELDS")]
    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private LayerMask targetLayerMask;
    #endregion

    #region Monobehaviour Methods
    protected override void Start()
    {
        base.Start();
        ObjectPool.RegisterObjectPoolItem(pfBullet.GetBulletCode(), pfBullet.gameObject, 20);
    }
    #endregion

    #region Protected Methods
    protected override void LongRangeAttack()
    {
        var shootDirection = (aiDestinationSetter.target.position - transform.position).normalized;
        Shoot(shootDirection);
    }
    protected override IEnumerator Attack()
    {
        isOnAction = true;
        attackMethod?.Invoke();
        // Add animation here

        yield return new WaitForSeconds(fireRate);
        isOnAction = false;
        currentState = ChasingState.Chasing;
    }

    protected override IEnumerator Chasing()
    {
        if (Physics2D.OverlapCircle(transform.position, attackRange, targetLayerMask))
        {
            currentState = ChasingState.Attack;
        }
        yield return null;
    }
    #endregion

}
