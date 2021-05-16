using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

public class EntityStats : MonoBehaviour
{
    #region Protected Fields
    [Header("ENTITY STAT")]
    [SerializeField, Range(100f, 5000f)]
    protected float maxHp = 200f;
    [SerializeField, Range(0f, 200f)]
    protected float maxArmor = 200f;
    [SerializeField]
    protected Slider healthBar = null;
    [SerializeField]
    protected Slider armorBar = null;
    [SerializeField]
    protected TMP_Text healthText = null;
    [SerializeField]
    protected TMP_Text armorText = null;

    protected float currentHp;
    protected float currentArmor;
    protected const float MIN_HEALTH = 0f;
    protected float smoothDuration = 1f;
    #endregion

    #region Monobehavior Methods
    private void Start()
    {
        if (!healthBar) Debug.LogError("HEALTH BAR IS EMPTY");
        currentHp = maxHp;
        currentArmor = maxArmor;

        healthBar.maxValue = maxHp;
        healthBar.value = maxHp;
        if (armorBar)
        {
            armorBar.maxValue = maxArmor;
            armorBar.value = maxArmor;
            armorText?.SetText("100%");
            healthText?.SetText("100%");
        }
    }
    #endregion

    #region Public Methods
    public void TakeDamage(float damage, Action onDied) {
        if (armorBar && currentArmor > 0)
        {
            // Calculate current armor here
            currentArmor -= damage;
            if (currentArmor <= MIN_HEALTH)
                currentArmor = 0;
            armorBar.DOValue(currentArmor, smoothDuration).SetEase(Ease.Linear);
            armorText?.SetText((currentArmor / maxArmor * 100).ToString() + "%");
            return;
        }
        // Calculate current HP here
        currentHp -= damage;
        healthBar.DOValue(currentHp, smoothDuration).SetEase(Ease.Linear);
        healthText?.SetText((currentHp / maxHp * 100).ToString() + "%");
        if (currentHp <= MIN_HEALTH)
            onDied?.Invoke();
    }

    public void Heal(float amount) {
        Debug.Log("Heal");
        // Calculate current HP here
        currentHp += amount;
        if (currentHp >= maxHp)
            currentHp = maxHp;
        healthBar.DOValue(currentHp, smoothDuration).SetEase(Ease.Linear);
        healthText?.SetText((currentHp / maxHp * 100).ToString() + "%");
    }

    #endregion
}