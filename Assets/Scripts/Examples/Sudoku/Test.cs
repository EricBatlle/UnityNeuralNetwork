using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    public int[] array = new int[2];
    [SerializeField] public List<int> intList = new List<int>();
    [SerializeField] public List<int[]> arrayList = new List<int[]>();

    public void TestA()
    {
        array[0] = 1;
        array[1] = 2;

        intList.Clear();
        arrayList.Clear();

        arrayList.Add(array);

        intList.Add(array[0]);
        
        array[0] = 10;

        PrintListContent(arrayList);
        //"item = 10"  
        //"item  = 2"

        PrintListContent(intList);
        //"item = 1"   (instead of 10?!)
        //"item  = 2"
    }
    
    [ContextMenu("Print")]
    public void PrintListContent(List<int> list)
    {
        //foreach (int item in intList)
        //{
        //        print("item = " + item);
        //}
    }
    public void PrintListContent(List<int[]> list)
    {
        //foreach (int item in intList)
        //{
        //        print("item = " + item);
        //}
    }    
}
