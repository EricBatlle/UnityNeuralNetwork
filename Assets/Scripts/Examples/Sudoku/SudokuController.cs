using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sudoku;

public class SudokuController : MonoBehaviour
{
    public Sudoku sudokuModel = null;
    //ToDo: UI creations go here, but IDK how to do it right now
    public int cellID;
    public int newValue;

    [ContextMenu("Chang")]
    private void ChangeValue()
    {
        if (newValue <= sudokuModel.cellsInSquare)        
            sudokuModel.GetCellFromID(cellID).CellValue = newValue;        
        else        
            Debug.Log("<b>ERROR:</b> Can't set a higher number than " + sudokuModel.cellsInSquare);        
    }
}
