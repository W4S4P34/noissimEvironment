using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private GameObject gameplayUI;

    [SerializeField]
    private Sprite spVictory;

    [SerializeField]
    private Sprite spDefeat;

    [SerializeField]
    private Image statusText;

    [SerializeField]
    private TextMeshProUGUI monsterKilled;

    [SerializeField]
    private TextMeshProUGUI expGained;

    [SerializeField]
    private TextMeshProUGUI rubyCollected;

    [SerializeField]
    private GameObject nextLevelButton;

    public void showResult(bool isVictory)
    {
        monsterKilled.text = GameplayCounter.getInstance().MonsterKilled.ToString();
        rubyCollected.text = GameplayCounter.getInstance().RubyCollected.ToString();

        if (isVictory)
        {
            statusText.sprite = spVictory;
            expGained.text = "+ 100";
            nextLevelButton.SetActive(true);
        }
        else
        {
            statusText.color = new Color(1f, 0f, 0f);
            statusText.sprite = spDefeat;
            expGained.text = "+ 0";
            nextLevelButton.SetActive(false);
        }

        ProgressSerial progressSerial = ProgressSerial.getInstance();
        progressSerial.RubyToSave += GameplayCounter.getInstance().RubyCollected;
        progressSerial.ExpToSave += isVictory ? 100 : 0;
        progressSerial.saveData();


        gameplayUI.SetActive(false);
        resultPanel.SetActive(true);
    }
}
