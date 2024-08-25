using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class MatkaAPI : MonoBehaviour
{
    [Serializable]
    public class Company
    {
        public int id;
        public string name;
        public string image;
        public DateTime created_at;
        public DateTime updated_at;
        public string bet_end_time;
        public string bet_result_time;
        public int sequence;
        public string holiday;
        public object information;
        public string bet_start_time;
    }
    [Serializable]
    public class LiveGame
    {
        public int id;
        public int company;
        public int game_type;
        public DateTime created_at;
        public DateTime updated_at;
        public int status;
        public int is_result_declared;
        public object result_declare_time;
        public int is_bet_time_over;
        public string bet_close_time;
        public string bet_result_time;
        public int is_cancelled;
        public int is_holiday;
        public int live_users;
        public int total_bets;
        public int total_amount;
        public object user;
        public int betting_id_starts;
        public int betting_id_ends;
        public string name;
        public string image;
        public int players;
    }
    [Serializable]
    public class LiveResults
    {
        public int id;
        public int company;
        public int game_type;
        public DateTime created_at;
        public DateTime updated_at;
        public int status;
        public int is_result_declared;
        public object result_declare_time;
        public int is_bet_time_over;
        public string bet_close_time;
        public string bet_result_time;
        public int is_cancelled;
        public int is_holiday;
        public int live_users;
        public int total_bets;
        public int total_amount;
        public object user;
        public int betting_id_starts;
        public int betting_id_ends;
        public string name;
        public string image;
        public int players;
        public int number;
    }

    [Serializable]
    public class Maintenance
    {
        public bool is_maintenance_mode ;
        public string maintenance_mode_message ;
    }
    [Serializable]
    public class Matka
    {
        public bool status;
        public string message;
        public List<Company> companies;
        public LiveGame live_game;
        public LiveResults live_results;
        public List<UpcomingGame> upcoming_games;
        public object is_holiday;
        public string app_url;
        public Update1 update;
        public Maintenance maintenance;
        public string storage_path;
        public string next_game_message;
        public Wallet wallet;
    }
    [Serializable]
    public class UpcomingGame
    {
        public int id;
        public int company;
        public int game_type;
        public DateTime created_at;
        public DateTime updated_at;
        public int status;
        public int is_result_declared;
        public object result_declare_time;
        public int is_bet_time_over;
        public string bet_close_time;
        public string bet_result_time;
        public int is_cancelled;
        public int is_holiday;
        public int live_users;
        public int total_bets;
        public int total_amount;
        public object user;
        public int betting_id_starts;
        public int betting_id_ends;
        public string name;
        public string image;
        public string bet_end_time;
    }
    [Serializable]
    public class Update1
    {
        public bool is_update_avalable;
        public string update_version;
        public string apk_url;
        public string update_message;
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

    private string Accept;
    [SerializeField]public string access_token;
    private string bearer;
    public int id;
    
    public int live_1_id;
    public TextMeshProUGUI live_1_name;
    public TextMeshProUGUI live_1_peoplePlaying;
    public TextMeshProUGUI walletText;
    public RawImage live_1_sprite;
    public RawImage avatar;
    public string avatarpath;
    public RawImage[] uggimages;
    public TMP_Text[] ugtext;
    public TMP_Text[] ugbet_close_time;
    public TMP_Text[] ugbet_result_time;
    private string[] ugimagepaths = new string[10];
    private string[] deetsimagepaths = new string[10];
    public RawImage deets_img;
    public TMP_Text deets_holiday;
    public TMP_Text deets_bet_2_time;
    public TMP_Text deets_bet_1_time;
    public TMP_Text deets_bet_3_time;
    public TMP_Text deets_2_time;
    public TMP_Text deets_1_time;
    public TMP_Text deets_3_time;
    public TMP_Text deets_city_txt;
    public RawImage live_results_img;
    public TMP_Text live_city_txt;
    public TMP_Text live_betting_txt;
    public GameObject[] ug_panels;
    public GameObject cityicon_prefab;
    public GameObject city_content;
    public GameObject detailsContainer;
    Matka matkadetail;
    string storagepath;
    public GameObject ug_prefab;
    public GameObject main_content;
    public ToastFactory toast;
    int fittervalue = 0;
    int fitterlive = 0;
    public GameObject live_result_obj;
    public GameObject live_games_obj;
    public GameObject live_game_msg;
    public GameObject live_games_panel;
    public GameObject ug_text_obj;
    public GameObject is_live;
    public TMP_Text betclose_txt;

    private static MatkaAPI matkaAPI;

    void Awake()
    {
        StartCoroutine(GetMatka());
        avatarpath = PlayerPrefs.GetString("ProfileImagePath");
        StartCoroutine(GetAvatarImage(avatarpath, avatar));
        detailsContainer.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Update()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(3);
        }
    }
    public void OpenMatka()
    {
        StartCoroutine(GetMatka());
    }

    IEnumerator GetMatka()
    {

        access_token = PlayerPrefs.GetString("Authorization");

        id = PlayerPrefs.GetInt("userID");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest MatkaAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/matka", idform);
        MatkaAPI.SetRequestHeader("Authorization", bearer + access_token);
        MatkaAPI.SetRequestHeader("Accept", Accept);
        yield return MatkaAPI.SendWebRequest();
        if (MatkaAPI.error == null)
        {
            matkadetail = JsonUtility.FromJson<Matka>(MatkaAPI.downloadHandler.text);
            PlayerPrefs.SetString("WalletAmount", matkadetail.wallet.deposit_balance.ToString());
            walletText.text = matkadetail.wallet.deposit_balance.ToString();
            fitterlive = 0;
            fittervalue = 0;
            storagepath = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/"; 
            string livepathimg = matkadetail.live_game.image;
            string mainimgpath = storagepath + livepathimg;
            if(System.String.IsNullOrEmpty(matkadetail.live_game.name))
            {
                live_games_panel.SetActive(false);
                live_game_msg.SetActive(true);
                live_game_msg.GetComponent<TMP_Text>().text = matkadetail.next_game_message;
                fitterlive = 50;
            }
            else 
            {
                live_games_panel.SetActive(true);
                live_game_msg.SetActive(false);
                StartCoroutine(getliveImage(mainimgpath,live_1_sprite));
                live_1_id = matkadetail.live_game.id;
                live_1_name.text = matkadetail.live_game.name;
                live_1_peoplePlaying.text = matkadetail.live_game.players.ToString()+ " People are Playing";
                PlayerPrefs.SetString("Matka_Live_name", matkadetail.live_game.name);
                PlayerPrefs.SetInt("Matka_Live_id", matkadetail.live_game.id);
                PlayerPrefs.Save();
                fitterlive = 0;
            }
            if(System.String.IsNullOrEmpty(matkadetail.live_results.name))
            {
                Destroy(live_result_obj);
                fittervalue = 450;
            }
            else 
            {
                string liveresultspath = matkadetail.live_results.image;
                live_city_txt.text = matkadetail.live_results.name;
                if(matkadetail.live_results.is_result_declared.Equals(1))
                {   
                    live_betting_txt.fontSize = 60;
                    live_betting_txt.text = matkadetail.live_results.number.ToString();
                    is_live.SetActive(false);
                    betclose_txt.text = "";
                }
                else
                {
                    live_betting_txt.fontSize = 15;
                    live_betting_txt.text = "Result will be announced soon.";
                    betclose_txt.text = "Betting is closed.";
                    is_live.SetActive(true);
                }
                StartCoroutine(getliveImage2(storagepath + liveresultspath,live_results_img));
                fittervalue = 0;
            }
            int fitters  = fittervalue + fitterlive;
            int right = 0;
            int x = 140;
            for(int i = 0; i<matkadetail.companies.Count; i++)
            {
                string imagepath = "";
                GameObject cityicon = Instantiate(cityicon_prefab, city_content.transform);
                cityicon.transform.localPosition = new Vector3(x,cityicon.transform.localPosition.y,0);
                imagepath = matkadetail.companies[i].image;
                RawImage image = cityicon.transform.GetChild(0).GetComponentInChildren<RawImage>();
                TMP_Text mytext = cityicon.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
                StartCoroutine(getliveImage3(storagepath + imagepath, image));
                mytext.text = matkadetail.companies[i].name;
                right = right - 250;
                x += 250;
            }
            city_content.GetComponent<RectTransform>().offsetMax = new Vector2(-right,city_content.GetComponent<RectTransform>().offsetMax.y);
            int v = fitters;
            int bottom  = 1650;
            ug_text_obj.transform.localPosition = new Vector3(ug_text_obj.transform.localPosition.x, ug_text_obj.transform.localPosition.y+fitters,0);
            live_games_obj.transform.localPosition = new Vector3(live_games_obj.transform.localPosition.x, live_games_obj.transform.localPosition.y+fitters,0);
            for(int i =0; i<matkadetail.upcoming_games.Count; i++)
            {
                string image = "";
                GameObject ug_obj = Instantiate(ug_prefab, main_content.transform);
                if((i%2)==0)
                {
                    ug_obj.transform.localPosition = new Vector3(ug_obj.transform.localPosition.x, ug_obj.transform.localPosition.y + v, 0);    
                }
                else
                {
                    ug_obj.transform.localPosition = new Vector3(ug_obj.transform.localPosition.x+476, ug_obj.transform.localPosition.y + v, 0);
                    v-=500;
                    bottom += 500;   
                }
                ug_obj.transform.GetChild(0).GetChild(1).GetComponentInChildren<TMP_Text>().text = matkadetail.upcoming_games[i].name;
                image = matkadetail.upcoming_games[i].image;
                RawImage myimage = ug_obj.transform.GetChild(0).GetChild(0).GetComponentInChildren<RawImage>();
                StartCoroutine(getliveImage1(storagepath + image, myimage));
                ug_obj.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Bet Result Time : " + matkadetail.upcoming_games[i].bet_end_time;
                ug_obj.transform.GetChild(0).GetChild(3).GetComponentInChildren<TMP_Text>().text = "Bet Close Time : " + matkadetail.upcoming_games[i].bet_result_time;
            }
            main_content.GetComponent<RectTransform>().offsetMin = new Vector2(main_content.GetComponent<RectTransform>().offsetMin.x,-bottom);
        }
        else
        {
            //toast.GetComponent<ToastFactory>().SendToastyToast(matkadetail.message);
        }
    }
    IEnumerator getliveImage(string imagepath, RawImage y)
    {
        UnityWebRequest liveImage = UnityWebRequestTexture.GetTexture(imagepath);
        yield return liveImage.SendWebRequest();
        if (liveImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)liveImage.downloadHandler).texture;  
            y.texture = img;
        }
    }
    IEnumerator getliveImage1(string imagepath, RawImage y)
    {
        UnityWebRequest liveImage = UnityWebRequestTexture.GetTexture(imagepath);
        yield return liveImage.SendWebRequest();
        if (liveImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)liveImage.downloadHandler).texture;  
            y.texture = img;
        }
    }
    IEnumerator getliveImage2(string imagepath, RawImage y)
    {
        UnityWebRequest liveImage = UnityWebRequestTexture.GetTexture(imagepath);
        yield return liveImage.SendWebRequest();
        if (liveImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)liveImage.downloadHandler).texture;  
            y.texture = img;
        }
    }
    IEnumerator getliveImage3(string imagepath, RawImage y)
    {
        UnityWebRequest liveImage = UnityWebRequestTexture.GetTexture(imagepath);
        yield return liveImage.SendWebRequest();
        if (liveImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)liveImage.downloadHandler).texture;  
            y.texture = img;
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

    public void DetailsPanel(string text)
    {
        detailsContainer.SetActive(true);
        for(int i = 0; i<matkadetail.companies.Count; i++)
        {
            if(matkadetail.companies[i].name.Equals(text))
            {
                string image = matkadetail.companies[i].image;
                deets_holiday.text = "Holiday : " + matkadetail.companies[i].holiday + " day of month";
                if(matkadetail.companies[i].name.Equals("Dubai City")||matkadetail.companies[i].name.Equals("dubai city"))
                {
                    deets_bet_2_time.text = matkadetail.companies[i].bet_end_time;
                    deets_bet_1_time.text = matkadetail.companies[i].bet_start_time;
                    deets_bet_3_time.text = matkadetail.companies[i].bet_result_time;
                    deets_1_time.text = "Bet Start Time: ";
                    deets_2_time.text = "Bet Close Time: ";
                    deets_3_time.text = "Bet Result Time: ";
                }
                else
                {
                    deets_bet_1_time.text = matkadetail.companies[i].bet_end_time;
                    deets_bet_3_time.text = "";
                    deets_bet_2_time.text = matkadetail.companies[i].bet_result_time;
                    deets_3_time.text = "";
                    deets_1_time.text = "Bet Close Time: ";
                    deets_2_time.text = "Bet Result Time: ";
                }
                deets_city_txt.text = matkadetail.companies[i].name;
                StartCoroutine(getliveImage2(storagepath + image, deets_img));
            }
        }
    }
}
