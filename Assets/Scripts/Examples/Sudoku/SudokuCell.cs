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
            SetText();
        }
    }
    [Header("UI References")]
    [SerializeField] private Text textComponent = null;

    private SudokuCell(SudokuCellModel modelStruct)
    {
        _model = modelStruct;
    }

    public void SetText()
    {
        this.textComponent.text = _model.CellValue.ToString();
    }
}
