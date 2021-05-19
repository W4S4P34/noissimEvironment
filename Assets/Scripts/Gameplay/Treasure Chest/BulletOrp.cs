using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BulletOrp : Item
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string description;
    [SerializeField]
    private Bullet pfBullet;

    private Action openBulletStatPanel;

    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }
    protected override void Update()
    {
        if (!isOnAction)
        {
            var colliderObjects = Physics2D.OverlapCircleAll(transform.position + offset, eventTriggerRange);
            foreach (var item in colliderObjects)
            {
                if (item.CompareTag("Player") && item.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    TriggerCloseEnough();
                    return;
                }
            }
            TriggerExitBound();
        }
        dropDownItem?.Invoke();
    }
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + offset, eventTriggerRange);
    }
    #endregion

    #region Methods
    public override void TriggerCloseEnough()
    {
        // Turn on UI panel
        if (openBulletStatPanel != null)
        {
            openBulletStatPanel.Invoke();
            openBulletStatPanel = null;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOnAction = true;
            Consume();
        }
    }
    private void TriggerExitBound()
    {
        // Turn off UI panel
    }
    public override void Consume()
    {
        // Instantiate bullet to use
        // Prevent instantiate too much
        if (ObjectPool.GetObject(pfBullet.GetBulletCode()) == null)
            ObjectPool.RegisterObjectPoolItem(pfBullet.GetBulletCode(), pfBullet.gameObject, 10);
        ActionEventHandler.Invoke(PlayerCombatEvent.PickBulletItem, new object[] { pfBullet, sprite, gameObject }, null);
    }
    #endregion
}
