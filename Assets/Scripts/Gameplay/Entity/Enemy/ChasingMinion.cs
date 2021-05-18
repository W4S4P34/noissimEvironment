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
    protected SpriteRenderer spriteRenderer;
    [SerializeField, Range(1f, 10f)]
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
        if (isDeath)
            return;
        var direction = (aiDestinationSetter.target.position - transform.position).normalized;
        if(Mathf.Abs(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) > 90f)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

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

    protected virtual void Shoot(Vector3 shootDirection)
    {
        var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
        _bullet.SetActive(true);
        _bullet.transform.position = transform.position + shootDirection;
        _bullet.GetComponent<Bullet>().Setup(shootDirection);
    }

    public override void OnTakeDamage(IEntityDamageEvent e)
    {
        base.OnTakeDamage(e);
        if (isDeath)
            return;
        // Add animation hit here
        animator?.SetTrigger("onHit");
    }

    protected override void OnDied()
    {
        base.OnDied();
        aiPath.canMove = false;
        isDeath = true;
        // Add animation death here
        animator?.SetTrigger("onDie");
    }
    #endregion

    #region Abstract Methods
    protected abstract IEnumerator Attack();
    protected abstract IEnumerator Chasing();
    #endregion

}
