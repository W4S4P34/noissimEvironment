using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimDirection : MonoBehaviour
{
    private Transform aimTransform;
    private Animator aimAnimator;

    [SerializeField] private Transform pfBullet;
    private Transform weaponTransform;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        weaponTransform = aimTransform.Find("Weapon");
        aimAnimator = aimTransform.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
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
            Transform bulletTransform = Instantiate(pfBullet, bulletPosition, Quaternion.identity);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = mousePosition - bulletPosition;
            shootDirection.Normalize();
            bulletTransform.GetComponent<Bullet>().Setup(shootDirection);
        }
    }
}
