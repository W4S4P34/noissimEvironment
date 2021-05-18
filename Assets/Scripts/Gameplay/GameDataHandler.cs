using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    public static GameDataHandler instance { get; private set; } = null;

    public List<Level> listLevel = null;

    public int currentLevel { get; private set; } = 0;


    private void Awake()
    {

        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
    }
}
