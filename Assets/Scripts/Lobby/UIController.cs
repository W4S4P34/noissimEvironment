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
        #endregion
    }
}

