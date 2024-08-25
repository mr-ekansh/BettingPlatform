using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyMatchesMatka : MonoBehaviour
{
    public GameObject DeetsPanel;
    private string access_token;
    private string bearer;
    public int userID;
    public RawImage profileImage;
    public TMP_Text walletText;
    public string profileImagePath;
    public string Accept;

    public TMP_Text M_amount;
    public TMP_Text M_cityName;
    public TMP_Text M_cityhead;
    public TMP_Text M_gamename;
    public RawImage M_gameImage;
    public TMP_Text M_date;
    public TMP_Text M_gamestatus;
    public TMP_Text M_winamount;
    public GameObject DragonNomatches;
    public GameObject RouletteNoMatches;
    public GameObject MatkaNoMatches;

    public GameObject matkamatches_prefab;
    public GameObject matka_content;
    public GameObject matkanumbers_content;
    public GameObject matkamatchnos_prefab;
    string storagepath;

    public GameObject roulettematches_prefab;
    public GameObject roulette_content;

    public GameObject dvtmatches_prefab;
    public GameObject dvt_content;

    [Serializable]
    public class BettingData
    {
        public int is_result_declared;
        public int gtype;
        public string game_status;
        public string company_name;
        public string image;
        public int user_id;
        public int bet_type;
        public int live_game_id;
        public int amount_played;
        public string game_type;
        public string created_at;
        public int amount;
        public string winning_amount;
    }
    [Serializable]
    public class Number
    {
        public int number;
        public int amount;
    }
    [Serializable]
    public class MatkaDetailsRoot
    {
        public bool status;
        public string message;
        public List<Number> numbers;
        public BettingData betting_data;
        public object winning_data;
        public string winning_percentage;
        public int total_amount_played;
    }

    [Serializable]
    public class DTmatchesHistory
    {
        public int id;
        public int live_game_id;
        public int user;
        public string number;
        public int amount;
        public string created_at;
        public DateTime updated_at;
        public string type;
        public int profit;
        public string slot_id;
    }
    [Serializable]
    public class DTHistoryRoot
    {
        public bool status;
        public string message;
        public List<DTmatchesHistory> data;
    }


    [Serializable]
    public class RouletteMatch
    {
        public int id;
        public int live_game_id;
        public int user;
        public string number;
        public int amount;
        public string created_at;
        public DateTime updated_at;
        public string type;
        public int profit;
        public string slot_id;
    }

    [Serializable]
    public class RouletteMatchRoot
    {
        public bool status;
        public string message;
        public List<RouletteMatch> data;
    }

    [Serializable]
    public class Gamestatus
    {
        public int id;
        public string name;
        public DateTime created_at;
        public object updated_at;
    }

    [Serializable]
    public class Livegame
    {
        public int id;
        public int company;
        public int game_type;
        public string created_at;
        public string updated_at;
        public int status;
        public int is_result_declared;
        public string result_declare_time;
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
        public Gamestatus gamestatus;
    }
    [Serializable]
    public class Match
    {
        public string company_name;
        public string image;
        public int betting_id;
        public int user_id;
        public int bet_type;
        public int live_game_id;
        public int amount_played;
        public string game_type;
        public string created_at;
        public string winning_amount;
        public Livegame livegame;
    }
    [Serializable]
    public class MatkaMatchRoot
    {
        public bool status;
        public string message;
        public List<Match> matches;
        public string base_path;
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        profileImagePath = PlayerPrefs.GetString("ProfileImagePath");
        StartCoroutine(GetAvatarImage(profileImagePath, profileImage));
        RouletteHistory();
        DragonHistory();
        GetMatkaMatch();

        walletText.text = PlayerPrefs.GetString("WalletAmount");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GameObject sceneloader = GameObject.FindGameObjectWithTag("SceneManager");
            sceneloader.GetComponent<SceneLoader>().HomePage();
        }
    }
    
    public void GetMatkaMatch()
    {
        userID = PlayerPrefs.GetInt("userID");
        StartCoroutine(GetMatkaMatches(userID));
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

    IEnumerator GetAvatarImage1(string x, RawImage y)
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

    IEnumerator GetGameImage(string x, Image y)
    {

        UnityWebRequest AvatarImage = UnityWebRequestTexture.GetTexture(x);
        yield return AvatarImage.SendWebRequest();

        if (AvatarImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)AvatarImage.downloadHandler).texture;
            y.GetComponent<Image>().sprite = Sprite.Create(img, new Rect(0, 0, 120, 120), Vector2.zero);
        }
    }
    IEnumerator GetMatkaMatches(int id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest MatkaMatches = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/my-matches", idform);
        MatkaMatches.SetRequestHeader("Authorization", bearer + access_token);
        MatkaMatches.SetRequestHeader("Accept", Accept);
        yield return MatkaMatches.SendWebRequest();
        if (MatkaMatches.error != null)
        {
        }
        else
        {
            storagepath = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/";
            MatkaMatchRoot matka = JsonUtility.FromJson<MatkaMatchRoot>(MatkaMatches.downloadHandler.text);
            if(matka.matches.Count<=0)
            {
                MatkaNoMatches.SetActive(true);
            }
            else
            {
                MatkaNoMatches.SetActive(false);
            }
            for (int i = 0; i < matka.matches.Count; i++)
            {
                GameObject mm_prefab =  Instantiate(matkamatches_prefab, matka_content.transform);
                if(matka.matches[i].winning_amount == "" || matka.matches[i].winning_amount == null)
                {
                    mm_prefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<TMP_Text>().text = "NO REWARDS";
                }
                else
                {
                    mm_prefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<TMP_Text>().text = "₹ " + matka.matches[i].winning_amount;
                }
                string imagepath = storagepath + matka.matches[i].image;
                RawImage z =  mm_prefab.transform.GetChild(3).GetComponentInChildren<RawImage>();
                StartCoroutine(GetAvatarImage1(imagepath,z));
                mm_prefab.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = matka.matches[i].game_type + " Game";
                mm_prefab.transform.GetChild(4).GetComponentInChildren<TMP_Text>().text = matka.matches[i].company_name;
                System.DateTime dateTime = System.DateTime.Parse(matka.matches[i].created_at);
                mm_prefab.transform.GetChild(5).GetComponentInChildren<TMP_Text>().text = dateTime.ToLongDateString();
                mm_prefab.transform.GetChild(6).GetComponentInChildren<TMP_Text>().text = "Total bet amount: ₹" + matka.matches[i].amount_played.ToString();
                mm_prefab.transform.GetChild(7).GetComponentInChildren<TMP_Text>().text = matka.matches[i].livegame.gamestatus.name;
                if(matka.matches[i].livegame.gamestatus.name.Equals("Off"))
                {
                    mm_prefab.transform.GetChild(7).GetComponentInChildren<TMP_Text>().color = Color.red;
                }
                else
                {
                    mm_prefab.transform.GetChild(7).GetComponentInChildren<TMP_Text>().color = Color.green;
                }
                mm_prefab.transform.GetComponent<MatkaMatchesButton>().betting_id = matka.matches[i].betting_id;
            }
        }
    }
    public void RouletteHistory()
    {
        userID = PlayerPrefs.GetInt("userID");
        StartCoroutine(GetRouletteMatches(userID));
    }
    IEnumerator GetRouletteMatches(int id)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest RouletteMatches = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/my-matches", idform);
        RouletteMatches.SetRequestHeader("Authorization", bearer + access_token);
        RouletteMatches.SetRequestHeader("Accept", Accept);
        yield return RouletteMatches.SendWebRequest();
        if (RouletteMatches.error != null)
        {
        }
        else
        {
            RouletteMatchRoot rm_root = JsonUtility.FromJson<RouletteMatchRoot>(RouletteMatches.downloadHandler.text);
            if(rm_root.data.Count<=0)
            {
                RouletteNoMatches.SetActive(true);
            }
            else
            {
                RouletteNoMatches.SetActive(false);
            }
            for(int i =0; i<rm_root.data.Count; i++)
            {
                GameObject rool_prefab = Instantiate(roulettematches_prefab,roulette_content.transform);
                DateTime date = DateTime.Parse(rm_root.data[i].created_at);
                rool_prefab.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Time : " + date.ToLongTimeString();
                rool_prefab.transform.GetChild(0).GetChild(3).GetComponentInChildren<TMP_Text>().text = "Date : " + date.ToLongDateString();
                rool_prefab.transform.GetChild(0).GetChild(4).GetComponentInChildren<TMP_Text>().text = "₹" + rm_root.data[i].amount.ToString();
                if(rm_root.data[i].type == "Number" || rm_root.data[i].type == "number")
                {
                    rool_prefab.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = "bet placed on: "+rm_root.data[i].number;
                }
                else
                {
                    rool_prefab.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = "bet placed on: "+rm_root.data[i].type;
                }
                rool_prefab.transform.GetChild(0).GetChild(7).GetComponentInChildren<TMP_Text>().text = "slot: "+rm_root.data[i].slot_id;
                if(rm_root.data[i].profit.Equals(0))
                {
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.SetActive(true);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.SetActive(false);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.SetActive(true);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).GetComponentInChildren<TMP_Text>().text = "Won: " + "₹" + rm_root.data[i].profit.ToString();
                }
            }
        }
    }


    public void DragonHistory()
    {
        userID = PlayerPrefs.GetInt("userID");
        StartCoroutine(GetDTMatches(userID));
    }
    IEnumerator GetDTMatches(int id)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest DTMatches = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/my-matches", idform);
        DTMatches.SetRequestHeader("Authorization", bearer + access_token);
        DTMatches.SetRequestHeader("Accept", Accept);
        yield return DTMatches.SendWebRequest();
        if (DTMatches.error != null)
        {
        }
        else
        {
            DTHistoryRoot dt_root = JsonUtility.FromJson<DTHistoryRoot>(DTMatches.downloadHandler.text);
            if(dt_root.data.Count<=0)
            {
                DragonNomatches.SetActive(true);
            }
            else
            {
                DragonNomatches.SetActive(false);
            }
            for(int i =0; i<dt_root.data.Count; i++)
            {
                GameObject rool_prefab = Instantiate(dvtmatches_prefab,dvt_content.transform);
                DateTime date = DateTime.Parse(dt_root.data[i].created_at);
                rool_prefab.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Time : " + date.ToLongTimeString();
                rool_prefab.transform.GetChild(0).GetChild(3).GetComponentInChildren<TMP_Text>().text = "Date : " + date.ToLongDateString();
                rool_prefab.transform.GetChild(0).GetChild(4).GetComponentInChildren<TMP_Text>().text = "₹" + dt_root.data[i].amount.ToString();
                rool_prefab.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = "bet placed on: "+dt_root.data[i].number;
                rool_prefab.transform.GetChild(0).GetChild(7).GetComponentInChildren<TMP_Text>().text = "slot: "+dt_root.data[i].slot_id;
                if(dt_root.data[i].profit.Equals(0))
                {
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.SetActive(true);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.SetActive(false);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.SetActive(true);
                    rool_prefab.transform.GetChild(0).GetChild(5).GetChild(1).GetComponentInChildren<TMP_Text>().text = "Won: " + "₹" + dt_root.data[i].profit.ToString();
                }
            }
        }
    }

    public void MatkaDeets(int id)
    {
        DeetsPanel.SetActive(true);
        userID = PlayerPrefs.GetInt("userID");
        StartCoroutine(GetMatkaDetails(userID, id));
    }

    public void ExitDetail()
    {
        DeetsPanel.SetActive(false);
        foreach (Transform child in matkanumbers_content.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    IEnumerator GetMatkaDetails(int userID, int BettingId)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", userID);
        idform.AddField("betting_id", BettingId);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest MatkaMatchesDeets = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/match-details", idform);
        MatkaMatchesDeets.SetRequestHeader("Authorization", bearer + access_token);
        MatkaMatchesDeets.SetRequestHeader("Accept", Accept);

        yield return MatkaMatchesDeets.SendWebRequest();
        if (MatkaMatchesDeets.error != null)
        {
        }
        else
        {
            MatkaDetailsRoot matka = JsonUtility.FromJson<MatkaDetailsRoot>(MatkaMatchesDeets.downloadHandler.text);
            if (matka.status != true)
            {
            }
            else
            {
                storagepath = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/";
                M_amount.text = "Total bet amount: ₹" + matka.total_amount_played.ToString();
                M_cityName.text = matka.betting_data.company_name;
                M_cityhead.text = matka.betting_data.company_name;
                M_gamename.text = matka.betting_data.game_type + " Game";
                M_gamestatus.text = matka.betting_data.game_status;
                if(matka.betting_data.winning_amount == null || matka.betting_data.winning_amount == "")
                {
                    M_winamount.text = "no rewards";
                }
                else
                {
                    M_winamount.text = "rewards: ₹" + matka.betting_data.winning_amount;
                }

                if (M_gamestatus.text == "On Going")
                {
                    M_gamestatus.color = Color.green;
                }
                else
                {
                    M_gamestatus.color = Color.red;
                }
                DateTime time = DateTime.Parse(matka.betting_data.created_at);

                M_date.text = time.ToLongDateString();
                string imagepath = matka.betting_data.image;
                StartCoroutine(GetAvatarImage1(storagepath + imagepath, M_gameImage));
                for(int i = 0; i<matka.numbers.Count; i++)
                {
                    GameObject mn_prefab =  Instantiate(matkamatchnos_prefab, matkanumbers_content.transform);
                    mn_prefab.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = matka.numbers[i].number.ToString();
                    mn_prefab.transform.GetChild(1).GetChild(0).GetComponentInChildren<TMP_Text>().text = "₹"+matka.numbers[i].amount.ToString();
                }
            }
        }
    }
}


