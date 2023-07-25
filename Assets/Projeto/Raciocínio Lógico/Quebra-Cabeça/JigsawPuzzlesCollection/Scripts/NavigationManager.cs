using UnityEngine;
using UnityEngine.SceneManagement;

namespace JigsawPuzzlesCollection.Scripts
{
    public class NavigationManager : MonoBehaviour
    {
        public string MoreGamesGoogleStore;
        public string MoreGamesAppleStore;

        public void BackToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void GoToMoreGames()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    if (!string.IsNullOrEmpty(MoreGamesAppleStore))
                    {
                        Application.OpenURL(MoreGamesAppleStore);
                    }
                    break;

                case RuntimePlatform.Android:
                    if (!string.IsNullOrEmpty(MoreGamesGoogleStore))
                    {
                        Application.OpenURL(MoreGamesGoogleStore);
                    }
                    break;
            }
        }
    }
}