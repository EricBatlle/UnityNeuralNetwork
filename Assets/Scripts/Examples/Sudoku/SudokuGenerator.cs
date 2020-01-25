using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SudokuGenerator : MonoBehaviour
{
    [Header("Sudoku Design Info")]
    [SerializeField] private int cellsInSquare = 4;
    [SerializeField] private int[] newSudokuSequence = null;
    [SerializeField] private bool isRandomlyGenerated = false;
    [Header("UI References")]    
    [SerializeField] private GameObject canvasGO = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject sudokuContainer_prefab = null;
    [SerializeField] private GameObject sudokuGrid_prefab = null;
    [SerializeField] private GameObject sudokuCell_prefab = null;

    [ContextMenu("Generate Sudoku")]
    private void GenerateSudoku()
    {
        ClearSudokuContainer();

        GameObject containerGO = (GameObject)PrefabUtility.InstantiatePrefab(sudokuContainer_prefab);
        containerGO.transform.SetParent(canvasGO.transform, false);
        
        //ToDo: Control that the numbers from newSudokuSequence are < squareCells
        Sudoku newSudoku = new Sudoku(cellsInSquare, newSudokuSequence, isRandomlyGenerated);
        containerGO.GetComponent<SudokuController>().sudokuModel = newSudoku;        

        //Change Container layout parameters
        FlexibleGridLayoutGroup containerLayout = containerGO.GetComponent<FlexibleGridLayoutGroup>();
        containerLayout.rows = newSudoku.cellsInSquareSide;
        containerLayout.cols = newSudoku.cellsInSquareSide; //cause it's an square        
        //Change Grid prefab layout parameters
        FlexibleGridLayoutGroup gridPrefabLayout = sudokuGrid_prefab.GetComponent<FlexibleGridLayoutGroup>();
        gridPrefabLayout.rows = newSudoku.cellsInSquareSide;
        gridPrefabLayout.cols = newSudoku.cellsInSquareSide; //cause it's an square

        //Create grids
        for (int i = 0; i < newSudoku.grids.Count; i++)
        {
            GameObject newGrid = (GameObject)PrefabUtility.InstantiatePrefab(sudokuGrid_prefab);
            newGrid.GetComponent<SudokuGrid>().model = newSudoku.grids[i];
            newGrid.transform.SetParent(containerGO.transform);
            //foreach grid create his cells            
            for (int t = 0; t < newSudoku.allSudokuCells.Length; t++)
            {
                //Find the belonging cell of the grid
                if(newSudoku.grids[i].id == newSudoku.allSudokuCells[t].belongingGrid.id)
                {
                    GameObject newCell = (GameObject)PrefabUtility.InstantiatePrefab(sudokuCell_prefab);
                    newCell.transform.SetParent(newGrid.transform);
                    newSudoku.allSudokuCells[t].cellComponent = newCell.GetComponent<SudokuCell>();
                    newCell.GetComponent<SudokuCell>().Model = newSudoku.allSudokuCells[t];                    
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
