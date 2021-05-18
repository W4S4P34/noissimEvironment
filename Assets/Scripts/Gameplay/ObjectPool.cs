using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ObjectPoolCode
{
    KuornosBullet,
    ClassicBullet_2,
    ClassicBulletMonster,
    ClassicMissleBullet,
    ClassicDestructionBullet,
    ClassicGuidedBullet,
    ClassicLaserBeam,
    SpawnPositionSign_1
}
public class ObjectPool : MonoBehaviour
{
    private static Dictionary<ObjectPoolCode, Queue<GameObject>> objectPoolQueue = null;
    private static Dictionary<ObjectPoolCode, GameObject> objectPool = null;

    private void Awake()
    {
        objectPoolQueue = new Dictionary<ObjectPoolCode, Queue<GameObject>>();
        objectPool = new Dictionary<ObjectPoolCode, GameObject>();
    }
    public static void RegisterObjectPoolItem(ObjectPoolCode code, GameObject gameObject, int amount)
    {
        if (!objectPoolQueue.TryGetValue(code,out Queue<GameObject> objectPoolItem))
        {
            objectPool.Add(code, gameObject);

            objectPoolItem = new Queue<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                var item = Instantiate(gameObject);
                item.SetActive(false);
                objectPoolItem.Enqueue(item);
            }
            objectPoolQueue.Add(code, objectPoolItem);

        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                var item = Instantiate(gameObject);
                item.SetActive(false);
                objectPoolItem.Enqueue(item);
            }
            
        }
    }

    public static GameObject GetObject(ObjectPoolCode code)
    {
        if (!objectPoolQueue.ContainsKey(code))
        {
            return null;
        }
        var objectPoolItem = objectPoolQueue[code];
        if(objectPoolItem.Count <= 0)
        {
            var item = Instantiate(objectPool[code]);
            item.SetActive(false);
            objectPoolItem.Enqueue(item);
        }
        return objectPoolItem.Dequeue();
    }

    public static void ReturnObject(ObjectPoolCode code, GameObject gameObject)
    {
        if (objectPoolQueue.ContainsKey(code))
        {
            objectPoolQueue[code].Enqueue(gameObject);
        }
    }
}
