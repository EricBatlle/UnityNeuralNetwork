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

    private SudokuCell(SudokuCellModel modelStruct)
    {
        _model = modelStruct;
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
        switch(Model.cellState)
        {
            case CellState.Invalid:
                rawImgComponent.color = Color.red;
            break;
            case CellState.Valid:
                rawImgComponent.color = Color.green;
                break;
            case CellState.Empty:
                rawImgComponent.color = Color.yellow;
                break;
            default:
                Debug.Log("<b>ERROR:</b> There is no cellState associated");
                break;
        }         
    }
}
