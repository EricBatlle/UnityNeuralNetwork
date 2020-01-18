using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SudokuGenerator : MonoBehaviour
{
    #region Grid and Cell Structs
    [Serializable]
    public struct Row
    {
        public List<int> rowNumbers;
    }
    [Serializable]
    public struct Column
    {
        public List<int> columnNumbers;
    }
    [Serializable]
    public struct SquareGrid
    {
        public int id;
        public List<int> contentNumbers;
    }
    [Serializable]
    public struct SudokuCellStruct
    {
        public int id;
        [Range(0, 9)] public  int value;
        public Row belongingRow;
        public Column belongingColumn;
        public SquareGrid belongingGrid;
    }
    #endregion

    [Header("Grid Info")]
    [SerializeField] private int cellsInSquare = 4;
    [SerializeField][DisplayWithoutEdit] private int cellsInSquareSide = 2;
    [SerializeField] private int[] allSudokuNumbers = new int[16];
    [SerializeField] private SudokuCellStruct[] allSudokuCells = new SudokuCellStruct[16];
    [SerializeField] private List<Row> rows = new List<Row>();
    [SerializeField] private List<Column> columns = new List<Column>();
    [SerializeField] private List<SquareGrid> grids = new List<SquareGrid>();
    [Header("UI References")]    
    [SerializeField] private GameObject sudokuContainerGO = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject sudokuGrid_prefab = null;
    [SerializeField] private GameObject sudokuCell_prefab = null;


    private void Start()
    {
        InitializeGridsAndCellsStructs();
        GenerateGrid();
    }

    [ContextMenu("Initialize GridAndCell Structs")]
    private void InitializeGridsAndCellsStructs()
    {
        cellsInSquareSide = (int)Mathf.Sqrt(cellsInSquare);

        allSudokuNumbers = new int[cellsInSquare * cellsInSquare];
        allSudokuCells = new SudokuCellStruct[cellsInSquare * cellsInSquare];

        rows.Clear();
        columns.Clear();
        grids.Clear();

        //Generate all numbers
        for (int i = 0; i < allSudokuNumbers.Length; i++)
        {
            allSudokuNumbers[i] = i;

            SudokuCellStruct sudokuCell = new SudokuCellStruct();
            sudokuCell.belongingRow = new Row();
            sudokuCell.belongingColumn = new Column();
            sudokuCell.belongingGrid = new SquareGrid();
            sudokuCell.belongingGrid.contentNumbers = new List<int>();
            sudokuCell.value = i;
            sudokuCell.id = i;
            allSudokuCells[i] = sudokuCell;
        }

        //Generate rows        
        for (int u = 0; u < allSudokuNumbers.Length; u+= cellsInSquare)
        {
            Row row = new Row();
            row.rowNumbers = new List<int>();
            for (int i = 0; i < allSudokuNumbers.Length/ cellsInSquare; i++)
            {                               
                row.rowNumbers.Add(allSudokuNumbers[i+u]);
            }
            allSudokuCells[u].belongingRow = row;
            rows.Add(row);
        }        

        //Generate columns
        for (int u = 0; u < cellsInSquare; u++)
        {
            Column column = new Column();
            column.columnNumbers = new List<int>();
            for (int i = 0; i < allSudokuNumbers.Length; i++)
            {
                //every cellsInSquare
                if (i % cellsInSquare == 0)
                    column.columnNumbers.Add(allSudokuNumbers[i + u]);                                   
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
                for (int i = 0; i < cellsInSquareSide; i++)
                {
                    for (int u = 0; u < cellsInSquareSide; u++)
                    {
                        squareGrid.contentNumbers.Add(allSudokuNumbers[((i) * cellsInSquare) + (u+(m* cellsInSquareSide * cellsInSquare)) + (t* cellsInSquareSide)]);

                        allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].belongingGrid.contentNumbers = squareGrid.contentNumbers;
                        allSudokuCells[((i) * cellsInSquare) + (u + (m * cellsInSquareSide * cellsInSquare)) + (t * cellsInSquareSide)].belongingGrid.id = squareGrid.id;
                    }
                }
                grids.Add(squareGrid);
            }
        }
    }

    [ContextMenu("Generate Grid")]
    private void GenerateGrid()
    {
        //Change Container layout parameters
        FlexibleGridLayoutGroup containerLayout = sudokuContainerGO.GetComponent<FlexibleGridLayoutGroup>();
        containerLayout.rows = cellsInSquareSide;
        containerLayout.cols = containerLayout.rows; //cause it's an square        
        //Change Grid prefab layout parameters
        FlexibleGridLayoutGroup gridPrefabLayout = sudokuGrid_prefab.GetComponent<FlexibleGridLayoutGroup>();
        gridPrefabLayout.rows = cellsInSquareSide;
        gridPrefabLayout.cols = containerLayout.rows; //cause it's an square

        //Create grids
        for (int i = 0; i < grids.Count; i++)
        {
            GameObject newGrid = Instantiate(sudokuGrid_prefab);
            newGrid.GetComponent<SudokuGrid>().model = grids[i];
            newGrid.transform.SetParent(sudokuContainerGO.transform);
            //foreach grid create his cells            
            for (int t = 0; t < allSudokuCells.Length; t++)
            {
                //Find the belonging cell of the grid
                if(grids[i].id == allSudokuCells[t].belongingGrid.id)
                {
                    GameObject newCell = Instantiate(sudokuCell_prefab);
                    newCell.transform.SetParent(newGrid.transform);
                    newCell.GetComponent<SudokuCell>().Model = allSudokuCells[t];
                }
            }
        }
    }

    [ContextMenu("Clear SudokuContainer")]
    private void ClearSudokuContainer()
    {
        List<Transform> tempList = sudokuContainerGO.transform.Cast<Transform>().ToList();
        foreach (Transform child in tempList)
            DestroyImmediate(child.gameObject);        
    }
    
}
