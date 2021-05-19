using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevel : ScriptableObject
{
    [SerializeField]
    private long[] expStage = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    public double calculateLevel()
    {
        long currentExp = ProgressSerial.getInstance().ExpToSave;

        double level = 1;

        for(int i = 0; i < expStage.Length; i++)
        {
            if(currentExp > expStage[i])
            {
                currentExp -= expStage[i];
                level++;
            }
            else
            {
                level += currentExp * 1.0 / expStage[i];
                break;
            }
        }

        return level;
    }
}