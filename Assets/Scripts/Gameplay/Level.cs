using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level",menuName ="ScriptableObject/Level")]
public class Level : ScriptableObject
{
    public int maxRoom;
    public int minRoom;

    public List<GameObject> listEnemies;
    public GameObject boss;
    public int maxItemPerTreasure;
    public List<Item> itemChest;
}
