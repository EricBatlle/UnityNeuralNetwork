using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SudokuGenerator;

public class SudokuGrid : MonoBehaviour
{
    public SquareGrid model;

    private SudokuGrid(SquareGrid modelStruct)
    {
        this.model = modelStruct;
    }
}
