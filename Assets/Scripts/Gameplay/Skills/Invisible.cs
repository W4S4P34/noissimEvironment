using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Invisible", menuName = "ScriptableObject/Skill/Invisible")]
public class Invisible : Skill
{
    [SerializeField]
    private float invisibleTime;

    public override void Cast(int skillIndex)
    {
        ActionEventHandler.Invoke(SkillCastEvent.Invisible, new object[] { invisibleTime, cooldowns, skillIndex }, null);
    }

}
