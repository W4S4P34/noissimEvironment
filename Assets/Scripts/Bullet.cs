using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;

    private int damageAmount;
    private int critChance;
    private bool isCrit;

    private void Awake()
    {
        critChance = 30;
    }

    public void Setup(Vector3 shootDirection)
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

        float moveSpeed = 50f;
        rigidbody2D.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pseudo Crit & Damage System Demo
        isCrit = Random.Range(0, 100) < critChance;

        if (isCrit)
        {
            damageAmount = Random.Range(400, 500);
        }
        else
        {
            damageAmount = Random.Range(150, 350);
        }

        if(collision.gameObject.tag == "Minion")
        {
            PopupDamage.Create(collision.gameObject.transform.position, damageAmount, isCrit);
            Destroy(gameObject);
        }
    }
}
