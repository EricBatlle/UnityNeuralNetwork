using UnityEngine;
using UnityEngine.UI;
using static Sudoku;

public class SudokuCell : MonoBehaviour
{
    private SudokuCellStruct _model;
    public SudokuCellStruct Model
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

    private SudokuCell(SudokuCellStruct modelStruct)
    {
        _model = modelStruct;
    }

    private void SetText()
    {
        this.textComponent.text = _model.value.ToString();
    }
}
