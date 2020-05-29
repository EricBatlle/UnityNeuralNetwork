using System.Collections.Generic;
using UnityEngine;
using static Sudoku;

/// <summary>
/// Derived class from Agent. 
/// His goal is complete correctly the sudoku
/// </summary>
public class SudokuAgent : Agent
{
    [SerializeField] private SudokuController sudokuController = null;

    [SerializeField] private int cellIdToModify = 0;
    [SerializeField] private int newCellValue = 0;

    private List<SudokuCellModel> availableCellsID = new List<SudokuCellModel>();

    public override void Init(NeuralNetwork net, params object[] info)
    {
        base.Init(net, info);
        sudokuController = (SudokuController)info[0];

        //Calculate only the available CELLS
        availableCellsID.Clear();
        foreach (SudokuCellModel cellModel in sudokuController.sudokuModel.allSudokuCells)
        {
            if (!cellModel.isOriginalCell)
                availableCellsID.Add(cellModel);
        }
    }

    //ToDo: sudoku's state maybe should change as they will be more?
    protected override void LoadInfo()
    {
        SudokuManager manager = (SudokuManager)FindObjectOfType(typeof(SudokuManager));
        sudokuController = manager.sudokuController;
    }

    protected override void CollectEnvironmentInformation()
    {
        //Get Sudoku information, as it is managed from sudokuController, there is no need        
    }

    protected override void SetNewInputs()
    {
        this.inputs = new float[sudokuController.sudokuModel.allSudokuCells.Length];
        for (int i = 0; i < sudokuController.sudokuModel.allSudokuCells.Length; i++)
        {
            inputs[i] = sudokuController.sudokuModel.allSudokuCells[i].CellValue;
        }
    }

    protected override void AgentAction()
    {
        int availableCellsIDIndex = outputs[0].ConvertToIntegerOnRange(1,-1,0, availableCellsID.Count-1);
        cellIdToModify = availableCellsID[availableCellsIDIndex].id;

        newCellValue = outputs[1].ConvertToIntegerOnRange(1,-1,0, sudokuController.sudokuModel.cellsInSquare);

        sudokuController.ChangeCellValue(cellIdToModify, newCellValue);
        MoveAgentToCell(cellIdToModify);        
    }

    private void OnDestroy()
    {
        GameObject.Destroy(sudokuController.sudokuContainer);
    }

    protected override float CalculateFitnessGain()
    {
        float newFitness = 0f;

        //Por cada celda no original
        foreach(SudokuCellModel cell in sudokuController.sudokuModel.allSudokuCells)
        {
            if(!cell.isOriginalCell)
            {
                switch (cell.cellState)
                {
                    case CellState.Valid:
                        newFitness += 10;
                        break;
                    case CellState.Invalid:
                        newFitness -= 0.5f;
                        break;
                    case CellState.Empty:
                        newFitness -= 1f;
                        break;
                    default:
                        break;
                }
            }            
        }
        return 1 + newFitness;
    }    

    private void MoveAgentToCell(int cellID)
    {
        this.gameObject.transform.position = sudokuController.sudokuModel.GetCellFromID(cellID).cellComponent.GetComponent<RectTransform>().position;
    }
}