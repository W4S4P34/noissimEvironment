using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pfGoPopup;
    [SerializeField]
    private GameObject pfGoPopupParticle;
    [SerializeField]
    private GameObject pfWinPopup;

    [SerializeField]
    private GameObject resultPanel;
    private GameplayUIHandler uiHandler;

    private Action slowMotionEndLevelAction;

    private void Awake()
    {
        uiHandler = resultPanel.gameObject.GetComponent<GameplayUIHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ActionEventHandler.AddNewActionEvent(GameDungeonEvent.PrepareDugeon, PrepareDungeon);
        ActionEventHandler.AddNewActionEvent(GameDungeonEvent.StartDungeon, StartDungeon);
        ActionEventHandler.AddNewActionEvent(GameDungeonEvent.EndGame, EndLevel);
    }
    private void Update()
    {
        slowMotionEndLevelAction?.Invoke();
    }


    private void StartDungeon()
    {
        Destroy(pfGoPopup);
        Destroy(pfGoPopupParticle);

        TreasureChest.CreateTreasureChest(GameDataHandler.instance.listLevel[GameDataHandler.instance.currentLevel].itemChest, GameDataHandler.instance.listLevel[GameDataHandler.instance.currentLevel].maxItemPerTreasure, GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    private void PrepareDungeon()
    {
        var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
        spawnPosition.z = 0f;
        pfGoPopup = Instantiate(pfGoPopup, spawnPosition, Quaternion.identity);
        pfGoPopupParticle = Instantiate(pfGoPopupParticle, spawnPosition, Quaternion.identity);

        AstarPath.active.Scan(AstarPath.active.data.gridGraph);
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(2f, () =>
        {
            ActionEventHandler.Invoke(GameDungeonEvent.StartDungeon);
        });

    }

    private void EndLevel()
    {
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale / 0.02f;

        // Slow motion action
        slowMotionEndLevelAction = () => {
            Time.timeScale = Mathf.Clamp(Time.timeScale + (1f / 3f) * Time.unscaledDeltaTime, 0f, 1f);
            Time.fixedDeltaTime = Time.unscaledDeltaTime;
        };

        // Victory pop up
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(3f, () => {
            slowMotionEndLevelAction = null;
            var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2));
            spawnPosition.z = 0f;
            pfWinPopup = Instantiate(pfWinPopup, spawnPosition, Quaternion.identity);
            pfWinPopup.transform.DOScale(0f, 0f);
            pfWinPopup.transform.DOScale(1f, 0.5f);
        });

        // Open result panel
        TimeManipulator.GetInstance().InvokeActionAfterSeconds(6f, () => {
            uiHandler.showResult(true);
        });
    }
}
