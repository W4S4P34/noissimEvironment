using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Entity
{
    [SerializeField]
    private List<Bullet> listBullet;

    private Transform aimTransform;

    private IWeapon weapon;
    private SpriteRenderer spriteRenderer;
    private int currentBullet = 0;

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

        // Register action event
        ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.HealEvent, Heal);
        ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.SwapBullet, SwapBulletEvent);
        ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.PickBulletItem, PickBulletItemEvent);
    }
    // Update is called once per frame
    private void Update()
    {
        HandleAiming();
        weapon?.CatchFireEvent();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapBullet();
        }
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

    private void SwapBullet()
    {
        if (listBullet.Count < 2) // Invoke event display error behaviour
            return;
        currentBullet = currentBullet == 0 ? 1 : 0;
        ActionEventHandler.Invoke(PlayerCombatEvent.SwapBullet, new object[] { currentBullet }, null);
    }

    #endregion


    #region Action event methods
    private void PickBulletItemEvent(object[] param)
    {
        if (param == null)
            return;
        Bullet bulletOrp = (Bullet) param[0];
        if(listBullet.Count >= 2)
            listBullet.RemoveAt(0);
        listBullet.Add(bulletOrp);
    }

    private void SwapBulletEvent(object[] param)
    {
        ((Gun)weapon).SwapBullet(listBullet[currentBullet]);
    }

    private void Heal(object[] param)
    {
        entityStat.Heal((float)param[0]);
        // Play effect here

    }
    #endregion

}
