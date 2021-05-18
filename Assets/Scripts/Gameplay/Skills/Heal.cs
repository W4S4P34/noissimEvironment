using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/Skill/Heal")]
public class Heal : Skill
{
    public float healthAmount;
    public override void Cast(int skillIndex)
    {
        ActionEventHandler.Invoke(SkillCastEvent.Heal, new object[] { healthAmount, cooldowns, skillIndex }, null);
    }

}
