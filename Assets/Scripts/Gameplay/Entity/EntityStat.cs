using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EntityStat : MonoBehaviour
{
    #region Protected Fields
    [Header("ENTITY STAT")]
    [SerializeField, Range(100f, 1000f)]
    protected float maxHp = 200f;
    [SerializeField]
    protected float armor = 10f;
    [SerializeField]
    protected Slider healthBar = null;

    protected float currentHp;
    protected const float MIN_HEALTH = 0f;
    #endregion

    #region Monobehavior Methods
    private void Start()
    {
        if (!healthBar) Debug.LogError("HEALTH BAR IS EMPTY");
        currentHp = maxHp;
        healthBar.maxValue = maxHp;
        healthBar.value = maxHp;
    }
    #endregion

    #region Public Methods
    public void TakeDamage(float damage, Action onDied) {
        // Calculate current HP here
        currentHp -= damage;
        healthBar.value = currentHp;
        if (currentHp <= MIN_HEALTH)
            onDied?.Invoke();
    }

    public void Heal(float amount) {
        // Calculate current HP here

    }

    #endregion
}