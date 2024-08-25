using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RouletteAPI : MonoBehaviour
{
    [Serializable]
    public class DataRouletteCall
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
        public object winning_number;
        public string slot;
    }
    [Serializable]
    public class RouletteLive
    {
        public bool status;
        public string message;
        public DataRouletteCall data;
        public List<RouletteUsers> users;
        public Timings timings;
        public int seconds;
    }
    [Serializable]
    public class RouletteUsers
    {
        public int id;
        public string name;
        public int amount;
        public string image;
    }
    [Serializable]
    public class Timings
    {
        public int betting_time;
        public int result_time;
        public int new_game_time;
        public int start_time;
    }
    [Serializable]
    public class Datum
    {
        public int winning_number;
    }
    [Serializable]
    public class PastResult
    {
        public bool status;
        public string message;
        public List<Datum> data;
    }

    [Serializable]
    public class Data
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
    }
    [Serializable]
    public class RouletteResult
    {
        public bool status;
        public string message;
        public Data data;
        public WinningData winning_data;
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
        public DateTime created_at;
        public DateTime updated_at;
        public int referral_balance;
    }
    [Serializable]
    public class Refund
    {
        public string status;
        public string message;
        public Wallet wallet; 
    }
    [Serializable]
    public class WinningData
    {
        public int is_winner;
        public int profit;
    }
    [Serializable]
    public class Roulette
    {
        public bool status;
        public string message;
        public Wallet1 wallet;
        public int betting_total;
    }
    [Serializable]
    public class Wallet1
    {
        public int id;
        public int user_id;
        public string deposit_balance;
        public int winning_balance;
        public float bonus_balance;
        public DateTime created_at;
        public DateTime updated_at;
        public float referral_balance;
    }

    [SerializeField] private string access_token;
    private string bearer;

    public GameObject[] chips;

    public int result;

    private string Accept;
    public int amount;
    public int userID;
    public int live_id;
    public GameObject resultsPAnel;
    public TMP_Text name_;
    public RawImage profile;
    public TMP_Text showresult;
    public GameObject[] checkcolour;
    public int winningNumber;
    public string wincheck;
    public int isWinner;
    public int Profit;
    public TMP_Text[] balanceText;
    public TMP_Text[] r_names;
    public RawImage[] r_profileImage;
    public TMP_Text[] pastWinner;
    private GameTimer timer;

    public Datum[] datum;

    public GameObject[] Red;

    public GameObject[] Black;

    public GameObject[] Green;


    public TMP_Text balanceuser;
    public TMP_Text totalamountbet;
    
    public bool isSpinning;
    
    public CanvasScaler _canvas;
    public CanvasScaler _canvas1;
    public Camera maincam;
    public TMP_Text slot_txt;

    void Awake()
    {
        Debug.Log(maincam.aspect.ToString());
        if(maincam.aspect > 1.5f)
        {
            _canvas.matchWidthOrHeight = 0.5f;
            _canvas1.matchWidthOrHeight = 1;
        }
        else
        {
            _canvas.matchWidthOrHeight = 0;
            _canvas1.matchWidthOrHeight = 0;
        }
    }
    void Start()
    {
        StartCoroutine(GetAvatarImage(PlayerPrefs.GetString("ProfileImagePath"), profile));
        StartCoroutine(CallAPI());
        timer = GameObject.FindWithTag("RouletteAPIHandler").GetComponent<GameTimer>();
        name_.text = PlayerPrefs.GetString("Name");
    }
    void Update()
    {
        if(maincam.aspect > 1.5f)
        {
            _canvas.matchWidthOrHeight = 1;
            _canvas1.matchWidthOrHeight = 1;
        }
        else
        {
            _canvas.matchWidthOrHeight = 0;
            _canvas1.matchWidthOrHeight = 0;
        }
    }

    public void RouletteCall( int amount, string boxtag)
    {
        userID = PlayerPrefs.GetInt("userID");

        StartCoroutine(GetRoulettePage(userID, amount, boxtag));
    }

    IEnumerator GetRoulettePage(int user_id, int amount, string boxtag)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        live_id = PlayerPrefs.GetInt("RouletteLiveID");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id", live_id);
        idform.AddField("user", user_id);
        
        idform.AddField("amount", amount);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        headers["Accept"] = Accept;

       
        string tp1 = "Number";
        string tp2 = "Odd";
        string tp3 = "Even";
        string tp4 = "Black";
        string tp5 = "Red";
        string tp6 = "Row1";
        string tp7 = "Row2";
        string tp8 = "Row3";
        string tp9 = "Column1";
        string tp10 = "Column2";
        string tp11 = "Column3";
        string tp12 = "OneToTwelve";
        string tp13 = "NineteenToThirtysix";

        Debug.Log(boxtag);
        Debug.Log(amount.ToString());
        string boxname = boxtag;

        if (boxtag == "Odd")
        {
            idform.AddField("type",tp2);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Even")
        {
            idform.AddField("type", tp3);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Black")
        {
            idform.AddField("type", tp4);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Red")
        {
            Debug.Log("no run");
            idform.AddField("type", tp5);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Row1")
        {
            idform.AddField("type", tp6);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Row2")
        {
            idform.AddField("type", tp7);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Row3")
        {
            idform.AddField("type", tp8);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Column1")
        {
            idform.AddField("type", tp9);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Column2")
        {
            idform.AddField("type", tp10);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "Column3")
        {
            idform.AddField("type", tp11);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "1-18")
        {
            idform.AddField("type", tp12);
            idform.AddField("number[]", boxname);
        }
        else if (boxtag == "19-36")
        {
            idform.AddField("type", tp13);
            idform.AddField("number[]", boxname);
        }
        else
        {
            Debug.Log("It is a number..................");
            idform.AddField("type", tp1);
            idform.AddField("number[]", boxname);
        }
        UnityWebRequest RouletteAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/play", idform);
        RouletteAPI.SetRequestHeader("Authorization", bearer + access_token);
        RouletteAPI.SetRequestHeader("Accept", Accept);
        yield return RouletteAPI.SendWebRequest();
        if (RouletteAPI.error != null)
        {

            Debug.Log("Error" + RouletteAPI.error);
        }
        else
        {
            Debug.Log("Response" + RouletteAPI.downloadHandler.text);
            Roulette roulette = JsonUtility.FromJson<Roulette>(RouletteAPI.downloadHandler.text);

            if (roulette.status == true)
            {
                Debug.Log(roulette.message);
                totalamountbet.text = "YOUR BET AMOUNT: ₹" + roulette.betting_total.ToString();
                PlayerPrefs.SetString("WalletAmount", roulette.wallet.deposit_balance);  
                PlayerPrefs.SetFloat("Bonus", roulette.wallet.bonus_balance);  
                PlayerPrefs.SetFloat("Commission", roulette.wallet.referral_balance);  
                Debug.Log(roulette.betting_total.ToString());
            }
            else
            {
                Debug.Log(roulette.message);
            }
        }
    }

    public IEnumerator GetRouletteResult()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        live_id = PlayerPrefs.GetInt("RouletteLiveID");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id", live_id);
        idform.AddField("user", PlayerPrefs.GetInt("userID"));
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        headers["Accept"] = Accept;
        UnityWebRequest RouletteResultAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/getResult", idform);
        RouletteResultAPI.SetRequestHeader("Authorization", bearer + access_token);
        RouletteResultAPI.SetRequestHeader("Accept", Accept);
        yield return RouletteResultAPI.SendWebRequest();
        if (RouletteResultAPI.error != null)
        {
            Debug.Log("Error" + RouletteResultAPI.error);
            timer.BallPlacement(23);
        }
        else
        {
            Debug.Log("Response" + RouletteResultAPI.downloadHandler.text);

            RouletteResult result = JsonUtility.FromJson<RouletteResult>(RouletteResultAPI.downloadHandler.text);
            Debug.Log("Data" + result.data);                
            Debug.Log("Winning number is........." + result.data.winning_number);
            wincheck = result.data.winning_number;
            if(System.String.IsNullOrEmpty(wincheck))
            {

            }
            else
            {
                winningNumber = int.Parse(result.data.winning_number);
                timer.BallPlacement(winningNumber);
                PlayerPrefs.SetString("WalletAmount", result.wallet.deposit_balance.ToString());
                isWinner = result.winning_data.is_winner;
                Profit = result.winning_data.profit;
                PlayerPrefs.SetInt("Roulette_Profit", result.winning_data.profit);
                PlayerPrefs.Save();
            }
        }
    }

    public void Checknocolour()
    {
        int[] Reds = { 32, 19, 21, 25, 34, 27, 36, 30, 23, 5, 16, 1, 14, 9, 18, 7, 12, 3 };
        int[] Blacks = { 15, 4, 2, 17, 6, 13, 11, 8, 10, 24, 33, 20, 31, 22, 29, 28, 35, 26 };
        for(int i = 0; i<18; i++)
        {
            if(winningNumber == 0)
            {
                checkcolour[0].SetActive(true);
                checkcolour[1].SetActive(false);
                checkcolour[2].SetActive(false);
                break;
            }
            else if(winningNumber == Reds[i])
            {
                checkcolour[0].SetActive(false);
                checkcolour[1].SetActive(true);
                checkcolour[2].SetActive(false);
                break;
            }
            else if(winningNumber == Blacks[i])
            {
                checkcolour[0].SetActive(false);
                checkcolour[1].SetActive(false);
                checkcolour[2].SetActive(true);
                break;
            }
        }
    }
    
    public void ResetTable(string Tag)
    {
        checkcolour[0].SetActive(false);
        checkcolour[1].SetActive(false);
        checkcolour[2].SetActive(false);
        showresult.text = "";
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tag);
        foreach (GameObject target in gameObjects)
        {
            if (target.CompareTag(Tag))
            {
                Destroy(target);
            }
        }
    }
                   
    

    IEnumerator GetAvatarImage(string x, RawImage y)
    {

        UnityWebRequest AvatarImage = UnityWebRequestTexture.GetTexture(x);
        yield return AvatarImage.SendWebRequest();

        if (AvatarImage.error != null)
        {
            Debug.Log(AvatarImage.error);
        }
        else
        {
            Debug.Log("image downloaded");
                
            Texture2D img = ((DownloadHandlerTexture)AvatarImage.downloadHandler).texture;

            y.texture = img;

        }
    }

    IEnumerator GetAvatarImageOthers(string x, RawImage y)
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

    IEnumerator GetAvatarImageOthers1(string x, RawImage y)
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

    public IEnumerator CallAPI()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest RouletteCallAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/live-game", idform);
        RouletteCallAPI.SetRequestHeader("Authorization", bearer + access_token);
        RouletteCallAPI.SetRequestHeader("Accept", Accept);
        yield return RouletteCallAPI.SendWebRequest();
        if (RouletteCallAPI.error != null)
        {
            Debug.Log("Error" + RouletteCallAPI.error);
            StartCoroutine(CallAPI());
            yield break;
        }
        else
        {
            Debug.Log("Response" + RouletteCallAPI.downloadHandler.text);
            RouletteLive roulette = JsonUtility.FromJson<RouletteLive>(RouletteCallAPI.downloadHandler.text);
            if (roulette.status == true)
            {
                
                PlayerPrefs.SetInt("RouletteLiveID", roulette.data.id);
                PlayerPrefs.SetInt("Roulette_Game_time", 60 - roulette.seconds);
                Debug.Log("time left is   :" + (60 - roulette.seconds).ToString());
                PlayerPrefs.Save();

                for(int i = 0; i<roulette.users.Count; i++)
                {
                    r_names[i].text = roulette.users[i].name;
                    balanceText[i].text = "₹"+roulette.users[i].amount.ToString();
                    string imagepath = roulette.users[i].image;
                    slot_txt.text = "slot : " + roulette.data.slot;
                    StartCoroutine(GetAvatarImageOthers1(imagepath, r_profileImage[i]));
                }
                StartCoroutine(timer.timer());
            }
        }
    }
    public IEnumerator GetPastResultAPI()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest RoulettePastResultAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/past-games", idform);
        RoulettePastResultAPI.SetRequestHeader("Authorization", bearer + access_token);
        RoulettePastResultAPI.SetRequestHeader("Accept", Accept);
        yield return RoulettePastResultAPI.SendWebRequest();
        if (RoulettePastResultAPI.error != null)
        {
            Debug.Log("Error" + RoulettePastResultAPI.error);
        }
        else
        {
            PastResult past = JsonUtility.FromJson<PastResult>(RoulettePastResultAPI.downloadHandler.text);
            if (past.status != true)
            {
                Debug.Log("error");
            }
            else
            {
                Debug.Log("Success");


                pastWinner[0].text = past.data[0].winning_number.ToString();
                pastWinner[1].text = past.data[1].winning_number.ToString();
                pastWinner[2].text = past.data[2].winning_number.ToString();
                pastWinner[3].text = past.data[3].winning_number.ToString();
                pastWinner[4].text = past.data[4].winning_number.ToString();
                pastWinner[5].text = past.data[5].winning_number.ToString();
                pastWinner[6].text = past.data[6].winning_number.ToString();
                pastWinner[7].text = past.data[7].winning_number.ToString();
                pastWinner[8].text = past.data[8].winning_number.ToString();
                pastWinner[9].text = past.data[9].winning_number.ToString();

                int[] rednumbers = {1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36};
                int[] blacknumbers = {2,4,6,8,10,11,13,15,17,20,22,24,26,28,29,31,33,35};
                for(int i =0; i<=9; i++)
                {
                    for (int x = 0; x < 17; x++)
                    {
                        int r1 = rednumbers[x];
                        int r2 = blacknumbers[x];
                        int c = past.data[i].winning_number;
                        if(c.Equals(35)||c.Equals(33))
                        {
                            Green[i].SetActive(false);
                            Red[i].SetActive(false);
                            Black[i].SetActive(true);
                            break;
                        }
                        if(c.Equals(36))
                        {
                            Green[i].SetActive(false);
                            Red[i].SetActive(true);
                            Black[i].SetActive(false);
                            break;
                        }
                        if(c.Equals(r1))
                        {
                            Green[i].SetActive(false);
                            Red[i].SetActive(true);
                            Black[i].SetActive(false);
                            break;
                        }
                        if(c.Equals(r2)||c.Equals(35))
                        {
                            Green[i].SetActive(false);
                            Red[i].SetActive(false);
                            Black[i].SetActive(true);
                            break;
                        }
                        if(c.Equals(0))
                        {
                            Green[i].SetActive(true);
                            Red[i].SetActive(false);
                            Black[i].SetActive(false);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    public IEnumerator EmptyResult()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        live_id = PlayerPrefs.GetInt("RouletteLiveID");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id", live_id);
        idform.AddField("user", PlayerPrefs.GetInt("userID"));
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        headers["Accept"] = Accept;
        UnityWebRequest RouletteResultAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/refund", idform);
        RouletteResultAPI.SetRequestHeader("Authorization", bearer + access_token);
        RouletteResultAPI.SetRequestHeader("Accept", Accept);
        yield return RouletteResultAPI.SendWebRequest();
        if (RouletteResultAPI.error != null)
        {
            Debug.Log("Error" + RouletteResultAPI.error);
        }
        else
        {
            Refund past = JsonUtility.FromJson<Refund>(RouletteResultAPI.downloadHandler.text);
            PlayerPrefs.SetString("WalletAmount",past.wallet.deposit_balance.ToString());
        }
    }
}






