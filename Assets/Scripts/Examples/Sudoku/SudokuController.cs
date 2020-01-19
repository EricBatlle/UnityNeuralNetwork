using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sudoku;

public class SudokuController : MonoBehaviour
{
    public Sudoku sudokuModel = null;
    //ToDo: UI creations go here, but IDK how to do it right now
    //public SudokuCellStruct cell;

    [ContextMenu("Chang")]
    private void ChangeValue()
    {        
        //cell = sudokuModel.GetCellFromID(0);
        //cell.CellValue = Random.Range(0, 9);

        sudokuModel.GetCellFromID(0).CellValue = Random.Range(0, 9);
    }
}
