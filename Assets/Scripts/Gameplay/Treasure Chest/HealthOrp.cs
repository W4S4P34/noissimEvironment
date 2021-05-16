using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrp : Item
{
    [SerializeField]
    private float healthAmount = 100f;

    
    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        if (!isOnAction)
            base.Update();
        dropDownItem?.Invoke();
    }
    #endregion

    #region Methods
    public override void TriggerCloseEnough()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        var direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * Time.deltaTime * forceMagnetize;
        if(Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            isOnAction = true;
            Consume();
        }
    }
    public override void Consume()
    {
        ActionEventHandler.Invoke(PlayerCombatEvent.HealEvent, new object[] { healthAmount }, null);
        Destroy(gameObject);
    }
    #endregion
}
