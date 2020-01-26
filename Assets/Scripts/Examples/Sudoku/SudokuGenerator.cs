using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SudokuGenerator : MonoBehaviour
{
    [Header("Sudoku Design Info")]
    public int cellsInSquare = 4;
    [SerializeField] private int[] newSudokuSequence = null;
    [SerializeField] private bool isRandomlyGenerated = false;
    [Header("UI References")]    
    [SerializeField] private GameObject canvasGO = null;
    [Header("Cell Colors")]
    public Color color_Invalid = Color.red;
    public Color color_Valid = Color.green;
    public Color color_Empty = Color.yellow;
    public Color color_OriginalCell = Color.blue;
    [Header("Prefabs")]
    [SerializeField] private GameObject sudokuContainer_prefab = null;
    [SerializeField] private GameObject sudokuGrid_prefab = null;
    [SerializeField] private GameObject sudokuCell_prefab = null;

    public int sudokuCount = 0;    

    [ContextMenu("Generate Sudoku")]
    public void GenerateSudoku()
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
                    newCell.GetComponent<SudokuCell>().sudokuGenerator = this;
                    newCell.GetComponent<SudokuCell>().Model = newSudoku.allSudokuCells[t];
                }
            }
        }
    }
    public SudokuController GenerateSudokuAndGetController()
    {
        //ClearSudokuContainer();
        GameObject containerGO = (GameObject)PrefabUtility.InstantiatePrefab(sudokuContainer_prefab);
        containerGO.transform.SetParent(canvasGO.transform, false);
        Vector3 newContainerPosition = containerGO.GetComponent<RectTransform>().position;
        newContainerPosition.x = newContainerPosition.x + ((1920 + 50)*sudokuCount);
        containerGO.GetComponent<RectTransform>().position = newContainerPosition;
        sudokuCount++;
        
        //ToDo: Control that the numbers from newSudokuSequence are < squareCells
        Sudoku newSudoku = new Sudoku(cellsInSquare, newSudokuSequence, isRandomlyGenerated);
        containerGO.GetComponent<SudokuController>().sudokuModel = newSudoku;
        containerGO.GetComponent<SudokuController>().sudokuContainer = containerGO;

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
                    newCell.GetComponent<SudokuCell>().sudokuGenerator = this;
                    newCell.GetComponent<SudokuCell>().Model = newSudoku.allSudokuCells[t];
                }
            }
        }

        return containerGO.GetComponent<SudokuController>();
    }

    [ContextMenu("Clear SudokuContainer")]
    private void ClearSudokuContainer()
    {
        List<Transform> tempList = canvasGO.transform.Cast<Transform>().ToList();
        foreach (Transform child in tempList)
            DestroyImmediate(child.gameObject);        
    }    
}
