using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorSudoku : MonoBehaviour
{
    public SudokuBoard[] sudokuBoards; // Matrizes de Sudoku pré-definidas para cada dificuldade
    public int currentDifficulty; // Dificuldade atual (0 = fácil, 1 = médio, 2 = difícil)

    private SudokuBoard currentBoard; // Tabuleiro de Sudoku atual

    private void Start()
    {
        // Inicialize o tabuleiro de Sudoku de acordo com a dificuldade atual
        currentBoard = Instantiate(sudokuBoards[currentDifficulty]);
    }
}
