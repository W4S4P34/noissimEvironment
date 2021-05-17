using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using noissimEnvironment.LobbyScene;

public class PlayerLobbyController : MonoBehaviour
{
    [SerializeField]
    private GameObject alertText;
    [SerializeField]
    private GameObject NPCName;

    [Header("Skill Panel")]
    [SerializeField]
    private GameObject skillPanel;

    [SerializeField]
    private Image background;
    [SerializeField]
    private GameObject skillPanelText;

    [SerializeField]
    private GameObject skill1Description;
    [SerializeField]
    private GameObject skill2Description;

    [SerializeField]
    private GameObject skill1Button;
    [SerializeField]
    private GameObject skill2Button;

    [SerializeField]
    private GameObject closeButton;
    [SerializeField]
    private GameObject applyButton;

    private void Start()
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);

        skillPanel.SetActive(false);
        skill1Description.SetActive(false);
        skill2Description.SetActive(false);

        Button skill_1 = skill1Button.GetComponent<Button>();
        skill_1.onClick.AddListener(skill1Click);
        Button skill_2 = skill2Button.GetComponent<Button>();
        skill_2.onClick.AddListener(skill2Click);
        Button closeBtn = closeButton.GetComponent<Button>();
        closeBtn.onClick.AddListener(OnSkillPanelClick);
        Button applyBtn = applyButton.GetComponent<Button>();
        applyBtn.onClick.AddListener(OnSkillPanelClick);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
                NPCName.SetActive(false);
                OnSkillPanelClick();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        alertText.SetActive(false);
        NPCName.SetActive(false);
    }

    public void OnSkillPanelClick()
    {
        if (!skillPanel.activeSelf)
        {
            skillPanel.SetActive(true);
            skill1Description.SetActive(false);
            skill1Description.SetActive(false);
        }
        else
        {
            skillPanel.SetActive(false);
            skill1Description.SetActive(false);
            skill1Description.SetActive(false);
        }
    }

    private void skill1Click()
    {
        if (!skill1Description.activeSelf)
        {
            skill1Description.SetActive(true);
            skill2Description.SetActive(false);
        }
        else
        {
            skill1Description.SetActive(false);
        }
    }

    private void skill2Click()
    {
        if (!skill2Description.activeSelf)
        {
            skill2Description.SetActive(true);
            skill1Description.SetActive(false);
        }
        else
        {
            skill2Description.SetActive(false);
        }
    }

    private void Update()
    {

    }
}

