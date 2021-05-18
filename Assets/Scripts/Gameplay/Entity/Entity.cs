using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
[RequireComponent(typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    #region Protected Fields
    [Header("ENTITY FIELDS")]
    [SerializeField]
    protected EntityStats entityStat = null;
    [SerializeField]
    protected Animator animator = null;
    protected bool isDeath = false;
    #endregion

    #region Private Fields
    protected bool isCrit = false;
    #endregion

    #region Monobehaviour Methods
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        if (!entityStat) entityStat = GetComponent<EntityStats>();
        if (!animator) animator = GetComponent<Animator>();
    }
    #endregion

    #region Public Methods
    // Callback function when player take damage
    public virtual void OnTakeDamage(IEntityDamageEvent e)
    {
        if (isDeath)
            return;
        var damage = e.GetDamage(ref isCrit);
        entityStat.TakeDamage(damage, OnDied);
        // Create pop up damage here
        PopupDamage.Create(transform.position, (int) damage, isCrit);
        
    }
    #endregion

    #region Protected Methods
    // Callback function when player died
    protected virtual void OnDied() {
        isDeath = true;
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(1f, () => Destroy(gameObject));
    }
    #endregion
}
