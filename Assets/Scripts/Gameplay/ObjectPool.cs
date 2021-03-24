using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ObjectPoolCode
{
    KuornosBullet,
    ClassicBulletMonster,
    MissleBullet
}
public class ObjectPool : MonoBehaviour
{
    private static Dictionary<ObjectPoolCode, Queue<GameObject>> objectPool = null;

    private void Awake()
    {
        objectPool = new Dictionary<ObjectPoolCode, Queue<GameObject>>();
    }
    public static void RegisterObjectPoolItem(ObjectPoolCode code, GameObject gameObject, int amount)
    {
        if (!objectPool.TryGetValue(code,out Queue<GameObject> objectPoolItem))
        {
            objectPoolItem = new Queue<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                var item = Instantiate(gameObject);
                item.SetActive(false);
                objectPoolItem.Enqueue(item);
            }
            objectPool.Add(code, objectPoolItem);
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
        if (!objectPool.ContainsKey(code))
        {
            return null;
        }
        var objectPoolItem = objectPool[code];
        return objectPoolItem.Dequeue();
    }

    public static void ReturnObject(ObjectPoolCode code, GameObject gameObject)
    {
        if (objectPool.ContainsKey(code))
        {
            objectPool[code].Enqueue(gameObject);
        }
    }
}
