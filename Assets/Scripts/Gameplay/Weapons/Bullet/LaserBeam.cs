using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Bullet
{
    #region Fields
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float damageRate = 0.5f;
    [SerializeField]
    private new Collider2D collider;

    private ContactFilter2D contactFilter2D;
    private bool isActive = false;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            isActive = false;
            TimeManipulator.GetInstance().InvokeActionWithPromise(damageRate, () => {
                List<Collider2D> objectCollisions = new List<Collider2D>();
                if (collider.OverlapCollider(contactFilter2D, objectCollisions) == 0)
                    return;
                foreach (var item in objectCollisions)
                {
                    if (bulletStat.exceptionTag.Contains(item.tag))
                        continue;
                    item.GetComponent<Entity>()?.OnTakeDamage(this);
                }
            }, () => isActive = true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + offset, transform.position);
    }
    #endregion

    #region Methods
    public void Trigger(bool isCrit = false)
    {
        this.isCrit = isCrit;
        transform.position += offset;
        isActive = true;
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(1.3f, () => {
            ObjectPool.ReturnObject(bulletStat.bulletCode, gameObject);
            isActive = false;
            gameObject.SetActive(false);
        });
    }
    #endregion
}
