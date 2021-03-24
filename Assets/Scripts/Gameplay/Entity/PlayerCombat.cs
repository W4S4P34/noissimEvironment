using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Entity
{
    private Transform aimTransform;

    [SerializeField] 
    private Bullet pfBullet;
    private Transform weaponTransform;

    private Animator aimAnimator;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        aimTransform = transform.Find("Aim");
        weaponTransform = aimTransform.Find("Weapon");
        aimAnimator = aimTransform.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Start()
    {
        base.Start();
        ObjectPool.RegisterObjectPoolItem(pfBullet.GetBulletCode(), pfBullet.gameObject, 50);
    }
    // Update is called once per frame
    private void Update()
    {
        HandleAiming();
        HandleShooting();
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

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aimAnimator.SetTrigger("isShooting");

            Vector3 bulletPosition = weaponTransform.position;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = mousePosition - bulletPosition;
            shootDirection.Normalize();

            var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
            _bullet.SetActive(true);
            _bullet.transform.position = bulletPosition;
            _bullet.GetComponent<Bullet>().Setup(shootDirection);
        }
    }
}
