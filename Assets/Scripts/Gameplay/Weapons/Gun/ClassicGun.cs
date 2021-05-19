using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGun : Gun
{
    #region Private Fields
    [SerializeField]
    private AudioSource effectAudioSource;
    [SerializeField]
    private AudioClip fireAudioClip;
    #endregion

    #region Monobehaviour Methods
    protected override void Start()
    {
        base.Start();
        ObjectPool.RegisterObjectPoolItem(pfBullet.GetBulletCode(), pfBullet.gameObject, bulletPerClip);
    }
    #endregion

    #region Public Methods
    public override void CatchFireEvent()
    {
        if (Input.GetButtonDown("Fire1") && !isOnAction)
        {
            isOnAction = true;
            StartCoroutine(Attack());
        }
    }
    public override IEnumerator Attack()
    {
        effectAudioSource.clip = fireAudioClip;
        effectAudioSource.Play();
        aimAnimator.SetTrigger("isShooting");

        Vector3 bulletPosition = weaponTransform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = mousePosition - bulletPosition;
        shootDirection.Normalize();

        var _bullet = ObjectPool.GetObject(pfBullet.GetBulletCode());
        _bullet.SetActive(true);
        _bullet.transform.position = bulletPosition;
        _bullet.transform.rotation = aimTransform.rotation;
        _bullet.GetComponent<Bullet>().Setup(shootDirection, Random.Range(1,101) <= critChance);
        yield return new WaitForSeconds(fireRate);
        isOnAction = false;
    }
    #endregion
}
