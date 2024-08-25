using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class DOBCalendar : MonoBehaviour
{

    public class Day
    {
        public int dayNum;
        public Color dayColor;
        public GameObject obj;

        public Day(int dayNum, Color dayColor, GameObject obj)
        {
            this.dayNum = dayNum;
            this.obj = obj;
            UpdateColor(dayColor);
            UpdateDay(dayNum);
        }

        public void UpdateColor(Color newColor)
        {
            obj.GetComponent<Image>().color = newColor;
            dayColor = newColor;
        }

        public void UpdateSprite(Sprite img)
        {
            obj.GetComponent<Image>().sprite = img;
            SpriteState tempState = obj.GetComponent<Button>().spriteState;
            tempState.selectedSprite = img;
            obj.GetComponent<Button>().spriteState = tempState;

        }

        public void UpdateDay(int newDayNum)
        {
            this.dayNum = newDayNum;
            if(dayColor == Color.white || dayColor == Color.green)
            {
                obj.GetComponentInChildren<TMP_Text>().text = (dayNum + 1).ToString();
                obj.GetComponentInChildren<Button>().interactable = true;
            }
            else
            {
                obj.GetComponentInChildren<TMP_Text>().text = "";
                obj.GetComponentInChildren<Button>().interactable = false;
            }
        }
    }

    private List<Day> days = new List<Day>();
    public Transform[] weeks;
    public TMP_Text MonthAndYear;
    public DateTime currDate = DateTime.Now;
    public Sprite simage;
    public Sprite defimage;
    public ToastFactory toast;
    public TMP_Dropdown _drop;
    private TMP_Text text;

    [Serializable]
    public class Charts
    {
       public string company_name;
       public string image;
       public int number;
       public int is_result_declared;
       public int is_holiday;
    }
    [Serializable]
    public class charting
    {
        public bool status;
        public string message;
        public List<Charts> chart;
    }
    charting chartdetail;
    public GameObject panel;
    int imageloc;

    private void Start()
    {   
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
        CreateDropBox(DateTime.Now.Year);
    }

    void CreateDropBox(int year)
    {
        _drop.options.Clear();
        List<string> optionList = new List<string>();
        for(int i = year; i>year-50; i--)
        {
            optionList.Add(i.ToString());
        }
        foreach (string t in optionList)
        {
            _drop.options.Add (new TMP_Dropdown.OptionData() {text=t});
        }
    }

    public void UpdateYear()
    {
        int year = int.Parse(_drop.options[_drop.value].text);
        UpdateCalendar(year, currDate.Month);
    }

    void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);
        currDate = temp;
        MonthAndYear.text = temp.ToString("MMMM") + " " + temp.Year.ToString();
        int startDay = GetMonthStartDay(year,month);
        int endDay = GetTotalNumberOfDays(year, month);

        if(days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)
                {
                    Day newDay;
                    int currDay = (w * 7) + i;
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new Day(currDay - startDay, Color.grey,weeks[w].GetChild(i).gameObject);
                        weeks[w].GetChild(i).GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, Color.white,weeks[w].GetChild(i).gameObject);
                        weeks[w].GetChild(i).GetComponent<Button>().interactable = true;
                    }
                    days.Add(newDay);
                }
            }
        }
        else
        {
            for(int i = 0; i < 42; i++)
            {
                if(i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateColor(Color.grey);
                }
                else
                {
                    days[i].UpdateColor(Color.white);
                }

                days[i].UpdateDay(i - startDay);
            }
        }

        if(DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            imageloc = (DateTime.Now.Day - 1) + startDay;
            days[(DateTime.Now.Day - 1) + startDay].UpdateSprite(simage);
        }
        else
        {
            days[imageloc].UpdateSprite(defimage);
        }
    }

    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);
        return (int)temp.DayOfWeek;
    }

    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    public void SwitchMonth(int direction)
    {
        if(direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }

        UpdateCalendar(currDate.Year, currDate.Month);
    }

    IEnumerator GetAvatarImage(string x, RawImage y)
    {

        UnityWebRequest AvatarImage = UnityWebRequestTexture.GetTexture(x);
        yield return AvatarImage.SendWebRequest();

        if (AvatarImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)AvatarImage.downloadHandler).texture;

            y.texture = img;
        }
    }
}
