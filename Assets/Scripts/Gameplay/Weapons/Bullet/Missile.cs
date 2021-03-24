using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    [SerializeField]
    private float radiusDamageArea = 0.5f;
    [SerializeField]
    private GameObject pfExplosion = null;
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private GameObject selfExplosion = null;
    #region Monobehaviour Methods
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radiusDamageArea);
    }
    #endregion

    #region Proteced Methods
    // Event trigger when hit something may be wall or obstacles or entity,....
    protected override void OnHitEvent(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag(bulletStat.exceptionTag))
            return;
        var collisionObjects = Physics2D.OverlapCircleAll(collision.transform.position, radiusDamageArea);
        foreach (var item in collisionObjects)
        {
            if (item.CompareTag(bulletStat.exceptionTag))
                continue;
            item.GetComponent<Entity>()?.OnTakeDamage(this);
        }
        if(!selfExplosion)
            selfExplosion = Instantiate(pfExplosion.gameObject);
        selfExplosion.transform.position = transform.position;
        selfExplosion.SetActive(true);
        Invoke("FinishedExplosion", 0.5f);
        ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
        gameObject.SetActive(false);
        
    }

    private void FinishedExplosion()
    {
        selfExplosion.SetActive(false);
    }
    #endregion
}
