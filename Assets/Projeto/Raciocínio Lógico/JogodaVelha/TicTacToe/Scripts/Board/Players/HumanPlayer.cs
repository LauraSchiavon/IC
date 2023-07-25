using UnityEngine;
using Broniek.Stuff.Sounds;

namespace TicTacToeWithAI.Board
{
    // Support for actions taken by a live player.

    public class HumanPlayer : MonoBehaviour
    {
        public static int playerId = 0;
        public static bool selfPlay;    // working when PlayerAI has playerId = -1, and PlayerHuman has playerId = 0

        public static int row;          // coordinates of the player's last move
        public static int col;

        private bool[,] board;
        private Transform boardTransform;

        private void Start()
        {
            boardTransform = BoardCreator.Instance.transform;
            board = (playerId == 0) ? BoardCreator.circleBoard : BoardCreator.crossBoard;
        }

        private void Update()
        {
            if (!GameController.gameOver)
                if (GameController.round == playerId)
                    if (Input.GetMouseButtonDown(0))
                        PlacePattern();
        }

        private void PlacePattern()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider.CompareTag("Tile"))
                {
                    col = Mathf.RoundToInt(hitInfo.point.x) + 1; // ransformation of board fields into numbering: row, column
                    row = 1 - Mathf.RoundToInt(hitInfo.point.y);

                    board[row, col] = true;

                    GameController.moves++;

                    GameController.round = selfPlay ? playerId : (++GameController.round) % 2;
                    int no = selfPlay ? GameController.moves % 2 : playerId;

                    hitInfo.collider.transform.GetChild(no).gameObject.SetActive(true);
                    hitInfo.collider.enabled = false;

                    SoundManager.GetSoundEffect(1, 0.5f);

                    if (selfPlay)                           // only playing alone with ourself
                        if (CheckIfOver())                  // check if AI wins
                        {
                            GameController.gameOver = true;
                            SoundManager.GetSoundEffect(0, 1f, 0.5f);
                        }
                }
            }
        }

        private bool CheckIfOver()
        {
            for (int k = 0; k < 2; k++)     // for each symbol
            {
                int amount3 = 0;
                int amount4 = 0;

                for (int i = 0; i < 3; i++)
                {
                    int amount1 = 0;
                    int amount2 = 0;

                    for (int j = 0; j < 3; j++)
                    {
                        if (boardTransform.GetChild(j + 3 * i).GetChild(k).gameObject.activeSelf)   // horizontally
                            amount1++;

                        if (boardTransform.GetChild(i + 3 * j).GetChild(k).gameObject.activeSelf)   // vertically
                            amount2++;
                    }

                    if (boardTransform.GetChild(4 * i).GetChild(k).gameObject.activeSelf)           // diagonally
                        amount3++;

                    if (boardTransform.GetChild(2 * i + 2).GetChild(k).gameObject.activeSelf)       // diagonally
                        amount4++;

                    if (amount1 == 3 || amount2 == 3)
                        return true;
                }

                if (amount3 == 3 || amount4 == 3)
                    return true;
            }

            return false;
        }
    }
}