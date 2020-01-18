using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SudokuGenerator : MonoBehaviour
{
    [Header("Sudoku Design Info")]
    [SerializeField] private int cellsInSquare = 4;
    [Header("UI References")]    
    [SerializeField] private GameObject canvasGO = null;
    [Header("Prefabs")]
    [SerializeField] private GameObject sudokuContainer_prefab = null;
    [SerializeField] private GameObject sudokuGrid_prefab = null;
    [SerializeField] private GameObject sudokuCell_prefab = null;

    [ContextMenu("Generate Sudoku")]
    private void GenerateSudoku()
    {
        GameObject containerGO = Instantiate(sudokuContainer_prefab);
        containerGO.transform.SetParent(canvasGO.transform, false);

        Sudoku newSudoku = new Sudoku(cellsInSquare);
        containerGO.GetComponent<SudokuController>().sudokuModel = newSudoku;        

        //Change Container layout parameters
        FlexibleGridLayoutGroup containerLayout = containerGO.GetComponent<FlexibleGridLayoutGroup>();
        containerLayout.rows = newSudoku.cellsInSquareSide;
        containerLayout.cols = containerLayout.rows; //cause it's an square        
        //Change Grid prefab layout parameters
        FlexibleGridLayoutGroup gridPrefabLayout = sudokuGrid_prefab.GetComponent<FlexibleGridLayoutGroup>();
        gridPrefabLayout.rows = newSudoku.cellsInSquareSide;
        gridPrefabLayout.cols = containerLayout.rows; //cause it's an square

        //Create grids
        for (int i = 0; i < newSudoku.grids.Count; i++)
        {
            GameObject newGrid = Instantiate(sudokuGrid_prefab);
            newGrid.GetComponent<SudokuGrid>().model = newSudoku.grids[i];
            newGrid.transform.SetParent(containerGO.transform);
            //foreach grid create his cells            
            for (int t = 0; t < newSudoku.allSudokuCells.Length; t++)
            {
                //Find the belonging cell of the grid
                if(newSudoku.grids[i].id == newSudoku.allSudokuCells[t].belongingGrid.id)
                {
                    GameObject newCell = Instantiate(sudokuCell_prefab);
                    newCell.transform.SetParent(newGrid.transform);
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
