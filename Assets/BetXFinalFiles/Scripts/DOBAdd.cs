using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DOBAdd : MonoBehaviour
{
    public GameObject Panel;
    public TMP_Text dobInput;
    private DOBCalendar _calendar;
    void Start()
    {
        _calendar = GameObject.FindWithTag("Calendar").GetComponent<DOBCalendar>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
    
    void TaskOnClick()
    {
        string date = this.gameObject.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text;
        int Month = _calendar.currDate.Month;
        int Year = _calendar.currDate.Year;
        string fulldate = date + "-"+ Month.ToString()+ "-" + Year.ToString();
        dobInput.text = fulldate;
        Panel.SetActive(false);
    }
}
