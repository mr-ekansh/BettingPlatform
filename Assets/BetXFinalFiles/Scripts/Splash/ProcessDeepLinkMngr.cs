using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    
    [Serializable]
    public class Payment
    {
        public string status;
        public string message;
    }
    public static ProcessDeepLinkMngr Instance { get; private set; }
    public string deeplinkURL;
    private string bearer = "Bearer ";
    private WalletAPI _wallet;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;                
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                onDeepLinkActivated(Application.absoluteURL);
            }
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    private void onDeepLinkActivated(string url)
    {
        deeplinkURL = url;
        StartCoroutine(redirect());
    }
    
    IEnumerator redirect()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(6);
        while (!operation.isDone)
        {
            yield return null;
        }
        StartCoroutine(Getstatus());
    }

    IEnumerator Getstatus()
    {
        string access_token = PlayerPrefs.GetString("Authorization");   
        string Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("order_id", PlayerPrefs.GetString("myclienttxnID"));
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest HomeAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/confirm-upi-order", idform);

        HomeAPI.SetRequestHeader("Authorization", bearer + access_token);
        HomeAPI.SetRequestHeader("Accept", Accept);

        yield return HomeAPI.SendWebRequest();
        if (HomeAPI.error != null)
        {
        }
        else
        {                                                             
            Payment home = JsonUtility.FromJson<Payment>(HomeAPI.downloadHandler.text);
            _wallet = GameObject.FindWithTag("Wallet").GetComponent<WalletAPI>();
            _wallet.CheckStatus(home.status);
            StartCoroutine(_wallet.GetWalletPage(PlayerPrefs.GetInt("userID")));
        }
    } 
}
