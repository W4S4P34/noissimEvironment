using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStat))]
[RequireComponent(typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    #region Protected Fields
    [SerializeField]
    protected EntityStat entityStat = null;
    [SerializeField]
    protected Animator animator = null;
    #endregion

    #region Private Fields
    private bool isCrit = false;
    #endregion

    #region Monobehaviour Methods
    protected virtual void Start()
    {
        if (!entityStat) entityStat = GetComponent<EntityStat>();
        if (!animator) animator = GetComponent<Animator>();
    }
    #endregion

    #region Public Methods
    // Callback function when player take damage
    public virtual void OnTakeDamage(IEntityDamageEvent e)
    {
        var damage = e.GetDamage(ref isCrit);
        entityStat.TakeDamage(damage, OnDied);
        // Create Pop up damage here
        
        // Add animation hit here

        
    }
    #endregion

    #region Protected Methods
    // Callback function when player died
    protected virtual void OnDied() {
        Destroy(gameObject);
        // Add animation died here

    }
    #endregion
}
