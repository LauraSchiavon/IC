using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TicTacToeWithAI.Board
{
    // Operation of the menu present in TicTacToe scene and support for the most important game parameters.

    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button newGameBtn;
        [SerializeField] private Button exitGameBtn;

        public static int round;
        public static bool win;

        public static int
            moves; // the number of moves made in the game by both players in total (number of pieces on the board)

        public static bool gameOver;

        public void NewGame()
        {
            gameOver = false;
            var scene = SceneManager.LoadSceneAsync("Board");
            scene.allowSceneActivation = true;
        }

        public void ExitGame()
        {
            moves = 0;
            round = 0;
            gameOver = false;
            var scene = SceneManager.LoadSceneAsync("MainMenu");
            scene.allowSceneActivation = true;
        }

        private void Update()
        {
            if (gameOver || moves >= 9)
                PrepareExit();
        }

        private void PrepareExit()
        {
            gameOver = true;
            moves = 0;
            round = 0;
            newGameBtn.transform.parent.gameObject.SetActive(true);
        }
    }
}