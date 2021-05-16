using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace noissimEnvironment.GameplayScene
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

        [Header("Bullet Item")]
        [SerializeField]
        private Image currentBulletItem;
        [SerializeField]
        private Image bulletItemSizeBg;
        [SerializeField]
        private Text bulletItemSize;
        [SerializeField]
        private List<Sprite> bulletItemSprite;

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

            // Register action event
            ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.SwapBullet, SwapBulletEvent);
            ActionEventHandler.AddNewActionEvent(PlayerCombatEvent.PickBulletItem, PickBulletItemEvent);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseClick();
            }
        }


        private void PickBulletItemEvent(object[] param)
        {
            if (param == null)
                return;
            Sprite bulletSprite = (Sprite)param[1];
            bulletItemSprite.Add(bulletSprite);
            bulletItemSize.text = "2";
            if (bulletItemSprite.Count >= 3)
                bulletItemSprite.RemoveAt(0);
           var gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            var colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            var alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            bulletItemSizeBg.DOGradientColor(gradient, 0.25f);
        }

        private void SwapBulletEvent(object[] param)
        {
            currentBulletItem.DOFade(0f, 0.2f);
            TimeManipulator.GetInstance().InvokeActionAfterSeconds(0.2f, () => {
                currentBulletItem.sprite = bulletItemSprite[(int)param[0]];
                currentBulletItem.DOFade(1f, 0.2f);
            });
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

