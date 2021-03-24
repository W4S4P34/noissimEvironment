using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gituku : ImmovableMinion

{
    #region Private Fields
    [Header("UNIQUE FIELDS")]
    [SerializeField, Tooltip("Odd number only")]
    private int bulletPerClip = 5;
    [SerializeField]
    private float bulletAngle = 15f;
    #endregion

    #region Monobehaviour Methods
    protected override void Start()
    {
        base.Start();
        ObjectPool.RegisterObjectPoolItem(pfBullet.GetBulletCode(), pfBullet.gameObject, 40);
    }
    #endregion

    #region Private Methods
    private Vector3 CalculateDirectionFromAngle(Vector3 positionA, Vector3 positionB, float angle) {
        angle += 180;
        var direction = (positionB - positionA).normalized;
        var angleAxisX = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newPosition = new Vector3(positionA.x + Mathf.Cos((angleAxisX-angle) * Mathf.Deg2Rad), positionA.y + Mathf.Sin((angleAxisX - angle) * Mathf.Deg2Rad));
        direction = (positionA - newPosition).normalized;
        return direction;
    }
    #endregion

    #region Protected Methods
    protected void LongRangeAttack()
    {
        var targetPosition = aiDestinationSetter.target.position;
        var shootDirection = (targetPosition - transform.position).normalized;
        ApplyForceBullet(shootDirection);
        for (int i = 1; i <= bulletPerClip/2; i++)
        {
            shootDirection = CalculateDirectionFromAngle(transform.position, targetPosition, i * bulletAngle);
            ApplyForceBullet(shootDirection);
            shootDirection = CalculateDirectionFromAngle(transform.position, targetPosition, -i * bulletAngle);
            ApplyForceBullet(shootDirection);
        }
    }
    protected override IEnumerator Attack()
    {
        isOnAction = true;
        yield return new WaitForSeconds(1f);
        LongRangeAttack();
        // Add animation here

        yield return new WaitForSeconds(fireRate);
        isOnAction = false;
        currentState = ImmovableState.Guarding;
    }

    protected override IEnumerator Guarding()
    {
        if (Physics2D.OverlapCircle(transform.position, attackRange, targetLayerMask))
        {
            currentState = ImmovableState.Attack;
        }
        yield return null;
    }
    #endregion
}
