using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SudokuGridCreator : MonoBehaviour
{
    [Serializable]
    public struct Row
    {
        public List<int> rowNumbers;
    }
    [Serializable]
    public struct Column
    {
        public List<int> columnNumbers;
    }
    [Serializable]
    public struct Grid4
    {
        public List<int> contentNumbers;
    }

    [SerializeField] private int[] allSudoku = new int[16];
    [SerializeField] private List<Row> rows = new List<Row>();
    [SerializeField] private List<Column> columns = new List<Column>();
    [SerializeField] private List<Grid4> grids4 = new List<Grid4>();

    [ContextMenu("GenerateGrid")]
    private void GenerateGrid()
    {
        rows.Clear();
        columns.Clear();
        grids4.Clear();

        //Generate all numbers
        for (int i = 0; i < allSudoku.Length; i++)
        {
            allSudoku[i] = i;
            
        }

        //Generate rows        
        for (int u = 0; u < allSudoku.Length; u+=4)
        {
            Row row = new Row();
            row.rowNumbers = new List<int>();
            for (int i = 0; i < allSudoku.Length/4; i++)
            {                               
                row.rowNumbers.Add(allSudoku[i+u]);
            }
            rows.Add(row);
        }        

        //Generate columns
        for (int u = 0; u < 4; u++)
        {
            Column column = new Column();
            column.columnNumbers = new List<int>();
            for (int i = 0; i < allSudoku.Length; i++)
            {
                //every 4
                if (i % 4 == 0)
                    column.columnNumbers.Add(allSudoku[i + u]);
            }
            columns.Add(column);
        }

        //Generate GridsOf4
        for (int m = 0; m < 2; m++)
        {
            for (int t = 0; t < 2; t++)
            {
                Grid4 grid4 = new Grid4();
                grid4.contentNumbers = new List<int>();
                for (int i = 0; i < 2; i++)
                {
                    for (int u = 0; u < 2; u++)
                    {
                        grid4.contentNumbers.Add(allSudoku[ ((i)*4) + (u+(t*8)) + (m*2) ]);
                    }
                }
                grids4.Add(grid4);
            }
        }
    }   
}
