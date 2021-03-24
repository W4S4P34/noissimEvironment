using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ChasingState
{
    Chasing,
    Attack
}
public abstract class ChasingMinion : Enemy
{
    #region Protected Fields
    [Header("GENERAL FIELDS")]
    [SerializeField]
    protected float attackRange = 2f;
    [SerializeField]
    protected AttackType attackType;
    [SerializeField, DrawIf("attackType", AttackType.LongRange)]
    protected Bullet pfBullet = null;

    protected Action attackMethod = null;
    protected bool isOnAction = false;
    protected ChasingState currentState = ChasingState.Chasing;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        switch (attackType)
        {
            case AttackType.Melee:
                attackMethod = MeleeAttack;
                break;
            case AttackType.LongRange:
                if (!pfBullet)
                    Debug.LogError("This enemy need bullet to active");
                attackMethod = LongRangeAttack;
                break;
            default:
                break;
        }
    }
    protected void FixedUpdate()
    {
        if (!isOnAction)
        {
            switch (currentState)
            {
                case ChasingState.Chasing:
                    StartCoroutine(Chasing());
                    break;
                case ChasingState.Attack:
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
    protected virtual void MeleeAttack()
    { }

    protected virtual void LongRangeAttack()
    { }

    protected virtual void ApplyForceBullet(Vector3 shootDirection)
    {
        var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
        _bullet.SetActive(true);
        _bullet.transform.position = transform.position + shootDirection;
        _bullet.GetComponent<Bullet>().Setup(shootDirection);
    }
    #endregion

    #region Abstract Methods
    protected abstract IEnumerator Attack();
    protected abstract IEnumerator Chasing();
    #endregion

}
