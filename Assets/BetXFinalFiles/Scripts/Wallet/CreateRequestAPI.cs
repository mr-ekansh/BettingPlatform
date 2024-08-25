using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;
using Firebase;

using System.Text;
using UnityEngine.UI;

public class CreateRequestAPI : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public int order_id;
        public string payment_url;
    }

    [Serializable]
    public class Payment
    {
        public bool status;
        public string msg;
        public Data data;
    }

    [Serializable]
    public class Root
    {
        public bool status;
        public string message;
        public User user;
        public Transaction transaction;
    }

    [Serializable]
    public class Transaction
    {
        public int user_id;
        public string amount;
        public string transaction_id;
        public string date;
        public string time;
        public DateTime updated_at;
        public DateTime created_at;
        public int id;
        public string merchant_key;
    }

    [Serializable]
    public class User
    {
        public int id;
        public object role_id;
        public object name;
        public object email;
        public string avatar;
        public object email_verified_at;
        public List<object> settings;
        public DateTime created_at;
        public DateTime updated_at;
        public object referral_code;
        public object referral_code_used;
        public object state_id;
        public int is_winning_amount;
        public string mobile;
        public int is_otp_verified;
        public string device_token;
        public string gender;
        public string city;
        public object birthday;
        public object code;
        public object email_otp;
        public int is_email_verified;
        public int profile_status;
        public int profile_complete_percentage;
        public object account_number;
        public object ifsc_code;
        public object client_name;
        public object client_email;
        public object client_mobile;
        public object client_vpa;
        public object app_version;
        public string account_status;
    }
    
    public string customerKey;
    public TMP_InputField amount;
    public string amountToEnter;
    public string transactionID;
    private string access_token;
    private int id;
    private string bearer;
    public string JsonString;
    public string Accept;
    public string imagepath;
    public string key;
    private string redirection_url;
    public ToastFactory toast;
    
    void Start()
    {
        imagepath = PlayerPrefs.GetString("ProfileImagePath");
    }

    private void Dynamiclinkcreate()
    {
        var components = new Firebase.DynamicLinks.DynamicLinkComponents(
        // The base Link.
        new System.Uri("https://bet99.page.link/"),
        // The dynamic link URI prefix.
        "https://bet99.page.link/"){
        AndroidParameters = new Firebase.DynamicLinks.AndroidParameters(
        "com.DefaultCompany.Bet99"),
        };
        redirection_url = components.LongDynamicLink.ToString();
    }

    public void MakePayment()
    {
        amountToEnter = amount.GetComponent<TMP_InputField>().text;
        if(amountToEnter == "")
        {
            amountToEnter = "0";
        }
        int samount = int.Parse(amountToEnter);
        if(samount == 0)
        {
            toast.GetComponent<ToastFactory>().SendToastyToast("Add Amount");
        }
        else
        {
            StartCoroutine(GetAmount(amountToEnter));
            Dynamiclinkcreate();
        }
    }

    IEnumerator GetAmount(string amount)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        id = PlayerPrefs.GetInt("userID");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id",id);
        idform.AddField("amount", amount);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest CreateReqAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/create-upi-order", idform);

        CreateReqAPI.SetRequestHeader("Authorization", bearer + access_token);
        CreateReqAPI.SetRequestHeader("Accept", Accept);

        yield return CreateReqAPI.SendWebRequest();
        Debug.Log(CreateReqAPI.downloadHandler.text);
        if(CreateReqAPI.error != null)
        {
        }
        else
        {
            Root root = JsonUtility.FromJson<Root>(CreateReqAPI.downloadHandler.text);
            
            key = root.transaction.merchant_key;
            transactionID = root.transaction.transaction_id;
            PlayerPrefs.SetString("myclienttxnID",root.transaction.transaction_id);
            JsonString = "{"
                + "\n"
            + "\"" + "key" + "\""
            + " : "
            + "\"" + key + "\""
            + ","
            +"\n"
            + "\"" + "client_txn_id" + "\""
            + " : "
            + "\"" + transactionID + "\""
            + ","
            + "\n"
            + "\"" + "amount" + "\""
            + " : "
            + "\"" + amountToEnter + "\""
            + ","
            + "\n"
            + "\"" + "p_info" + "\""
            + " : "
            + "\"" + "Product Name" + "\""
            + ","
            + "\n"
            + "\"" + "customer_name" + "\""
            + " : "
            + "\"" + PlayerPrefs.GetString("Name") + "\""
            + ","
            + "\n"
            + "\"" + "customer_email" + "\""
            + " : "
            + "\"" + PlayerPrefs.GetString("email") + "\""
            + ","
            + "\n"
            + "\"" + "customer_mobile" + "\""
            + " : "
            + "\"" + PlayerPrefs.GetString("10digitnumber") + "\""
            + ", "
            + "\n"
            + "\"" + "redirect_url" + "\""
            + " : "
            + "\"" + redirection_url + "\""
            + ","
            + "\n"
            + "\"" + "udf1" + "\""
            + " : "
            + "\"" + "user1" + "\""
            + ","
            + "\n"
            + "\"" + "udf2" + "\""
            + " : "
            + "\"" + "user " + "\""
            + ","
            + "\n"
            + "\"" + "udf3" + "\""
            + " : "
            + "\"" + "user" + "\""
            + "\n"
            + "}";
            string orderURL = "https://merchant.upigateway.com/api/create_order";
            StartCoroutine(Post(orderURL, JsonString));
        }
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);

        UploadHandlerRaw uH = new UploadHandlerRaw(bodyRaw);
        uH.contentType = "application/json";
        request.uploadHandler = uH;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
        }
        else
        {
            Payment payment = JsonUtility.FromJson<Payment>(request.downloadHandler.text);
            if (payment.status == true)
            {
                Application.OpenURL(payment.data.payment_url);
            }
            else
            {
            }
        }
    }
}
 

