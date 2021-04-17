using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    #region Fields
    [SerializeField]
    private float radiusDamageArea;
    [SerializeField]
    private float offsetX = 0f;
    [SerializeField]
    private float offsetY = 0f;
    [SerializeField]
    private Animator animator = null;

    #endregion


    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 position = transform.position + Vector3.right * offsetX + Vector3.up * offsetY;
        Gizmos.DrawWireSphere(position, radiusDamageArea);
    }

    #endregion

    #region Protected Methods
    protected override void OnHitEvent(Collider2D collision)
    {
        
    }
    #endregion

    #region Public Methods
    public void Trigger(bool isCrit = false)
    {
        this.isCrit = isCrit;
        animator?.Play("explosion");
        Vector2 position = transform.position + Vector3.right * offsetX + Vector3.up * offsetY;
        var collisionObjects = Physics2D.OverlapCircleAll(position, radiusDamageArea);
        foreach (var item in collisionObjects)
        {
            if (item.CompareTag(bulletStat.exceptionTag))
                continue;
            item.GetComponent<Entity>()?.OnTakeDamage(this);
        }
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(1.2f, () => {
            ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
            gameObject.SetActive(false);
        });
    }
    #endregion

}
