using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DragonTigerBetAPI : MonoBehaviour
{
    [Serializable]
    public class DTUser
    {
        public int id;
        public string name;
        public int amount;
        public string image;
    }
    [Serializable]
    public class Details
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
        public string slot;
    }
    [Serializable]
    public class DragonTigerRoot
    {
        public bool status;
        public string message;
        public Details data;
        public DTTimings timings;
        public List<DTUser> users;
        public int seconds;
    }
    [Serializable]
    public class DTTimings
    {
        public int betting_time;
        public int result_time;
        public int new_game_time;
        public int show_time;
    }
    [Serializable]
    public class AddBet
    {
        public bool status;
        public string message;
        public Wallet wallet;
        public int betting_total;
    }
    [Serializable]
    public class Wallet
    {
        public int id;
        public int user_id;
        public string deposit_balance;
        public int winning_balance;
        public float bonus_balance;
        public object created_at;
        public DateTime updated_at;
        public float referral_balance;
    }

    private string access_token;
    private string bearer;
    private string Accept;
    public RawImage profile;
    public TMP_Text name_;
    public TMP_Text balance;
    public TMP_Text[] balanceText;
    public TMP_Text[] dt_names;
    public RawImage[] dt_profileImage;
    public TMP_Text balanceuser;
    public int userID;

    public int DvTId;
    public int DvTGameTime;
    public int DvTresultTime;
    public int DvTstartTime;
    public int DvtShowTime;
    public TMP_Text totalbet_amount;
    public GameObject introscreen;
    public AudioSource intro;
    public AudioSource BGaudio;
    public CanvasScaler _canvas;
    public Camera maincam;

    private GameTimerDvT timer;
    public TMP_Text slot_txt;

    void Awake()
    {
        if(maincam.aspect > 1.5f)
        {
            _canvas.matchWidthOrHeight = 1;
        }
        else
        {
            _canvas.matchWidthOrHeight = 0;
        }
    }


    void Start()
    {
        StartCoroutine(removeintro());
        Screen.orientation = ScreenOrientation.Landscape;
        string s = PlayerPrefs.GetString("ProfileImagePath");
        StartCoroutine(GetAvatarImage(s, profile));
        timer = GameObject.FindWithTag("DvTHandler").GetComponent<GameTimerDvT>();
        name_.text = PlayerPrefs.GetString("Name");
        balance.text = "₹"+PlayerPrefs.GetString("WalletAmount");
    }

    void Update()
    {
        if(maincam.aspect > 1.5f)
        {
            _canvas.matchWidthOrHeight = 1;
        }
        else
        {
            _canvas.matchWidthOrHeight = 0;
        }
        balanceuser.text = "₹"+PlayerPrefs.GetString("WalletAmount");
    }
    public IEnumerator removeintro()
    {
        // intro.time = 3f;
        // intro.Play();
        yield return new WaitForSecondsRealtime(3);
        BGaudio.Play();
        yield return new WaitForSecondsRealtime(2);
        StartCoroutine(CallAPI());
        introscreen.SetActive(false);
    }

    public IEnumerator CallAPI()
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest DragonAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/live-game", idform);
        DragonAPI.SetRequestHeader("Authorization", bearer + access_token);
        DragonAPI.SetRequestHeader("Accept", Accept);
        yield return DragonAPI.SendWebRequest();
        if (DragonAPI.error != null)
        {
            StartCoroutine(CallAPI());
            yield break;
        }
        else
        {
            DragonTigerRoot DTroot = JsonUtility.FromJson<DragonTigerRoot>(DragonAPI.downloadHandler.text);
            if (DTroot.status == true)
            {
                DvTId = DTroot.data.id;

                PlayerPrefs.SetInt("DTLiveID", DvTId);
                PlayerPrefs.Save();
                DvTGameTime = 40 - DTroot.seconds;
                PlayerPrefs.SetInt("DvT_Game_time", DvTGameTime);
                
                for(int i = 0; i<DTroot.users.Count; i++)
                {
                    dt_names[i].text = DTroot.users[i].name;
                    balanceText[i].text = "₹" + DTroot.users[i].amount.ToString();
                    string imagepath = DTroot.users[i].image;
                    slot_txt.text = "slot : " + DTroot.data.slot;
                    StartCoroutine(GetAvatarImageOthers(imagepath,dt_profileImage[i]));
                }
                StartCoroutine(timer.timer());
            }
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
    public void PlaceBet(int Amount, string boxtag)
    {
        StartCoroutine(DvTBet(Amount, boxtag));
    }
    IEnumerator DvTBet( int amount,string boxtag)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id",PlayerPrefs.GetInt("DTLiveID"));
        idform.AddField("type", 1);
        idform.AddField("user", PlayerPrefs.GetInt("userID"));
        idform.AddField("amount", amount);
        idform.AddField("number[]", boxtag);
  
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest DvTAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/play", idform);
        DvTAPI.SetRequestHeader("Authorization", bearer + access_token);
        DvTAPI.SetRequestHeader("Accept", Accept);
        yield return DvTAPI.SendWebRequest();
        if (DvTAPI.error != null)
        {
        }
        else
        {
            AddBet root = JsonUtility.FromJson<AddBet>(DvTAPI.downloadHandler.text);
            if (root.status != true)
            {
            }
            else
            {
                PlayerPrefs.SetString("WalletAmount", root.wallet.deposit_balance);
                PlayerPrefs.SetFloat("Bonus", root.wallet.bonus_balance);  
                PlayerPrefs.SetFloat("Commission", root.wallet.referral_balance);  
                PlayerPrefs.Save();
                totalbet_amount.text = "Your Bet Amount: ₹" + root.betting_total.ToString();
            }
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
