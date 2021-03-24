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
        ApplyForceBullet(transform.up);
        ApplyForceBullet(transform.up * -1);
        ApplyForceBullet(transform.right);
        ApplyForceBullet(transform.right * -1);
        ApplyForceBullet((transform.up + transform.right).normalized);
        ApplyForceBullet((-transform.up + transform.right).normalized);
        ApplyForceBullet((-transform.up + -transform.right).normalized);
        ApplyForceBullet((transform.up + -transform.right).normalized);
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
