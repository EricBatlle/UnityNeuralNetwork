using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sudoku;

public class SudokuController : MonoBehaviour
{
    public Sudoku sudokuModel = null;
    [Header("UI References")]    
    public GameObject sudokuContainer = null;

    [Header("Debug Purpouse")]
    public int cellID;
    public int newValue;

    /// <summary>
    /// Change the value from Editor
    /// </summary>
    [ContextMenu("ChangeCellValue")]
    private void ChangeCellValue()
    {        
        if (newValue <= sudokuModel.cellsInSquare)
        {
            SudokuCellModel cellModelToChange = sudokuModel.GetCellFromID(cellID);
            if (!cellModelToChange.isOriginalCell)
                cellModelToChange.CellValue = newValue;
            else
                Debug.Log("<b>ERROR:</b> Can't modify original Cells");
        }
        else        
            Debug.Log("<b>ERROR:</b> Can't set a higher number than " + sudokuModel.cellsInSquare);        
    }
    public void ChangeCellValue(int cellID, int newValue)
    {        
        if (newValue <= sudokuModel.cellsInSquare)
        {
            SudokuCellModel cellModelToChange = sudokuModel.GetCellFromID(cellID);
            if (!cellModelToChange.isOriginalCell)
                cellModelToChange.CellValue = newValue;
            else
                Debug.Log("<b>erroR:</b> Can't modify original Cells");
        }
        else        
            Debug.Log("<b>ERROR:</b> Can't set a higher number than " + sudokuModel.cellsInSquare);        
    }
}
