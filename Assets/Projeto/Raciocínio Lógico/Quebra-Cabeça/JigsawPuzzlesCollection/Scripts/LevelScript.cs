using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JigsawPuzzlesCollection.Scripts
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class LevelScript : MonoBehaviour
    {
        private Level m_level;

        public void SetDetails(Level level)
        {
            m_level = level;

            var cardContentElement = transform.Find("CardContent");

            var imageElement = cardContentElement.Find("Image");
            imageElement.GetComponent<Image>().sprite = level.Image;

            var button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                LevelSelectorScript.Instance.SelectedLevel = level;
                SceneManager.LoadScene("Game");
            });
        }
    }
}