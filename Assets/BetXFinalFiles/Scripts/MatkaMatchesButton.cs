using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatkaMatchesButton : MonoBehaviour
{
    private MyMatchesMatka _matka;
    public int betting_id;

    void Start()
    {
        _matka = GameObject.FindWithTag("MatkaMatchesAPI").GetComponent<MyMatchesMatka>();
    }

    public void ButtonPress()
    {
        _matka.MatkaDeets(betting_id);
    }
}
