using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddBet : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField enteramount;
    public string ValueEntered;
    public new string name;
    public string str;
    public TMP_Text showamount;
    public TMP_Text totalamount;
    public GameObject jodi;
   
    public int totalSum;
    public bool isToAdd = false;
    public GameObject cancel;

    void Start()
    {
        jodi = GameObject.FindGameObjectWithTag("JodiAPIHandler");
    }

    public void AmountToAdd()
    {
        ValueEntered = enteramount.text;
        string name = gameObject.name;
        int valent = int.Parse(ValueEntered);
        if (ValueEntered != "")
        {
            showamount.gameObject.SetActive(true);
            showamount.text = ValueEntered;
            isToAdd = true;
            cancel.SetActive(true);
        }
        else
        {
            showamount.gameObject.SetActive(false);
            isToAdd = false;
        }
    }

    public void CancelJodiBet()
    {
        string z = totalamount.text;
        int x = int.Parse(z);
        int y = int.Parse(ValueEntered);
        jodi.GetComponent<JodiAPI>().Canceldeal(y);
        int amountleft = x-y;
        totalamount.text = amountleft.ToString();
        showamount.text = "";
        showamount.gameObject.SetActive(false);
        isToAdd = false;
        cancel.SetActive(false);
    }
    
    public void Setinteger()
    {
        PlayerPrefs.SetInt("selectednumber",int.Parse(this.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text));
    }

    public void OpenPanel()
    {
        if(isToAdd == false)
        {
            panel.SetActive(true);
        }
    }
}
