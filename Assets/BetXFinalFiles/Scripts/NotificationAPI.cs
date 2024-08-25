using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NotificationAPI : MonoBehaviour
{
    [Serializable]
    public class Notification
    {
        public int id;
        public string title;
        public string message;
        public string created_at;
        public DateTime updated_at;
        public int type;
        public int user_id;
    }

    [Serializable]
    public class Root
    {
        public bool status;
        public string message;
        public List<Notification> notifications;
    }
    private string access_token;
    private string Accept;
    public GameObject notif_content;
    public GameObject noDeets;
    public GameObject notif_prefab;


    void Start()
    {
        StartCoroutine(GetNotificationPage());
    }

    IEnumerator GetNotificationPage()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", PlayerPrefs.GetInt("userID"));
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = "Bearer " + access_token;
        UnityWebRequest NotifAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/notifications", idform);


        NotifAPI.SetRequestHeader("Authorization", "Bearer " + access_token);
        NotifAPI.SetRequestHeader("Accept", Accept);

        yield return NotifAPI.SendWebRequest();

        if (NotifAPI.error != null)
        {
        }
        else
        {
            Root root = JsonUtility.FromJson<Root>(NotifAPI.downloadHandler.text);
            if(root.notifications.Count<=0)
            {
                noDeets.SetActive(true);
            }
            else
            {
                noDeets.SetActive(false);
                for(int i = 0; i<root.notifications.Count; i++)
                {
                    GameObject mm_prefab =  Instantiate(notif_prefab, notif_content.transform);
                    DateTime date = DateTime.Parse(root.notifications[i].created_at);
                    mm_prefab.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = root.notifications[i].message;
                    mm_prefab.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = "Date: " + date.ToLongDateString();
                    mm_prefab.transform.GetChild(3).GetComponentInChildren<TMP_Text>().text = "Time: " + date.ToLongTimeString();
                }
            }
        }
    }
}
