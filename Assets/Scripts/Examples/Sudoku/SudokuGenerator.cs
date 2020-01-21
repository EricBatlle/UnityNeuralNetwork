using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Sudoku;

[ExecuteInEditMode]
public class SudokuGenerator : MonoBehaviour
{
    

    [Header("Grid Info")]
    [SerializeField] private int cellsInSquare = 4;
    [SerializeField][DisplayWithoutEdit] private int cellsInSquareSide = 2;
    [SerializeField] private int[] allSudokuNumbers = new int[16];
    [SerializeField] private SudokuCellStruct[] allSudokuCells = new SudokuCellStruct[16];
    [SerializeField] private List<Row> rows = new List<Row>();
    [SerializeField] private List<Column> columns = new List<Column>();
    [SerializeField] private List<SquareGrid> grids = new List<SquareGrid>();
    [Header("UI References")]    
    [SerializeField] private GameObject canvasGO = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject sudokuContainer_prefab = null;
    [SerializeField] private GameObject sudokuGrid_prefab = null;
    [SerializeField] private GameObject sudokuCell_prefab = null;


    private void Start()
    {
        //InitializeGridsAndCellsStructs();
        GenerateGrid();
    }
   
    [ContextMenu("Generate Grid")]
    private void GenerateGrid()
    {
        Sudoku newSudoku = new Sudoku();    

        GameObject containerGO = Instantiate(sudokuContainer_prefab);
        containerGO.transform.SetParent(canvasGO.transform, false);
        containerGO.GetComponent<SudokuController>().sudoku = newSudoku;

        //Change Container layout parameters
        FlexibleGridLayoutGroup containerLayout = containerGO.GetComponent<FlexibleGridLayoutGroup>();
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
            newGrid.transform.SetParent(containerGO.transform);
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
        List<Transform> tempList = canvasGO.transform.Cast<Transform>().ToList();
        foreach (Transform child in tempList)
            DestroyImmediate(child.gameObject);        
    }
    
}
