using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Calendar : MonoBehaviour
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
    public GameObject chart_content;
    public GameObject chartbox_prefab;

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
        string fulldate = DateTime.Now.Year + "-"+ DateTime.Now.Month + "-" + DateTime.Now.Date;
        StartCoroutine(CallApi(fulldate));
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

    public IEnumerator CallApi(string date)
    {
        string access_token = PlayerPrefs.GetString("Authorization");

        string bearer = "Bearer ";
        string Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("date", date);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest dateAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/get-chart", idform);
        dateAPI.SetRequestHeader("Authorization", bearer + access_token);
        dateAPI.SetRequestHeader("Accept", Accept);
        yield return dateAPI.SendWebRequest();
        if(dateAPI.error == null)
        { 
            foreach (Transform child in chart_content.transform) 
            {
                GameObject.Destroy(child.gameObject);
            }
            chartdetail = JsonUtility.FromJson<charting>(dateAPI.downloadHandler.text);
            int v = -225;
            int bottom = 0;
            string storagepath = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/";
            for(int i = 0; i<chartdetail.chart.Count; i++)
            {
                GameObject box = Instantiate(chartbox_prefab, chart_content.transform);
                box.transform.localPosition = new Vector3(box.transform.localPosition.x, box.transform.localPosition.y + v, 0);
                box.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = chartdetail.chart[i].company_name;
                string imagepath = chartdetail.chart[i].image;
                if(chartdetail.chart[i].is_result_declared == 1)
                {
                    box.transform.GetChild(3).GetChild(0).GetComponentInChildren<TMP_Text>().text = chartdetail.chart[i].number.ToString();
                }
                StartCoroutine(GetAvatarImage(storagepath + imagepath,box.transform.GetChild(0).GetComponentInChildren<RawImage>()));
                bottom += 450;
                v -= 400;
            }
            chart_content.GetComponent<RectTransform>().offsetMin = new Vector2(chart_content.GetComponent<RectTransform>().offsetMin.x, -bottom);
            panel.SetActive(false);
        }
        else
        {
            toast.GetComponent<ToastFactory>().SendToastyToast(chartdetail.message);
        }
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
