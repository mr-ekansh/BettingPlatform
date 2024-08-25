using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeAPI : MonoBehaviour
{
    [Serializable]
    public class Banner
    {
        public int id;
        public string title;
        public string description;
        public string image;
        public int priority;
        public DateTime created_at;
        public DateTime updated_at;
    }

    [Serializable]
    public class Maintenance
    {
        public bool is_maintenance_mode;
        public string maintenance_mode_message;
    }

    [Serializable]
    public class NoticeDetails
    {
        public int id;
        public string title;
        public string description;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;
    }

    [Serializable]
    public class Onlinegame
    {
        public int id;
        public string name;
        public string image;
        public DateTime created_at;
        public DateTime updated_at;
    }

    [Serializable]
    public class Home
    {
        public bool status;
        public string message;
        public List<Onlinegame> onlinegames;
        public Wallet wallet;
        public string app_url;
        public Update1 update;
        public Maintenance maintenance;
        public bool is_email_verified;
        public string storage_path;
        public User user;
        public bool notice_status;
        public NoticeDetails notice_details;
        public string greetings;
        public string customer_care_number;
        public string whatsapp_number;
        public List<Banner> banner;
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
    public class User
    {
        public int id;
        public int role_id;
        public string name;
        public string email;
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
        public string birthday;
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
    [Serializable]
    public class Wallet
    {
        public int id;
        public int user_id;
        public int deposit_balance;
        public int winning_balance;
        public float bonus_balance;
        public DateTime created_at;
        public DateTime updated_at;
        public float referral_balance;
    }

    public int user_id;
    public User user;
    public string bearer = "Bearer ";

    public TextMeshProUGUI walletAmount;
    public string walletamounttext;

    public string access_token;

    private string Accept;

    public string mobilenumber;

    public RawImage[] GameImages;
    public RawImage AvatarImage;
    private string imagepathRoulette;
    private string imagepathMatka;
    private string imagepathLottery;
    private string imagepathTigerDragon;
    public string imagepathProfileImage;
    private string imagepathWinGo;
    
    public int rouletteGameTime;
    public int resultTime;
    public int newgameTime;
    public int start_time;
    public TMP_InputField EditName;
    public TMP_InputField Editemail;
    public TMP_InputField Editcity;
    public TMP_Text Editdob;
    public TMP_Dropdown EditGender;
    
    public RawImage halfProfileImage;
    public RawImage fullProfileImage;
    public Slider slider;
    public TMP_Text percenttext;
    public GameObject loadCanvas;
    public GameObject AlertCanvas;
    public GameObject UpdateCanvas;
    public GameObject MaintainCanvas;
    public TMP_Text[] noticefields;
    public TMP_Text[] miscmsgs;
   
    private static HomeAPI homeAPI;
    public int rouletteLive_id;
    string thisapp_url;
    public GameObject banner_prefab;
    public GameObject banner_content;
    public ScrollRect scroller;
    Coroutine scrolling;


    void Awake()
    {
        loadCanvas.SetActive(true);
        StartCoroutine(GetHomePage());
        Screen.orientation = ScreenOrientation.Landscape;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Application.Quit();
            }
            else
            {
                GameObject sceneloader = GameObject.FindGameObjectWithTag("SceneManager");
                sceneloader.GetComponent<SceneLoader>().HomePage();
            }
        }
    }

    private IEnumerator scrollbanner()
    {
        float i = 0;
        while (true)
        {
            if(i<1)
            {
                while(i<1f)
                {
                    scroller.normalizedPosition = new Vector2(i, 0);
                    i+=0.1f;
                    yield return new WaitForSeconds(Time.deltaTime*3f);
                }
            }
            else
            {
                while(i>0)
                {
                    scroller.normalizedPosition = new Vector2(i, 0);
                    i-=0.1f;
                    yield return new WaitForSeconds(Time.deltaTime*3f);
                }
            }
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator GetHomePage()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        user_id = PlayerPrefs.GetInt("userID");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", user_id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest HomeAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/home", idform);

        HomeAPI.SetRequestHeader("Authorization", bearer + access_token);
        HomeAPI.SetRequestHeader("Accept", Accept);

        yield return HomeAPI.SendWebRequest();
        if (HomeAPI.error != null)
        {
        }
        else
        {                                                                        
            Home home = JsonUtility.FromJson<Home>(HomeAPI.downloadHandler.text);            
                                
            if (home.status == true)
            { 
                if(home.maintenance.is_maintenance_mode == true)
                {
                    MaintainCanvas.SetActive(true);
                    miscmsgs[0].text = home.maintenance.maintenance_mode_message;
                }  
                else
                {
                    MaintainCanvas.SetActive(false);
                }
                if(home.update.is_update_avalable == true && Application.version != home.update.update_version)
                {
                    UpdateCanvas.SetActive(true);
                    miscmsgs[1].text = home.update.update_message;
                    thisapp_url = home.update.apk_url;
                }
                else
                {
                    UpdateCanvas.SetActive(false);
                }
                if(PlayerPrefs.GetInt("notice") == 1)
                {
                    if(home.notice_status == true)
                    {
                        AlertCanvas.SetActive(true);
                        for(int i = 0; i<home.banner.Count; i++)
                        {
                            GameObject bp = Instantiate(banner_prefab,banner_content.transform);
                            StartCoroutine(GetImage(home.storage_path+home.banner[i].image,bp.GetComponent<RawImage>()));
                        }
                        scrolling = StartCoroutine(scrollbanner());
                        noticefields[0].text = home.greetings;
                        noticefields[1].text = home.notice_details.title;
                        noticefields[2].text = home.notice_details.description;
                        noticefields[3].text = "Customer Care: " + home.customer_care_number;
                        noticefields[4].text = "WhatsApp: " + home.whatsapp_number;
                    }
                    else
                    {
                        AlertCanvas.SetActive(false);
                    }
                } 
                else
                {
                    AlertCanvas.SetActive(false);
                }
                PlayerPrefs.SetString("web_URL",home.app_url);
                PlayerPrefs.SetString("callNumber", home.customer_care_number);
                PlayerPrefs.SetString("whatsappNumber", home.whatsapp_number);
                StartCoroutine(GetImage(home.storage_path +  home.onlinegames[0].image, GameImages[0]));
                StartCoroutine(GetImage1(home.storage_path +  home.onlinegames[1].image, GameImages[1]));
                StartCoroutine(GetImage2(home.storage_path +  home.onlinegames[2].image, GameImages[2]));
                imagepathProfileImage = home.storage_path + home.user.avatar;
                StartCoroutine(GetAvatarImage(imagepathProfileImage, AvatarImage));
                
                PlayerPrefs.SetString("Name", home.user.name);
                PlayerPrefs.SetString("email", home.user.email);
                PlayerPrefs.SetString("city", home.user.city);
                PlayerPrefs.SetString("gender", home.user.gender);
                PlayerPrefs.SetString("date", home.user.birthday);
                PlayerPrefs.SetString("ProfileImagePath", home.storage_path + home.user.avatar);
                PlayerPrefs.SetString("WalletAmount", home.wallet.deposit_balance.ToString());  
                PlayerPrefs.SetFloat("Bonus", home.wallet.bonus_balance);  
                PlayerPrefs.SetFloat("Commission", home.wallet.referral_balance);  
                PlayerPrefs.SetInt("ProfilePercentage", home.user.profile_complete_percentage);                     
                PlayerPrefs.Save();
                percenttext.text = home.user.profile_complete_percentage.ToString() + "% PROFILE COMPLETED";
                float value;
                value = home.user.profile_complete_percentage;
                value = value/100;
                slider.value = value;
                EditName.text = PlayerPrefs.GetString("Name");
                Editemail.text = PlayerPrefs.GetString("email");
                Editcity.text = PlayerPrefs.GetString("city");
                if(System.String.IsNullOrEmpty(PlayerPrefs.GetString("date")))
                {

                }
                else
                {
                    Editdob.text = PlayerPrefs.GetString("date");
                }

                if(PlayerPrefs.GetString("gender") == "Male")
                {
                    EditGender.value = 0;
                }
                else
                {
                    EditGender.value = 1;
                }
                walletamounttext = home.wallet.deposit_balance.ToString();
                walletAmount.text = home.wallet.deposit_balance.ToString();
            }          
        }
    }

    IEnumerator GetImage(string x, RawImage y)
    {
        UnityWebRequest ImageGames = UnityWebRequestTexture.GetTexture(x);
        yield return ImageGames.SendWebRequest();

        if (ImageGames.error != null)
        {
        }
        else
        {               
            Texture2D img = ((DownloadHandlerTexture)ImageGames.downloadHandler).texture;
            y.texture = img;
        }
    }

    IEnumerator GetAvatarImage(string x, RawImage y)
    {
        UnityWebRequest ImageGames = UnityWebRequestTexture.GetTexture(x);
        yield return ImageGames.SendWebRequest();

        if (ImageGames.error != null)
        {
        }
        else
        {               
            Texture2D img = ((DownloadHandlerTexture)ImageGames.downloadHandler).texture;
            y.texture = img;
            halfProfileImage.texture = img;
            fullProfileImage.texture = img;
        }
    }

    IEnumerator GetImage1(string x, RawImage y)
    {
        UnityWebRequest ImageGames = UnityWebRequestTexture.GetTexture(x);
        yield return ImageGames.SendWebRequest();

        if (ImageGames.error != null)
        {
        }
        else
        {               
            Texture2D img = ((DownloadHandlerTexture)ImageGames.downloadHandler).texture;
            y.texture = img;
        }
    }

    IEnumerator GetImage2(string x, RawImage y)
    {
        UnityWebRequest ImageGames = UnityWebRequestTexture.GetTexture(x);
        yield return ImageGames.SendWebRequest();

        if (ImageGames.error != null)
        {
        }
        else
        {               
            Texture2D img = ((DownloadHandlerTexture)ImageGames.downloadHandler).texture;
            y.texture = img;
        }
        loadCanvas.SetActive(false);
    }

    public void Closenotice()
    {
        StopCoroutine(scrolling);
        PlayerPrefs.SetInt("notice", 0);
        AlertCanvas.SetActive(false);
    }

    public void gameUpdate()
    {
        Application.OpenURL(thisapp_url);
    }
}







