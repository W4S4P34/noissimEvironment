using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nyx : ChasingMinion
{
    #region Private Fields
    [Header("UNIQUE FIELDS")]
    [SerializeField]
    private float fireRate = 2f;
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
        Shoot(transform.up);
        Shoot(transform.up * -1);
        Shoot(transform.right);
        Shoot(transform.right * -1);
        Shoot((transform.up + transform.right).normalized);
        Shoot((-transform.up + transform.right).normalized);
        Shoot((-transform.up + -transform.right).normalized);
        Shoot((transform.up + -transform.right).normalized);
    }
    protected override IEnumerator Attack()
    {
        isOnAction = true;
        attackMethod?.Invoke();
        // Add animation here

        yield return new WaitForSeconds(fireRate);
        isOnAction = false;
    }

    protected override IEnumerator Chasing()
    {
        currentState = ChasingState.Attack;
        yield return null;
    }
    #endregion
}
