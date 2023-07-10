using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
    public Text cellPrefab; // Prefab para as células do Sudoku

    private const int boardSize = 9; // Tamanho do tabuleiro de Sudoku
    private const int subgridSize = 3; // Tamanho da subgrade de Sudoku

    private Text[,] cells; // Matriz para armazenar as células do Sudoku

    private void Start()
    {
        // Inicialize a matriz de células do Sudoku
        cells = new Text[boardSize, boardSize];

        // Crie as células do Sudoku
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                cells[row, col] = Instantiate(cellPrefab, transform);
                cells[row, col].text = "0"; // Altere para um valor inicial desejado
            }
        }
    }
}
