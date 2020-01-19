using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Sudoku
{
    #region Sudoku Components (Row,Column,Grid,Cell) Classes
    [Serializable]
    public class Row
    {
        public List<int> rowNumbers;
        public List<int> rowCellIDs;
        [SerializeField] private bool isAnyValueRepeated;
        public bool IsAnyValueRepeated
        {
            get => rowNumbers.ThereAreRepeatedValuesOnList();
            set => isAnyValueRepeated = value;
        }        
    }
    [Serializable]
    public class Column
    {
        public List<int> columnNumbers;        
        public List<int> columnCellIDs;
        [SerializeField] private bool isAnyValueRepeated;        
        public bool IsAnyValueRepeated
        {
            get => columnNumbers.ThereAreRepeatedValuesOnList();
            set => isAnyValueRepeated = value;
        }        
    }
    [Serializable]
    public class SquareGrid
    {
        public int id;
        public List<int> contentNumbers;
        public List<int> gridCellIDs;
        [SerializeField] private bool isAnyValueRepeated;
        public bool IsAnyValueRepeated
        {
            get => contentNumbers.ThereAreRepeatedValuesOnList();
            set => isAnyValueRepeated = value;
        }
    }
    [Serializable]
    public class SudokuCellModel
    {
        public int id;
        public SudokuCell cellComponent = null;
        [SerializeField][Range(0, 9)] private int cellValue;
        public int CellValue
        {
            get => cellValue;
            set
            {
                cellValue = value;
                UpdateAssociatedFields(id, cellValue);
                cellComponent?.SetText();
            }
        }
        public Row belongingRow = new Row();
        public Column belongingColumn = new Column();
        public SquareGrid belongingGrid = new SquareGrid();

        private void UpdateAssociatedFields(int cellID, int newValue)
        {            
            if (belongingRow.rowNumbers != null)
                if(belongingRow.rowCellIDs.FindIndex(x => x == cellID) != -1)                
                    belongingRow.rowNumbers[belongingRow.rowCellIDs.FindIndex(x => x == cellID)] = newValue;                    

            if (belongingColumn.columnNumbers != null)
                if(belongingColumn.columnCellIDs.FindIndex(x => x == cellID) != -1)
                    belongingColumn.columnNumbers[belongingColumn.columnCellIDs.FindIndex(x => x == cellID)] = newValue;

            if (belongingGrid.contentNumbers != null)            
                if(belongingGrid.gridCellIDs.FindIndex(x => x == cellID) != -1)
                    belongingGrid.contentNumbers[belongingGrid.gridCellIDs.FindIndex(x => x == cellID)] = newValue;

            CheckRepeatedValues();
        }

        public void CheckRepeatedValues()
        {
            belongingRow.IsAnyValueRepeated = belongingRow.IsAnyValueRepeated;
            belongingColumn.IsAnyValueRepeated = belongingColumn.IsAnyValueRepeated;
        }
    }
    #endregion

    [SerializeField] public int cellsInSquare = 4;
    [SerializeField] [DisplayWithoutEdit] public int cellsInSquareSide = 2;
    [SerializeField] public int[] allSudokuNumbers = new int[16];
    [SerializeField] public SudokuCellModel[] allSudokuCells = new SudokuCellModel[16];
    [SerializeField] public List<Row> rows = new List<Row>();
    [SerializeField] public List<Column> columns = new List<Column>();
    [SerializeField] public List<SquareGrid> grids = new List<SquareGrid>();    

    public Sudoku(int cellsInSquare)
    {
        this.cellsInSquare = cellsInSquare;
        InitializeGridsAndCellsStructs();
        SetRepeatedValues();
    }
    
    private void SetRepeatedValues()
    {
        foreach (SudokuCellModel cell in allSudokuCells)
        {
            cell.CheckRepeatedValues();
        }
    }
    
    public SudokuCellModel GetCellFromID(int cellID)
    {
        for (int i = 0; i < allSudokuCells.Length; i++)
        {
            if (allSudokuCells[i].id == cellID)
                return allSudokuCells[i];
        }
        Debug.Log("<b>ERROR</b>: This ID do not belong to any cell");
        return new SudokuCellModel();
    }

    private void InitializeGridsAndCellsStructs()
    {
        cellsInSquareSide = (int)Mathf.Sqrt(cellsInSquare);

        allSudokuNumbers = new int[cellsInSquare * cellsInSquare];
        allSudokuCells = new SudokuCellModel[cellsInSquare * cellsInSquare];

        rows.Clear();
        columns.Clear();
        grids.Clear();

        //Generate all numbers
        for (int i = 0; i < allSudokuNumbers.Length; i++)
        {
            allSudokuNumbers[i] = UnityEngine.Random.Range(0, 9);
            //allSudokuNumbers[i] = i;

            SudokuCellModel sudokuCell = new SudokuCellModel();
            sudokuCell.belongingRow = new Row();
            sudokuCell.belongingColumn = new Column();
            sudokuCell.belongingGrid = new SquareGrid();
            sudokuCell.belongingGrid.contentNumbers = new List<int>();
            sudokuCell.belongingGrid.gridCellIDs = new List<int>();
            sudokuCell.CellValue = allSudokuNumbers[i];
            sudokuCell.id = i;
            allSudokuCells[i] = sudokuCell;
        }

        //Generate rows        
        for (int u = 0; u < allSudokuNumbers.Length; u += cellsInSquare)
        {
            Row row = new Row();
            row.rowNumbers = new List<int>();
            row.rowCellIDs = new List<int>();
            for (int i = 0; i < allSudokuNumbers.Length / cellsInSquare; i++)
            {
                row.rowNumbers.Add(allSudokuNumbers[i + u]);
                row.rowCellIDs.Add(allSudokuCells[i + u].id);
            }
            allSudokuCells[u].belongingRow = row;
            rows.Add(row);
        }

        //Generate columns
        for (int u = 0; u < cellsInSquare; u++)
        {
            Column column = new Column();
            column.columnNumbers = new List<int>();
            column.columnCellIDs = new List<int>();
            for (int i = 0; i < allSudokuNumbers.Length; i++)
            {
                //every cellsInSquare
                if (i % cellsInSquare == 0)
                {
                    column.columnNumbers.Add(allSudokuNumbers[i + u]);
                    column.columnCellIDs.Add(allSudokuCells[i + u].id);
                }
            }
            columns.Add(column);
            allSudokuCells[u].belongingColumn = column;
        }

        //Generate GridsOfcellsInSquare
        for (int m = 0; m < cellsInSquareSide; m++)
        {
            for (int t = 0; t < cellsInSquareSide; t++)
            {
                SquareGrid squareGrid = new SquareGrid();
                squareGrid.id = (m + t * cellsInSquareSide);
                squareGrid.contentNumbers = new List<int>();
                squareGrid.gridCellIDs = new List<int>();
                for (int i = 0; i < cellsInSquareSide; i++)
                {
                    for (int u = 0; u < cellsInSquareSide; u++)
                    {
                        squareGrid.contentNumbers.Add(allSudokuNumbers[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)]);
                        squareGrid.gridCellIDs.Add(allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].id);

                        allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].belongingGrid.contentNumbers = squareGrid.contentNumbers;
                        allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].belongingGrid.gridCellIDs = squareGrid.gridCellIDs;
                        allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].belongingGrid.id = squareGrid.id;
                    }
                }
                grids.Add(squareGrid);
            }
        }
    }

}
