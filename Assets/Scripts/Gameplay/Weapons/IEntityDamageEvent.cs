using UnityEngine;
public interface IEntityDamageEvent
{
    float GetDamage(ref bool isCrit);
    GameObject GetGameObject();
}
