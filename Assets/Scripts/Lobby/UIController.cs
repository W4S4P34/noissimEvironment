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
        [Header("========== Panel Settings ==========")]
        [SerializeField]
        private GameObject panelSettings = null;

        [Space(10)]
        [Header("========== Panel Settings ==========")]
        [SerializeField]
        private GameObject skillPanel = null;

        [Space(10)]
        [Header("========== Panel Settings ==========")]
        [SerializeField]
        private GameObject playerEXPPanel = null;

        // Start is called before the first frame update
        void Start()
        {
            panelSettings.SetActive(false);
            skillPanel.SetActive(false);
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
        #endregion

        public void OnClick_Exit()
        {

        }
    }
}

