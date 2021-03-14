using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform mousePosition;

    private void Start()
    {
        // PopupDamage.Create(Vector3.zero, 300);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool isCrit = Random.Range(0, 100) < 30;

            PopupDamage.Create(mousePosition.position, 300, isCrit);
        }
    }
}
