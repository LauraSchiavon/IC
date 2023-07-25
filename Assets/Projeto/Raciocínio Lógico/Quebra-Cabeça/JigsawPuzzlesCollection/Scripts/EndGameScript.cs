using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JigsawPuzzlesCollection.Scripts
{
    public class EndGameScript : MonoBehaviour
    {
        public GameObject piecesContainer;
        public GameObject background;
        public GameObject menuContainer;

        [Header("Success Panel")]
        public bool showPanel;
        public GameObject panel;
        public float showPanelSlideDuration = 2f;

        [Header("Success Behavior")]
        public bool hideSeams;
        public bool showFullImage;
        public float showFullImageScaleDuration = 1.5f;

        private bool m_gameCompleted;

        public void Awake()
        {
            panel.SetActive(false);
        }

        public void EndGame()
        {
            var selectedLevel = LevelSelectorScript.Instance?.SelectedLevel;

            if (selectedLevel != null)
            {
                var backgroundImageComponents = background.GetComponentsInChildren<Image>();
                foreach (var backgroundImageComponent in backgroundImageComponents)
                {
                    backgroundImageComponent.sprite = selectedLevel.Image;
                    backgroundImageComponent.overrideSprite = selectedLevel.Image;
                }
            }

            if (showPanel)
            {
                menuContainer?.SetActive(false);

                if (panel?.activeSelf == false)
                {
                    panel.SetActive(true);
                    StartCoroutine(TweenPanel());
                }
            }

            if (hideSeams)
            {
                // Hide the individual parts
                piecesContainer?.SetActive(false);

                // Hide seams image
                background.GetComponent<Image>().enabled = false;
                // Increase opacity of background to max
                foreach (var backgroundImage in background.GetComponentsInChildren<Image>())
                {
                    backgroundImage.color = Color.white;
                }

                if (showFullImage)
                {
                    StartCoroutine(TweenBackgroundImage());
                }
            }
        }

        private IEnumerator TweenPanel()
        {
            var previousPosition = panel.transform.localPosition;
            var destinationPosition = new Vector3(previousPosition.x - 300, previousPosition.y, 0);
            float time = 0.0f;
            do
            {
                time += Time.deltaTime;
                panel.transform.localPosition = Vector3.Lerp(previousPosition, destinationPosition, time / showPanelSlideDuration);
                yield return 0;
            } while (time < showPanelSlideDuration);
        }

        private IEnumerator TweenBackgroundImage()
        {
            var previousScale = background.transform.localScale;
            var destinationScale = new Vector3(1.1f, 1.1f, 1.1f);
            float time = 0.0f;
            do
            {
                time += Time.deltaTime;
                background.transform.localScale = Vector3.Lerp(previousScale, destinationScale, time / showFullImageScaleDuration);
                yield return 0;
            } while (time < showFullImageScaleDuration);
        }
    }
}