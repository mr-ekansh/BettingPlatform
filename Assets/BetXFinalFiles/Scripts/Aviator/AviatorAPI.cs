using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AviatorAPI : MonoBehaviour
{

    [Serializable]
    public class UserDetail
    {
        public bool status;
        public string message;
    }
    public ToastFactory toast;
    void Start()
    {
        
    }
    
    IEnumerator CallLogin(string amount)
    {
        string access_token = PlayerPrefs.GetString("Authorization");
        WWWForm form = new WWWForm();
        form.AddField("user", PlayerPrefs.GetString("userID"));
        form.AddField("betting_amount", amount); 
        string bearer = "Bearer ";
        string Accept = "application/json";


        UnityWebRequest www = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/aviator/join", form);
        www.SetRequestHeader("Authorization", bearer + access_token);
        www.SetRequestHeader("Accept", Accept);
        yield return www.SendWebRequest();

        if (www.error != null)
        {
        }
        else
        {
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(www.downloadHandler.text);
            if (userDetail.status == true)
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(userDetail.message);

            }
            else
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(userDetail.message);
            }
        }
    }
}
