using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WithdrawRequestAPI : MonoBehaviour
{
    private string access_token;
    private string bearer;
    public int userID;
    public string Accept;
    public TMP_InputField Amount;
    public TMP_InputField AccountNumber;
    public TMP_InputField IFSC;
    public TMP_InputField Name;
    public TMP_Text totalamount;
    public string amount;
    public string accountnum;
    public string ifsc;
    public string Name1;
    public string mobilenumber;
    public TMP_Text ShowDepositAmount;

    public GameObject[] status;
    public ToastFactory toast;
    public WalletAPI apiwallet;
    public GameObject NoWithdrawals;
    public GameObject NoTransactions;
    public GameObject THbox_prefab;
    public GameObject trans_content;
    public GameObject WHbox_prefab;
    public GameObject withdraw_content;
    public GameObject noWithdraw;

    [Serializable]
    public class TransactionHistory
    {
        public int id;
        public int user_id;
        public int amount;
        public string date;
        public string time;
        public string created_at;
        public DateTime updated_at;
        public string description;
        public int current_balance;
        public int is_winning_amount;
        public object details;
        public int live_game_id;
        public int is_signup_bonus;
        public object res_check;
        public string transaction_id;
        public string status;
    }

    [Serializable]
    public class TransactionRoot
    {
        public bool status;
        public string message;
        public List<TransactionHistory> history;
    }

    [Serializable]
    public class History
    {
        public int id;
        public int user_id;
        public int amount_requested;
        public string created_at;
        public DateTime updated_at;
        public int status;
        public string account_number;
        public string ifsc_code;
        public string name;
        public string mobile_number;
        public object bank_name;
        public string payment_id;
        public string fund_account_id;
        public string reference_id;
        public string payment_status;
        public string payment_desc;
        public string order_id;
        public string beneficiaryid;
        public string txid;
        public string payment_time;
        public string request_status;
    }

    [Serializable]
    public class Root
    {
        public bool status;
        public string message;
        public List<History> history;
    }
    [Serializable]
    public class WithdrawCash
    {
        public bool status;
        public string message;
        public Wallet wallet;
    }
    [Serializable]
    public class Wallet
    {
        public int id;
        public int user_id;
        public int deposit_balance;
        public int winning_balance;
        public int bonus_balance;
        public string created_at;
        public DateTime updated_at;
        public int referral_balance;
    }

    void Start()
    {
        RequestTrnasactionHistory();
        RequestWithdrawHistory();
    }

    public void GetNewBalance()
    {
        totalamount.text = ShowDepositAmount.text;
    }

    public void WithdrawCashCall()
    {
        if(apiwallet.withdrawstatus == "on")
        {
            userID = PlayerPrefs.GetInt("userID");
            amount = Amount.text;
            accountnum = AccountNumber.text;
            PlayerPrefs.SetString("AmountWithdraw", amount);
            PlayerPrefs.Save();
            mobilenumber = PlayerPrefs.GetString("mobile_number");
            ifsc = IFSC.text;
            Name1 = Name.text;
            StartCoroutine(WithdrawRequest(userID.ToString(), amount, accountnum, mobilenumber, ifsc, Name1));
        }
        else
        {
            noWithdraw.SetActive(true);
        }
    }

    IEnumerator WithdrawRequest(string id, string amount, string accountnumber, string mobile, string ifsc, string name)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        idform.AddField("amount", amount);
        idform.AddField("account_number", accountnumber);
        idform.AddField("mobile_number", mobile);
        idform.AddField("name", name);
        idform.AddField("ifsc_code", ifsc);

        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest withdraw = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/withdraw-request", idform);
        withdraw.SetRequestHeader("Authorization", bearer + access_token);
        withdraw.SetRequestHeader("Accept", Accept);
        yield return withdraw.SendWebRequest();
        if (withdraw.error != null)
        {
        }
        else
        {
            WithdrawCash cash = JsonUtility.FromJson<WithdrawCash>(withdraw.downloadHandler.text);
            if (cash.status == true)
            {
                totalamount.text = "₹" + cash.wallet.deposit_balance.ToString();
               
                toast.GetComponent<ToastFactory>().SendToastyToast(cash.message);

            }
            else
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(cash.message);
            }
        }
    }

    public void RequestWithdrawHistory()
    {
        userID = PlayerPrefs.GetInt("userID");

        StartCoroutine(WithdrawRequestHistory(userID.ToString()));
    }
    IEnumerator WithdrawRequestHistory(string id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);

        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest withHistory = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/withdraw-history", idform);
        withHistory.SetRequestHeader("Authorization", bearer + access_token);
        withHistory.SetRequestHeader("Accept", Accept);
        yield return withHistory.SendWebRequest();
        if (withHistory.error != null)
        {
        }
        else
        {
            Root root = JsonUtility.FromJson<Root>(withHistory.downloadHandler.text);
            if (root.status != true)
            {
            }
            else
            {
                for(int i = 0; i<root.history.Count; i++)
                {
                    NoWithdrawals.SetActive(false);
                    GameObject thb = Instantiate(WHbox_prefab, withdraw_content.transform);
                    if(root.history[i].request_status.Equals("Approved"))
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = root.history[i].request_status;
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.green;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.green;
                    }
                    else if(root.history[i].request_status.Equals("Declined"))
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = root.history[i].request_status;
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.red;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.red;
                    }
                    else
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = root.history[i].request_status;
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.yellow;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.yellow;
                    }
                    thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().text = "₹ " + root.history[i].amount_requested;
                    DateTime date = DateTime.Parse(root.history[i].created_at);
                    thb.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = date.ToLongTimeString();
                    thb.transform.GetChild(0).GetChild(7).GetComponentInChildren<TMP_Text>().text = date.ToLongDateString();
                    thb.transform.GetChild(0).GetChild(4).GetComponentInChildren<TMP_Text>().text = root.history[i].payment_desc;
                }
            }
        }
    }

    public void RequestTrnasactionHistory()
    {
        userID = PlayerPrefs.GetInt("userID");

        StartCoroutine(TransactionRequestHistory(userID.ToString()));
    }

    IEnumerator TransactionRequestHistory(string id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);

        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest TransacHistory = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/deposit-history", idform);
        TransacHistory.SetRequestHeader("Authorization", bearer + access_token);
        TransacHistory.SetRequestHeader("Accept", Accept);
        yield return TransacHistory.SendWebRequest();
        if (TransacHistory.error != null)
        {
        }
        else
        {
            TransactionRoot T_root = JsonUtility.FromJson<TransactionRoot>(TransacHistory.downloadHandler.text);
            if (T_root.status == true)
            {
                for(int i = 0; i<T_root.history.Count; i++)
                {
                    NoTransactions.SetActive(false);
                    GameObject thb = Instantiate(THbox_prefab, trans_content.transform);
                    thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().text = "₹ " + T_root.history[i].amount;
                    thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = T_root.history[i].status;
                    DateTime date = DateTime.Parse(T_root.history[i].created_at);
                    thb.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = date.ToLongTimeString();
                    thb.transform.GetChild(0).GetChild(7).GetComponentInChildren<TMP_Text>().text = date.ToLongDateString();
                    thb.transform.GetChild(0).GetChild(4).GetComponentInChildren<TMP_Text>().text = T_root.history[i].transaction_id;
                }
            }
        }
    }
}


