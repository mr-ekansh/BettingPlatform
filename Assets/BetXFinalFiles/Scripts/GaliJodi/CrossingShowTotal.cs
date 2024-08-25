using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CrossingShowTotal : MonoBehaviour
{

    public TMP_InputField amount;
    public string inputamount;
    public TMP_InputField numberarray;
    public string numberarr;
    [SerializeField] public int[] intarray;
    public TMP_Text totalAmount;
    public List<string> list;
    public GameObject cross_content;
    public GameObject crossnumbers_prefab;


    public void ShowTotal()
    { 
        foreach (Transform child in cross_content.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }

        inputamount = amount.text;
        numberarr = numberarray.text;
        int[] arr = numberarr.ToString().Select(o => Convert.ToInt32(o) - 48).ToArray();
        int n = arr.Length;
        intarray = arr;
        int a = int.Parse(inputamount);
        list = new List<string>();
        totalAmount.text = (arr.Length * arr.Length * a).ToString();
        for (int x = 0; x < n; x++)
        {
            list.Add(arr[x].ToString() + arr[x].ToString());
            
            for (int j = x + 1; j < n; j++)
            {
                list.Add(arr[x].ToString() + arr[j].ToString());
                list.Add(arr[j].ToString() + arr[x].ToString());
            }
        }

        for(int i = 0; i < (n*n);i++)
        {
            GameObject cnumber = Instantiate(crossnumbers_prefab,cross_content.transform);
            cnumber.transform.GetChild(0).GetComponentsInChildren<TMP_Text>().First().text = list[i];
            cnumber.transform.GetChild(1).GetComponentsInChildren<TMP_Text>().Last().text = inputamount;
        }
    }
}


