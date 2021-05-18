using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill : ScriptableObject
{
    public Sprite skillIcon;
    public string description;
    public float cooldowns;
    public GameObject parcicleSkillEffect;

    public abstract void Cast(int skillIndex);
}
