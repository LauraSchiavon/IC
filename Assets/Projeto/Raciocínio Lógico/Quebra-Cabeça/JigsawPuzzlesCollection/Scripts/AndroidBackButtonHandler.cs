#if EASY_MOBILE
using EasyMobile;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JigsawPuzzlesCollection.Scripts
{
    public class AndroidBackButtonHandler : Singleton<AndroidBackButtonHandler>
    {
        [Header("Exit Game Dialog")]
        public string gameTitle = "Exit Game";
        public string gameMessage = "Are you sure you want to exit?";
        public string gameYesButton = "Yes";
        public string gameNoButton = "No";

        [Header("Exit Level Dialog")]
        public string levelTitle = "Exit Level";
        public string levelMessage = "Are you sure you want to exit?";
        public string levelYesButton = "Yes";
        public string levelNoButton = "No";

        private float m_lastPressTime = 0;
        void Update()
        {
            // Exit on Android Back button
            if (Input.GetKeyUp(KeyCode.Escape))
            {
#if UNITY_ANDROID && EASY_MOBILE
                string title;
                string message;
                string yesButton;
                string noButton;
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    title = levelTitle;
                    message = levelMessage;
                    yesButton = levelYesButton;
                    noButton = levelNoButton;
                }
                else
                {
                    title = gameTitle;
                    message = gameMessage;
                    yesButton = gameYesButton;
                    noButton = gameNoButton;
                }

                NativeUI.AlertPopup alert = NativeUI.ShowTwoButtonAlert(
                    title,
                    message,
                    yesButton, 
                    noButton
                );

                if (alert != null)
                {
                    alert.OnComplete += button =>
                    {
                        switch (button)
                        {
                            case 0: // Yes
                                if (SceneManager.GetActiveScene().name == "Game")
                                {
                                    SceneManager.LoadScene("Menu");
                                }
                                else
                                {
                                    Application.Quit();
                                }
                                break;
                            case 1: // No
                                break;
                        }
                    };
                }     
#else
                if (Time.time - m_lastPressTime < 1.0f)
                {
                    if (SceneManager.GetActiveScene().name == "Game")
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    else
                    {
                        Application.Quit();
                    }
                }
                else
                {
                    m_lastPressTime = Time.time;
                }
#endif
            }
        }
    }
}
