using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPanelpress : MonoBehaviour
{
    private MatkaAPI _matkaapi;

    void Start()
    {
        _matkaapi = GameObject.FindWithTag("MatkaAPIHandler").GetComponent<MatkaAPI>();
    }

    public void ButtonPress()
    {
        TMP_Text mytext = this.gameObject.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        _matkaapi.DetailsPanel(mytext.text);
    }
}
