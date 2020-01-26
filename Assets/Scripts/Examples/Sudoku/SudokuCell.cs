using UnityEngine;
using UnityEngine.UI;
using static Sudoku;

public class SudokuCell : MonoBehaviour
{
    [SerializeField] private SudokuCellModel _model;
    public SudokuCellModel Model
    {
        get => _model;
        set
        {
            _model = value;
            UpdateUI();
        }
    }
    [Header("UI References")]
    [SerializeField] private Text textComponent = null;
    [SerializeField] private RawImage rawImgComponent = null;

    public SudokuGenerator sudokuGenerator = null;
   
    public void Init(SudokuCellModel modelStruct)
    {
        Model = modelStruct;
    }

    public void UpdateUI()
    {
        SetText();
        SetColor();
    }
    private void SetText()
    {
        this.textComponent.text = _model.CellValue.ToString();
    }
    private void SetColor()
    {
        if(Model.isOriginalCell)
            rawImgComponent.color = sudokuGenerator.color_OriginalCell;
        else
        {
            switch (Model.cellState)
            {
                case CellState.Invalid:
                    rawImgComponent.color = sudokuGenerator.color_Invalid;
                break;
                case CellState.Valid:
                    rawImgComponent.color = sudokuGenerator.color_Valid;
                    break;
                case CellState.Empty:
                    rawImgComponent.color = sudokuGenerator.color_Empty;
                    break;
                default:
                    Debug.Log("<b>ERROR:</b> There is no cellState associated");
                    break;
            }         

        }
    }
}
