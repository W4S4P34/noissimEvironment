using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    [SerializeField]
    private List<Level> listLevel = null;
    [SerializeField]
    private List<Item> listItem = null;

    private int MAX_ITEM = 4;
    // Start is called before the first frame update
    void Start()
    {
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(1f, () =>
        {
            TreasureChest.CreateTreasureChest(listItem, MAX_ITEM, new Vector3(-1.1f,-6.6f));
        });
    }

}
