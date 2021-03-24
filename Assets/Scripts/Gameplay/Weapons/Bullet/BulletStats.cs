using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletStatExample", menuName = "ScriptableObject/BulletStat")]
public class BulletStats : ScriptableObject
{
    [Range(0f, 1000f)]
    public float damage = 50f;
    [Range(0f, 10f)]
    public float repulsiveForce = 0f;
    [Range(0f, 100f)]
    public float projectileSpeed = 50f;
    [TagSelector]
    public string exceptionTag;
    public ObjectPoolCode bulletCode;
}
