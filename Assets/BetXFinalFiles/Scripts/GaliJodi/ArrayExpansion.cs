using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayExpansion : MonoBehaviour
{

    public Array AddToArray(int amt, Array a , int index)
    {
        Array New = Array.CreateInstance(a.GetType().GetElementType(), a.Length + 1);
        for (int i = 0; i < index; i++)
        {
            New.SetValue(a.GetValue(i), i);
        }
        for (int i = index +1;i<New.Length;i++)
        {
            New.SetValue(a.GetValue(i - 1), i);
        }
        New.SetValue(amt, index);
        

        return New;
    }
   
}
