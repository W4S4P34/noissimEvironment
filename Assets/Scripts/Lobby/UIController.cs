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
        [Space(10)]
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
        private GameObject settingButton;

        [SerializeField]
        private Text exitText;
        [SerializeField]
        private Text resumeText;
        [SerializeField]
        private Text restartText;

        [SerializeField]
        private Image dim;

        [Space(10)]
        [Header("Panel Settings")]
        [SerializeField]
        private GameObject panelSettings;

        [Space(10)]
        [Header("Panel Skill")]
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

        [Space(10)]
        [Header("Panel Player Experience")]
        [SerializeField]
        private GameObject playerEXPPanel = null;

        [Space(10)]
        [Header("Panel Player Ruby")]
        [SerializeField]
        private GameObject playerRubyPanel = null;

        // Start is called before the first frame update
        void Start()
        {
            skillPanel.SetActive(false);
            skill1Description.SetActive(false);
            skill2Description.SetActive(false);
            Button skill_1 = skill1Button.gameObject.GetComponent<Button>();
            skill_1.onClick.AddListener(skill1Click);
            Button skill_2 = skill2Button.gameObject.GetComponent<Button>();
            skill_2.onClick.AddListener(skill2Click);
            Button closeBtn = closeButton.gameObject.GetComponent<Button>();
            closeBtn.onClick.AddListener(displaySkillPanel);
            Button applyBtn = applyButton.gameObject.GetComponent<Button>();
            applyBtn.onClick.AddListener(displaySkillPanel);

            dim.DOFade(0f, 0f);
            textPause.DOFade(0, 0f);
            exitButton.transform.DOScale(Vector3.zero, 0f);
            resumeButton.transform.DOScale(Vector3.zero, 0f);
            settingButton.transform.DOScale(Vector3.zero, 0f);
            exitText.DOFade(0f, 0f);
            resumeText.DOFade(0f, 0f);
            restartText.DOFade(0f, 0f);

            Button exitBtn = exitButton.GetComponent<Button>();
            exitBtn.onClick.AddListener(OnClick_Exit);

            Button resumeBtn = resumeButton.GetComponent<Button>();
            resumeBtn.onClick.AddListener(OnPauseClick);

            Button settingBtn = settingButton.GetComponent<Button>();
            settingBtn.onClick.AddListener(OnClick_Setting);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseKeyboard();
            }
        }

        #region On click event methods
        private void OnPauseClick()
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                skillPanel.SetActive(false);
                playerEXPPanel.SetActive(false);

                dim.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    exitButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    settingButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    exitText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    restartText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);
                });

            }
            else
            {
                playerEXPPanel.SetActive(true);
                dim.DOFade(0f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    settingButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    exitButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    restartText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    exitText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);

                    TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.5f, () => pausePanel.SetActive(false));
                });

            }
        }

        private void OnPauseKeyboard()
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                skillPanel.SetActive(false);
                playerEXPPanel.SetActive(false);

                dim.DOFade(1f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    exitButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    settingButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    exitText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    restartText.DOFade(1f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);

                });
            }
            else
            {
                playerEXPPanel.SetActive(true);
                dim.DOFade(0f, 0.5f).SetEase(Ease.OutCubic);
                textPause.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.2f);
                TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.25f, () =>
                {
                    settingButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack);
                    resumeButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.15f);
                    exitButton.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

                    restartText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.15f);
                    resumeText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.3f);
                    exitText.DOFade(0f, 0.2f).SetEase(Ease.OutCubic).SetDelay(0.45f);

                    TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.5f, () => pausePanel.SetActive(false));
                });

            }
        }

        public void SetActivePanelSettings(bool state)
        {
            panelSettings.SetActive(state);
        }

        public void OnClick_Setting()
        {
            SetActivePanelSettings(!panelSettings.activeSelf);
        }

        public void displaySkillPanel()
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
        #endregion

        public void OnClick_Exit()
        {

        }
    }
}

