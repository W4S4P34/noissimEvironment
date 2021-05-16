using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IWeapon
{
    #region Protected Fields

    #region SerializeField
    [Header("GUN FIELDS")]
    [SerializeField, Range(1,30)]
    protected int bulletPerClip = 30;
    [SerializeField]
    protected Bullet pfBullet = null;
    [SerializeField]
    protected float fireRate = 0f;
    [SerializeField, Range(0,100)]
    protected int critChance = 0;
    #endregion

    protected Animator aimAnimator;
    protected Transform weaponTransform;
    protected Transform aimTransform;
    protected bool isOnAction = false;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!pfBullet)
            Debug.LogError("Bullet prefab is null");
        weaponTransform = transform;
        aimTransform = transform.parent;
        aimAnimator = aimTransform.GetComponent<Animator>();
    }
    #endregion

    #region Methods
    public abstract void CatchFireEvent();
    public abstract IEnumerator Attack();
    public virtual void SwapBullet(Bullet pfBullet)
    {
        this.pfBullet = pfBullet;
    }
    #endregion


}
