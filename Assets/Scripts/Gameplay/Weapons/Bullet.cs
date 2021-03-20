using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IEntityDamageEvent
{
    #region Protected Fields
    [SerializeField]
    protected BulletStat bulletStat;
    protected Vector3 shootDirection;
    #endregion

    #region Private Fields
    private bool isCrit;
    private int damageAmount;
    #endregion


    #region Monobehaviour Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitEvent(collision);
    }
    #endregion

    #region Protected Methods
    // Event trigger when hit something may be wall or obstacles or entity,....
    protected virtual void OnHitEvent(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bulletStat.exceptionTag))
            return;
        var entity = collision.GetComponent<Entity>();
        entity?.OnTakeDamage(this);
        ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
        gameObject.SetActive(false);
    }
    #endregion

    #region Public Methods
    // Function apply projectile force for bullet
    public void Setup(Vector3 shootDirection)
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.AddForce(shootDirection * bulletStat.projectileSpeed, ForceMode2D.Impulse);

    }
    // Function calculate damage function
    public float GetDamage(ref bool isCrit)
    {
        // Pseudo Crit & Damage System Demo
        isCrit = Random.Range(0, 100) < bulletStat.critChance;
        return isCrit == true ? bulletStat.damage * 2f : bulletStat.damage;
    }
    public ObjectPoolCode GetBulletCode()
    {
        return bulletStat.bulletCode;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    #endregion
}
