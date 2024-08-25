using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartsAPI : MonoBehaviour
{
    private Calendar _calendar;
    void Start()
    {
        _calendar = GameObject.FindWithTag("Calendar").GetComponent<Calendar>();
        Button btn = this.gameObject.GetComponent<Button>();
		btn.onClick.AddListener(selectDate);
    }

    public void selectDate()
    {
        string date = this.gameObject.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text;
        int Month = _calendar.currDate.Month;
        int Year = _calendar.currDate.Year;
        string fulldate = Year.ToString() + "-"+ Month.ToString()+ "-" + date;
        StartCoroutine(_calendar.CallApi(fulldate));
    }
}
