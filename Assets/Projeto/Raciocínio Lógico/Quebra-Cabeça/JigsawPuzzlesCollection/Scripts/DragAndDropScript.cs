using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace JigsawPuzzlesCollection.Scripts
{
    public class DragAndDropScript : MonoBehaviour
    {
        public Image board;
        public Image Background;
        public EndGameScript EndGameScript;

        [Header("Hints")]
        public bool provideHints;
        public int numAllowedHints = -1;
        public Button hintButton;

        [Header("Reshuffle")]
        public bool allowReshuffle;
        public int numAllowedReshuffles = -1;
        public Button reshuffleButton;

        private GameObject m_selectedPiece;
        private int m_sortingOrder = 40;
        private readonly List<PieceScript> m_pieces = new List<PieceScript>();

        public void Start()
        {
            ConfigureHint();
            ConfigureReshuffle();
            var selectedLevel = LevelSelectorScript.Instance?.SelectedLevel;

            if (board)
            {
                var boardImageIndex = GameplayManager.Instance.SelectedBoard();
                board.sprite = GameplayManager.Instance.Boards[boardImageIndex];
            }

            if (GameplayManager.Instance.ShowBackground())
            {
                if (selectedLevel != null)
                {
                    Background.overrideSprite = selectedLevel.Image;
                }
            }
            else
            {
                Background.sprite = null;
                Background.overrideSprite = null;
            }

            var pieces = FindObjectsOfType<PieceScript>();
            int index = 0;
            foreach (var piece in pieces)
            {
                var puzzleObject = piece.gameObject.transform.Find("Puzzle");
                var spriteRenderer = puzzleObject.GetComponent<SpriteRenderer>();
                if (selectedLevel != null)
                {
                    spriteRenderer.sprite = selectedLevel.Image;
                }

                var sortingGroup = piece.GetComponent<SortingGroup>();
                sortingGroup.sortingOrder = (index++) + 1;

                m_pieces.Add(piece);
            }

            StartCoroutine(EnableMenuOnGameStart());
        }

        private void ConfigureHint()
        {
            if (hintButton != null)
            {
                hintButton.gameObject.SetActive(provideHints);
                hintButton.onClick.AddListener(() =>
                {
                    if (numAllowedHints > 0)
                    {
                        numAllowedHints--;
                    }

                    var unplacedPieces = m_pieces.Where(p => !p.InRightPosition).ToArray();
                    int index = Random.Range(0, unplacedPieces.Length);

                    var piece = unplacedPieces[index];
                    piece.MoveToRightPosition();

                    if (numAllowedHints == 0)
                    {
                        hintButton.interactable = false;
                    }
                });
            }
        }

        private void ConfigureReshuffle()
        {
            if (reshuffleButton != null)
            {
                reshuffleButton.gameObject.SetActive(allowReshuffle);
                reshuffleButton.onClick.AddListener(() =>
                {
                    if (numAllowedReshuffles > 0)
                    {
                        numAllowedReshuffles--;
                    }

                    var unplacedPieces = m_pieces.Where(p => !p.InRightPosition);
                    foreach (var piece in unplacedPieces)
                    {
                        piece.MoveToRandomPosition();
                    }

                    if (numAllowedReshuffles == 0)
                    {
                        reshuffleButton.interactable = false;
                    }
                });
            }
        }

        private IEnumerator EnableMenuOnGameStart()
        {
            if (hintButton)
            {
                hintButton.interactable = false;
            }
            if (reshuffleButton)
            {
                reshuffleButton.interactable = false;
            }

            while (m_pieces.Any(p => !p.IsMovable))
            {
                yield return new WaitForSeconds(0.2f);
            }

            if (hintButton)
            {
                hintButton.interactable = true;
            }
            if (reshuffleButton)
            {
                reshuffleButton.interactable = true;
            }
        }

        public void Update()
        {
            if (m_selectedPiece == null && Input.GetMouseButtonDown(0))
            {
                List<RaycastHit2D> results = new List<RaycastHit2D>();
                var hitCount = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, new ContactFilter2D(), results);

                if (hitCount > 0)
                {
                    var puzzlePieces = results.Where(r => r.transform.CompareTag("Puzzle"));
                    foreach (var puzzlePiece in puzzlePieces.OrderByDescending(x => x.transform.GetComponent<SortingGroup>().sortingOrder))
                    {
                        var pieceScript = puzzlePiece.transform.GetComponent<PieceScript>();
                        if (!pieceScript.InRightPosition && pieceScript.IsMovable)
                        {
                            m_selectedPiece = puzzlePiece.transform.gameObject;

                            var sortingGroup = m_selectedPiece.GetComponent<SortingGroup>();
                            sortingGroup.sortingOrder = m_sortingOrder++; // Move to the top

                            pieceScript.Selected = true;

                            break;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (m_selectedPiece != null)
                {
                    m_selectedPiece.GetComponent<PieceScript>().Selected = false;
                    m_selectedPiece = null;
                }
            }

            if (m_selectedPiece != null)
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, m_selectedPiece.transform.position.z);
            }

            if (m_pieces.All(p => p.InRightPosition))
            {
                EndGameScript.EndGame();
                // Disable this script - we no longer want to allow pieces to be moved
                enabled = false;
            }
        }
    }
}