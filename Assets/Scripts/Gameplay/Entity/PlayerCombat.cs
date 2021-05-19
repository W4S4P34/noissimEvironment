using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Entity
{
    [SerializeField]
    private List<Bullet> listBullet;
    [SerializeField]
    private List<Skill> listSkills;


    private bool [] isOnCastSkill = new bool[] { false, false };
    private bool isIntangible = false;

    private Transform aimTransform;

    private IWeapon weapon;
    private SpriteRenderer spriteRenderer;
    private int currentBullet = 0;

    private ParticleSystem[] healParticleSystem;
    private GameObject healParticleEffect;
    

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
        ActionEventHandler.AddNewActionEvent(SkillCastEvent.Heal, Heal);
        ActionEventHandler.AddNewActionEvent(SkillCastEvent.Invisible, BecomeIntangible);
        
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
        if (Input.GetKeyDown(KeyCode.E) && !isOnCastSkill[0])
        {
            isOnCastSkill[0] = true;
            listSkills[0]?.Cast(0);
            TimeManipulator.GetInstance().InvokeActionAfterSeconds(listSkills[0].cooldowns, () => isOnCastSkill[0] = false);
        }
        if (Input.GetKeyDown(KeyCode.R) && !isOnCastSkill[1])
        {
            isOnCastSkill[1] = true;
            listSkills[1]?.Cast(1);
        }
    }
    #endregion

    #region Methods
    public override void OnTakeDamage(IEntityDamageEvent e)
    {
        if (isIntangible)
            return;
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
        if (listBullet.Count < 2)
        {
            // Invoke event display error behaviour
            //ActionEventHandler.Invoke(PlayerCombatEvent.DisplayErrorBehaviour)
            return;
        }
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
        listBullet.Add(bulletOrp);
        if (listBullet.Count >= 3)
        {
            listBullet.RemoveAt(0); 
            SwapBullet();
        }
    }

    private void SwapBulletEvent(object[] param)
    {
        if (param == null)
            return;
        ((Gun)weapon).SwapBullet(listBullet[currentBullet]);
    }

    private void Heal(object[] param)
    {
        if (param == null)
            return;
        entityStat.Heal((float)param[0]);
        if(param.Length > 1)
        {
            ActionEventHandler.Invoke(SkillCastEvent.UIChangeEvent, param, null);
            // Play effect here
            if (!healParticleEffect)
            {
                healParticleEffect = Instantiate(listSkills[(int)param[2]].parcicleSkillEffect, transform.position + Vector3.up * -0.59f + Vector3.right * 0.14f, Quaternion.identity);
                healParticleEffect.transform.SetParent(this.transform);
                healParticleSystem = healParticleEffect?.GetComponentsInChildren<ParticleSystem>();
            }
            foreach (var item in healParticleSystem)
            {
                item.Play();
            }

        }
    }

    private void BecomeIntangible(object[] param)
    {
        if (param == null)
            return;
        Debug.Log("Invoke visible");
        isIntangible = true;
        animator.SetBool("isInvisible", true);
        TimeManipulator.GetInstance().InvokeActionAfterSeconds((float) param[0], () => {
            isIntangible = false;
            animator.SetBool("isInvisible", false);
            ActionEventHandler.Invoke(SkillCastEvent.UIChangeEvent, param, null);
            TimeManipulator.GetInstance().InvokeActionAfterSeconds((float) param[1], () => isOnCastSkill[(int)param[2]] = false);
        });
        // Play effect here

    }

    protected override void OnDied()
    {
        isDeath = true;
        ActionEventHandler.Invoke(GameDungeonEvent.LoseGame);
        gameObject.SetActive(false);
    }
    #endregion

}
