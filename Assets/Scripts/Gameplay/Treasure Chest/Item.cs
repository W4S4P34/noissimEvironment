using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Item : MonoBehaviour
{
    #region Fields
    protected int dropItemRate { get; private set; }
    [SerializeField]
    protected float eventTriggerRange;

    protected static float forceMagnetize = 10f;
    protected static GameObject player;
    protected static float friction = 0f;

    protected bool isOnAction = true;
    protected Action dropDownItem;


    #endregion


    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }
    // Update is called once per frame
    protected virtual void Update()
    {
        var colliderObjects = Physics2D.OverlapCircleAll(transform.position, eventTriggerRange);
        foreach (var item in colliderObjects)
        {
            if (item.CompareTag("Player") && item.gameObject.layer == LayerMask.NameToLayer("Player"))
                TriggerCloseEnough();
        }
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, eventTriggerRange);
    }
    #endregion

    /// <summary>
    /// Event trigger when player close item. 
    /// - Item automatic go into player's bag or open details info panel for player
    /// </summary>
    public abstract void TriggerCloseEnough();

    /// <summary>
    /// Call when player use(consume) this item
    /// </summary>
    public virtual void Consume() { }

    /// <summary>
    /// Event trigger when treasure open and item drop out
    /// </summary>
    public virtual void DropOut() {
        isOnAction = true;
        Vector3 velocity = new Vector3(Random.Range(-2, 2),Random.Range(-2, 2));
        if(dropDownItem == null)
        {
            dropDownItem = () =>
            {
                // apply speed
                transform.position += velocity * Time.deltaTime;
                // find speed vector length:
                var len = Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
                if (len > 0)
                {
                    // find the multiplier (new speed divided by current):
                    var mul = Mathf.Max(len - friction, 0) / len;
                    // apply the multiplier:
                    velocity *= mul;
                }
            };
        }
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(2f, () =>
        {
            isOnAction = false;
            dropDownItem = null;
        });
    }



}
