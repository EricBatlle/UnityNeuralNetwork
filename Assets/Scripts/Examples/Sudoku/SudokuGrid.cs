using UnityEngine;
using static Sudoku;

public class SudokuGrid : MonoBehaviour
{
    public SquareGrid model;

    private SudokuGrid(SquareGrid modelStruct)
    {
        this.model = modelStruct;
    }
}
