using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DvTResultAPI : MonoBehaviour
{
    [SerializeField] public string access_token;
    public string AccessToken;
    [SerializeField] public GameObject homeAPIHandler;

    public Image[] showCards = new Image[10];
    public Sprite DragonLine;
    public Sprite TigerLine;
    public Sprite TieLine;

    public DTpastresult[] past;

    public string bearer;
    private string Accept;
    public string winner;
    public int livegameId;
    private int highnumber;
    private int lownumber;
    public int isWinner;
    public int dTProfit;
    public int userID;
    public int isResultDeclare;
    public TMP_Text profit;
    public TMP_Text balanceuser;
    private string Tiger = "Tiger";
    private string Tie = "Tie";
    private string Dragon = "Dragon";
    public ToastFactory toast;
    private GameTimerDvT _timer;

    [Serializable]
    public class DTData
    {
        public int id;
        public string start_time;
        public object end_time;
        public string date;
        public object players;
        public DateTime created_at;
        public DateTime updated_at;
        public string status;
        public int betting_id_starts;
        public int betting_id_ends;
        public int total_bets;
        public int total_amount;
        public int is_result_declared;
        public string winning_number;
        public int low_value;
        public int high_value;
    }
    [Serializable]
    public class DvT
    {
        public bool status;
        public string message;
        public DTData data;
        public DTWinningData winning_data;
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
        public object created_at;
        public DateTime updated_at;
        public int referral_balance;
    }
    [Serializable]
    public class DTWinningData
    {
        public int is_winner;
        public int profit;
    }
    [Serializable]
    public class DTpastresult
    {
        public string winning_number;
    }
    [Serializable]
    public class DTPast
    {
        public bool status;
        public string message;
        public List<DTpastresult> data;
    }
    void Start()
    {
        homeAPIHandler = GameObject.FindGameObjectWithTag("HomeAPIHandler");
        balanceuser = gameObject.GetComponent<DragonTigerBetAPI>().balanceuser;
        _timer = GameObject.FindWithTag("DvTHandler").GetComponent<GameTimerDvT>();
    }

    void Update()
    {
        balanceuser.text = "₹"+PlayerPrefs.GetString("WalletAmount");
    }
    
    public IEnumerator DvTBetResult()
    {

        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id", PlayerPrefs.GetInt("DTLiveID"));
        idform.AddField("user", PlayerPrefs.GetInt("userID"));


        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest DvTAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/getResult", idform);

        DvTAPI.SetRequestHeader("Authorization", bearer + access_token);
        DvTAPI.SetRequestHeader("Accept", Accept);

        yield return DvTAPI.SendWebRequest();
        if (DvTAPI.error != null)
        {
            toast.GetComponent<ToastFactory>().SendToastyToast(DvTAPI.error);
        }
        else
        {
            DvT root = JsonUtility.FromJson<DvT>(DvTAPI.downloadHandler.text);

            winner = root.data.winning_number;
            highnumber = root.data.high_value;
            lownumber = root.data.low_value;
            isWinner = root.winning_data.is_winner;
            

            dTProfit = root.winning_data.profit;
            PlayerPrefs.SetInt("isWinnerDvT", root.winning_data.is_winner);
            _timer.winwallet = root.wallet.deposit_balance;
            PlayerPrefs.Save();
            
            isResultDeclare = root.data.is_result_declared;
            profit.text = "₹" + dTProfit.ToString();

            if(winner == "Tiger")
            {
                PlayerPrefs.SetInt("tigercardanim", highnumber);
                PlayerPrefs.SetInt("dragoncardanim", lownumber);
            }
            else if(winner == "Dragon")
            {
                PlayerPrefs.SetInt("dragoncardanim", highnumber);
                PlayerPrefs.SetInt("tigercardanim", lownumber);
            }
            else
            {
                PlayerPrefs.SetInt("dragoncardanim", highnumber);
                PlayerPrefs.SetInt("tigercardanim", lownumber);
            }
        }
    }

    public IEnumerator DragonPastResults()
    {
        
        AccessToken = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + AccessToken;
        UnityWebRequest DragonPastAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/past-games", idform);
        DragonPastAPI.SetRequestHeader("Authorization", bearer + AccessToken);
        DragonPastAPI.SetRequestHeader("Accept", Accept);
        yield return DragonPastAPI.SendWebRequest();
        if (DragonPastAPI.error != null)
        {
            toast.GetComponent<ToastFactory>().SendToastyToast(DragonPastAPI.error);
        }
        else
        {

            DTPast dtpast = JsonUtility.FromJson<DTPast>(DragonPastAPI.downloadHandler.text);

            for(int i = 0; i<10; i++)
            {
                if(dtpast.data[i].winning_number.Equals(Tiger))
                {
                    showCards[i].sprite = TigerLine;
                }
                if(dtpast.data[i].winning_number.Equals(Tie))
                {
                    showCards[i].sprite = TieLine;
                }
                if(dtpast.data[i].winning_number.Equals(Dragon))
                {
                    showCards[i].sprite = DragonLine;
                }
            }
        }
    }
}

