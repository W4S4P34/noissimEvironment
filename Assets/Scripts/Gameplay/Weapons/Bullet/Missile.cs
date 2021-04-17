using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    
    [SerializeField]
    private float radiusDamageArea = 0.5f;

    private GameObject selfExplosion = null;
    #region Monobehaviour Methods
    protected override void Start()
    {
        base.Start();
    }
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
        var collisionObjects = Physics2D.OverlapCircleAll(transform.position, radiusDamageArea);
        foreach (var item in collisionObjects)
        {
            if (item.CompareTag(bulletStat.exceptionTag))
                continue;
            item.GetComponent<Entity>()?.OnTakeDamage(this);
        }
        if(!selfExplosion)
            selfExplosion = Instantiate(bulletStat.pfExplosion.gameObject);
        selfExplosion.transform.position = transform.position;
        selfExplosion.transform.rotation = transform.rotation;
        selfExplosion.SetActive(true);
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.5f, FinishedExplosion);
        ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
        gameObject.SetActive(false);
        
    }

    private void FinishedExplosion()
    {
        selfExplosion.SetActive(false);
    }
    #endregion
}
