using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace noissimEnvironment.LobbyScene
{
    public class UIController : MonoBehaviour
    {

        [Header("Pause Panel")]
        [SerializeField]
        private GameObject pausePanel;
        [SerializeField]
        private Image textPause;

        [SerializeField]
        private GameObject exitButton;
        [SerializeField]
        private GameObject resumeButton;
        [SerializeField]
        private GameObject restartButton;

        [SerializeField]
        private Text exitText;
        [SerializeField]
        private Text resumeText;
        [SerializeField]
        private Text restartText;

        [SerializeField]
        private Image dim;

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

        // Start is called before the first frame update
        void Start()
        { 
            dim.DOFade(0f, 0f);
            textPause.DOFade(0, 0f);
            exitButton.transform.DOScale(Vector3.zero, 0f);
            resumeButton.transform.DOScale(Vector3.zero, 0f);
            restartButton.transform.DOScale(Vector3.zero, 0f);
            exitText.DOFade(0f, 0f);
            resumeText.DOFade(0f, 0f);
            restartText.DOFade(0f, 0f);

            skillPanel = GameObject.Find("SkillPanel").GetComponent<GameObject>();
            skillPanel.SetActive(false);
            skill1Description.SetActive(false);
            skill2Description.SetActive(false);

            Button skill_1 = skill1Button.GetComponent<Button>();
            skill_1.onClick.AddListener(skill1Click);
            Button skill_2 = skill2Button.GetComponent<Button>();
            skill_2.onClick.AddListener(skill2Click);
            Button closeBtn = closeButton.GetComponent<Button>();
            closeBtn.onClick.AddListener(OnSkillPanelClick);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseClick();
            }
        }

        #region On click event methods
        private void OnPauseClick()
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);

                dim.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    exitButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    restartButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    exitText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    restartText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);

                });
            }
            else
            {
                dim.DOFade(0f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    restartButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    exitButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    restartText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    exitText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);

                    TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.5f, () => pausePanel.SetActive(false));
                });

            }
        }

        public void OnSkillPanelClick()
        {
            if (!skillPanel.activeSelf)
            {
                skillPanel.SetActive(true);
                skill1Description.SetActive(false);
                skill1Description.SetActive(false);
            } else
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
            } else
            {
                skill1Description.SetActive(false);
            }
        }

        private void skill2Click()
        {
            if (!skill2Description.activeSelf)
            {
                skill2Description.SetActive(true);
            }
            else
            {
                skill2Description.SetActive(false);
            }
        }
        #endregion
    }
}

