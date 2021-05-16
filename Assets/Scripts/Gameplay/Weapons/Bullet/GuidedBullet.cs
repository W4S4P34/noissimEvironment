using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
public class GuidedBullet : Bullet
{


    [SerializeField]
    private AIDestinationSetter aiDestinationSetter;
    [SerializeField]
    private AIPath aiPath;
    [SerializeField]
    private Seeker seeker;

    [Header("Stat")]
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private bool increaseSpeedOverTime;


    private bool isActive = false;
    private Vector3 targetDirection;
    
    #region Monobehaviour Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        Trigger(false);
    }

    private void Update()
    {
        if (isActive)
        {
            if (aiDestinationSetter.target == null)
                aiDestinationSetter.target = GameObject.FindWithTag("Player")?.transform;

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            if (aiDestinationSetter.target == null)
                aiDestinationSetter.target = GameObject.FindWithTag("Player")?.transform;

            // Rotation
            targetDirection = (aiDestinationSetter.target.position - transform.position).normalized;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);


        }
    }

    #endregion

    public void Trigger(bool isCrit = false)
    {
        this.isCrit = isCrit;
        aiPath.canMove = true;
        isActive = true;
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(lifetime, () => {
            isActive = false;
            aiPath.canMove = false;
            Setup(targetDirection, isCrit);
        });
    }


}
