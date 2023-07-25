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
        public static int moves;    // the number of moves made in the game by both players in total (number of pieces on the board)
        public static bool gameOver;

        private void Awake() => AddListeners();

        private void AddListeners()
        {
            newGameBtn.onClick.AddListener(NewGame);
            exitGameBtn.onClick.AddListener(ExitGame);
        }

        private void NewGame()
        {
            gameOver = false;
            SceneManager.LoadScene("TicTacToe");
        }

        private void ExitGame()
        {
            moves = 0;
            round = 0;
            gameOver = false;
            SceneManager.LoadScene("MainMenu");
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
            newGameBtn.gameObject.SetActive(true);
        }
    }
}