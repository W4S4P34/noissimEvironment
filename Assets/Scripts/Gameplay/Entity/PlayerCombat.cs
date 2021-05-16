using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Entity
{
    private Transform aimTransform;

    private IWeapon weapon;
    private SpriteRenderer spriteRenderer;

    #region Monobehaviour Methods
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aimTransform = transform.Find("Aim");
        weapon = aimTransform.Find("Weapon").GetComponent<IWeapon>();

        ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.HealEvent, Heal);
    }
    // Update is called once per frame
    private void Update()
    {
        HandleAiming();
        weapon?.CatchFireEvent();
    }
    #endregion

    #region Methods
    public override void OnTakeDamage(IEntityDamageEvent e)
    {
        var damage = e.GetDamage(ref isCrit);
        entityStat.TakeDamage(damage, OnDied);
        // Create pop up damage here
        PopupDamage.Create(transform.position, (int)damage, isCrit);
        // Add animation hit here
        animator.SetTrigger("onHit");
    }
    private void HandleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
            spriteRenderer.flipX = true;
        }
        else
        {
            aimLocalScale.y = 1f;
            spriteRenderer.flipX = false;
        }
        aimTransform.localScale = aimLocalScale;
    }

    private void Heal(object[] param, Action onHealComplete)
    {
        entityStat.Heal((float) param[0]);
        // Play effect here

        onHealComplete?.Invoke();
    }
    #endregion

}
