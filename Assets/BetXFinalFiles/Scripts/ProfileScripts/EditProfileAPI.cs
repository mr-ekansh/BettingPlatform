using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EditProfileAPI : MonoBehaviour
{
    [Serializable]
    public class Data
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
        public string referral_code;
        public object referral_code_used;
        public object state_id;
        public int is_winning_amount;
        public string mobile;
        public int is_otp_verified;
        public string device_token;
        public string gender;
        public string city;
        public string birthday;
        public string code;
        public object email_otp;
        public int is_email_verified;
        public int profile_status;
        public int profile_complete_percentage;
        public string account_number;
        public string ifsc_code;
        public string client_name;
        public string client_email;
        public string client_mobile;
        public string client_vpa;
        public string app_version;
        public string account_status;
    }

    [Serializable]
    public class EditProfile
    {
        public bool status;
        public string message;
        public Data data;
    }
    
    [Serializable]
    public class TransactionHistory
    {
        public int id;
        public int transaction_type;
        public int user_id;
        public float amount;
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
        public int is_first_payment_bonus;
        public object transactiontype;
    }

    [Serializable]
    public class TransactionRoot
    {
        public bool status;
        public string message;
        public List<TransactionHistory> history;
    }

    [Serializable]
    public class LeaderBoard
    {
        public bool status;
        public string message;
        public List<Userdata> users;
    }

   [Serializable]
   public class Userdata
    {
        public string name;
        public int amount;
        public string profile_picture;
    }


    public GameObject FullprofilePanel;
    public TMP_Text halfpanelemail;
    public TMP_Text halfpanelName;
    public TMP_Text NameShow;
    public TMP_Text EmailShow;
    public TMP_Text CityShow;
    public TMP_InputField EditName;
    public TMP_Text EditDOB;
    public TMP_Dropdown EditGender;
    public TMP_InputField EditEmail;
    public TMP_InputField EditCity;
    private string access_token;
    private string Accept;
    public ToastFactory toast;
    public RawImage[] accimage;
    public RawImage halfimg;
    public RawImage profilepic;
    public Slider slider;
    public TMP_Text percenttext;
    public RawImage[] l_images;
    public TMP_Text[] l_names;
    public TMP_Text[] l_amount;
    public GameObject THbox_prefab;
    public GameObject trans_content;
    public GameObject NoTransactions;

    void Start()
    {
        StartCoroutine(LeaderboardsAPI());
        RequestTrnasactionHistory();
    }

    void Update()
    {
        halfpanelName.text = PlayerPrefs.GetString("Name");
        halfpanelemail.text = PlayerPrefs.GetString("mobile_number");
    }

    public void ProfileEdit()
    {
        StartCoroutine(SetProfile());
    }

    IEnumerator SetProfile()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", PlayerPrefs.GetInt("userID"));
        idform.AddField("city", EditCity.text);
        idform.AddField("name", EditName.text);
        idform.AddField("birthday", EditDOB.text);
        idform.AddField("email", EditEmail.text);
        idform.AddField("gender", EditGender.options[EditGender.value].text);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = "Bearer " + access_token;
        UnityWebRequest ProfileAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/edit-profile", idform);
        ProfileAPI.SetRequestHeader("Authorization", "Bearer " + access_token);
        ProfileAPI.SetRequestHeader("Accept", Accept);

        yield return ProfileAPI.SendWebRequest();

        if(ProfileAPI.error != null)
        {
        }
        else
        {
            EditProfile edit = JsonUtility.FromJson<EditProfile>(ProfileAPI.downloadHandler.text);
            if(edit.status != true)
            {
            }
            else
            {
                PlayerPrefs.SetString("Name", EditName.text);
                PlayerPrefs.SetString("email", EditEmail.text);
                PlayerPrefs.SetString("city", EditCity.text);
                PlayerPrefs.SetString("gender", EditGender.options[EditGender.value].text);
                PlayerPrefs.SetString("date", EditDOB.text);    
                PlayerPrefs.SetInt("ProfilePercentage", edit.data.profile_complete_percentage);  
                if(System.String.IsNullOrEmpty(PlayerPrefs.GetString("date")))
                {}
                else
                {
                    EditDOB.text = PlayerPrefs.GetString("date");
                }
                percenttext.text = edit.data.profile_complete_percentage.ToString() + "% PROFILE COMPLETED";
                int value = 0;
                value = edit.data.profile_complete_percentage;
                value = value/100;
                slider.value = value;
                toast.GetComponent<ToastFactory>().SendToastyToast(edit.message);
            }
        }
    }

    IEnumerator GetAvatarImage(string x, Image y)
    {
        UnityWebRequest AvatarImage = UnityWebRequestTexture.GetTexture(x);
        yield return AvatarImage.SendWebRequest();

        if (AvatarImage.error != null)
        {
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)AvatarImage.downloadHandler).texture;

            y.GetComponent<Image>().sprite = Sprite.Create(img, new Rect(0, 0,112, 112), Vector2.zero);
        }
    }

    public void Setnewimage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 2073600, false);
 
                if (texture != null)
                {
                    StartCoroutine(UploadImage(texture));
                }
            }
        }, "Select an image", "image/*");
    }

    private IEnumerator UploadImage(Texture2D texture)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        for(int i = 0; i<accimage.Length; i++)
        {
            accimage[i].texture = texture;
        }
        halfimg.texture = texture;
        profilepic.texture = texture;
        var bytes = texture.EncodeToJPG();
        var form = new WWWForm();
        form.AddField("user_id", PlayerPrefs.GetInt("userID"));
        form.AddBinaryData("image", bytes);
        
        using(var unityWebRequest = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/profile-picture", form))
        {
            unityWebRequest.SetRequestHeader("Authorization", "Bearer " + access_token);
            unityWebRequest.SetRequestHeader("Accept", Accept);
        
            yield return unityWebRequest.SendWebRequest();
        
            if (unityWebRequest.result != UnityWebRequest.Result.Success) 
            {
                print($"Failed to upload {texture.name}: {unityWebRequest.result} - {unityWebRequest.error}");
            }
            else 
            {
                print($"Finished Uploading {texture.name}");
            }
        }
    }

    private IEnumerator LeaderboardsAPI()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = "Bearer " + access_token;
        UnityWebRequest leadAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/leaderboard", idform);
        leadAPI.SetRequestHeader("Authorization", "Bearer " + access_token);
        leadAPI.SetRequestHeader("Accept", Accept);

        yield return leadAPI.SendWebRequest();

        if(leadAPI.error != null)
        {
        }
        else
        {
            LeaderBoard leader = JsonUtility.FromJson<LeaderBoard>(leadAPI.downloadHandler.text);
            string storage_path = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/";
            for(int i = 0; i<10; i++)
            {
                yield return new WaitForSeconds(0.2f);
                l_names[i].text = leader.users[i].name;
                l_amount[i].text = "₹"+leader.users[i].amount;
                string imagepath = leader.users[i].profile_picture;
                StartCoroutine(GetAvatarImage2(storage_path+imagepath, l_images[i]));
            }
        } 
    }

    IEnumerator GetAvatarImage2(string x, RawImage y)
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
    
    public void RequestTrnasactionHistory()
    {
        int userID = PlayerPrefs.GetInt("userID");

        StartCoroutine(TransactionRequestHistory(userID.ToString()));
    }

    IEnumerator TransactionRequestHistory(string id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        string bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);

        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest TransacHistory = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/transaction-history", idform);
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
                    if(T_root.history[i].transaction_type.Equals(1))
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Deposit";
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.green;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.green;
                    }
                    else if(T_root.history[i].transaction_type.Equals(2))
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Withdrawal";
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.red;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.red;
                    }
                    else
                    {
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = "Betting";
                        thb.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().color = Color.yellow;
                        thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().color = Color.yellow;
                    }
                    thb.transform.GetChild(0).GetChild(5).GetComponentInChildren<TMP_Text>().text = "₹ " + T_root.history[i].amount;
                    DateTime date = DateTime.Parse(T_root.history[i].created_at);
                    thb.transform.GetChild(0).GetChild(6).GetComponentInChildren<TMP_Text>().text = date.ToLongTimeString();
                    thb.transform.GetChild(0).GetChild(7).GetComponentInChildren<TMP_Text>().text = date.ToLongDateString();
                    thb.transform.GetChild(0).GetChild(4).GetComponentInChildren<TMP_Text>().text = T_root.history[i].description;
                }
            }
        }
    }

    public void Website()
    {
        string appURL = PlayerPrefs.GetString("web_URL");
        Application.OpenURL(appURL);
    }
    
    public void Logout()
    {
        string firebasetokens = PlayerPrefs.GetString("FireBaseToken");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("FireBaseToken", firebasetokens);
    }

    public void CallClient()
    {
        string number = PlayerPrefs.GetString("callNumber");
        Application.OpenURL("tel://" + number);
    }

    public void WhatsappClient()
    {
        string number = PlayerPrefs.GetString("whatsappNumber");
        string message = "";
        String url = "https://api.whatsapp.com/send?phone="+ number +"&text=" + message;
        Application.OpenURL(url);
    }
}
