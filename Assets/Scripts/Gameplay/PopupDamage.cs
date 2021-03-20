using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    public static PopupDamage Create(Vector3 position, int damageAmount, bool isCrit)
    {
        Transform dmgPopupTransform = Instantiate(GameAssets.i.pfPopupDamage, position, Quaternion.identity);
        PopupDamage popupDamage = dmgPopupTransform.GetComponent<PopupDamage>();
        popupDamage.Setup(damageAmount, isCrit);

        return popupDamage;
    }

    private static int sortingOrder;

    private const float _DISAPPEAR_TIMER_MAX = 0.75f;

    private TextMeshPro textMeshPro;
    private float disappearTimer;
    private Color textColor;

    private float popupSpeed = 20f;
    private Vector3 moveVector;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCrit)
    {
        textMeshPro.SetText(damageAmount.ToString());
        if (!isCrit)
        {
            textMeshPro.fontSize = 10;
            textColor = ConvertHex2Color("#ac0d0d");
        }
        else
        {
            textMeshPro.fontSize = 13;
            textColor = ConvertHex2Color("#f0c929");
        }
        textMeshPro.color = textColor;

        disappearTimer = _DISAPPEAR_TIMER_MAX;

        if (sortingOrder >= 100)
        {
            sortingOrder = 0;
        }

        sortingOrder++;
        textMeshPro.sortingOrder = sortingOrder;

        moveVector = new Vector3(Random.Range(-.7f, .7f), 1) * popupSpeed;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > _DISAPPEAR_TIMER_MAX * 0.5f)
        {
            // First half
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            float disappearSpeed = 3f;

            textColor.a -= disappearSpeed * Time.deltaTime;
            textMeshPro.color = textColor;

            if (textMeshPro.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private Color ConvertHex2Color(string hexCode)
    {
        Color textColor;

        if (ColorUtility.TryParseHtmlString(hexCode, out textColor))
        {
            return textColor;
        }

        return Color.white;
    }
}
