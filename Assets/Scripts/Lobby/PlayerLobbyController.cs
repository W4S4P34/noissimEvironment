using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using noissimEnvironment.LobbyScene;

public class PlayerLobbyController : MonoBehaviour
{
    [SerializeField]
    private GameObject alertText;
    [SerializeField]
    private GameObject NPCName;
    private UIController uiController;

    private void Start()
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);
        uiController = new UIController();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.tag == "DungeonEntrance")
        {
            alertText.SetActive(true);
        }
        else if (collision.gameObject.tag == "NPC")
        {
            NPCName.SetActive(true);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DungeonEntrance")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(1);
            }
        }
        else if (collision.gameObject.tag == "NPC")
        {
            // Load Skill Panel
            if (Input.GetKeyDown(KeyCode.F))
            {
                uiController.OnSkillPanelClick();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);
    }

    private void Update()
    {

    }
}

