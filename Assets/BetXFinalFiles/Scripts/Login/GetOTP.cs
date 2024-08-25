using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetOTP : MonoBehaviour
{
    private string FirebaseToken;
    public TMP_InputField ref_code;
    public TMP_InputField mobileNumberLogin;
    public TMP_InputField mobileNumber;
    public TMP_InputField Email;
    public TMP_InputField Name;
    public bool isLoggedIn = false;
    public string MobileNumber;
    public string mobile_number;
    public string _ref_code;
    public string _email;
    public string _name;
    public string codeIND = "+91";
    public string OTPlogin;
    public TMP_InputField LoginOTPtext;

    public ToastFactory toast;
    public int phoneLimit;
    public TMP_InputField otp1;

    public string OTP;

    public GameObject retryPanel;
    public GameObject retryPanel01;
    public GameObject verifyRegisterCanvas;
    public GameObject RegisterCanvas;
    [SerializeField] public string accessToken;
    [SerializeField] public int userID;
    public GameObject LoginOTPCanvas;
    public GameObject LoginCanvas;
    public GameObject sceneLoader;



    [Serializable]
    public class Root
    {
        public bool status;
        public string message;
        public User user;
        public string access_token;
    }

    [Serializable]
    public class UserAuthentication
    {
        public bool status;
        public string message;
        public string access_token;
        public User user;

    }

    [Serializable]
    public class User
    {
        public int id;
        public int role_id;
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

    [Serializable]
    public class UserDetail
    {
        public bool status;
        public string message;
    }
    
    public void Start()    
    {
        Screen.orientation = ScreenOrientation.Portrait;
        int n = PlayerPrefs.GetInt("isLoggedin");
        string y = PlayerPrefs.GetString("Authorization");
        if (n == 1)
        {
            isLoggedIn = true;
            SceneManager.LoadScene(2);
        }

    }
    
    public void SetString(string Key, string Value)
    {
        PlayerPrefs.SetString(Key, Value);

    }

    public void GetString(string Value)
    {
        PlayerPrefs.GetString(Value);
    }

    public void validateLogin()
    {
        mobile_number = mobileNumber.GetComponent<TMP_InputField>().text;
        
        MobileNumber = codeIND + mobile_number;
        _email = Email.GetComponent<TMP_InputField>().text;
        _name = Name.GetComponent<TMP_InputField>().text;
        _ref_code = ref_code.GetComponent<TMP_InputField>().text;

        if (MobileNumber.Length == 13 && _email != "" && _name != "")
        {
            string a = "register";
            StartCoroutine(CallLogin(MobileNumber, a));
            PlayerPrefs.SetString("mobile_number", MobileNumber);
            PlayerPrefs.SetString("10digitnumber", mobile_number);
            PlayerPrefs.Save();

        }
        else
        {
            if (MobileNumber.Length < 13)
            {
                toast.GetComponent<ToastFactory>().SendToastyToast("Incorrect Mobile Number");
            }
            if (_email == "")
            {
                toast.GetComponent<ToastFactory>().SendToastyToast("Email is Required");
            }
            if (_name == "")
            {
                toast.GetComponent<ToastFactory>().SendToastyToast("Username is Required");
            }
        }
    }

    IEnumerator CallLogin(string MobileNumber, string Type)
    {
        WWWForm form = new WWWForm();
        form.AddField("mobile_number", MobileNumber);
        form.AddField("screen", Type);


        UnityWebRequest www = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/login-with-otp", form);
        yield return www.SendWebRequest();

        if (www.error != null)
        {
        }
        else
        {
            UserDetail userDetail = JsonUtility.FromJson<UserDetail>(www.downloadHandler.text);
            if (userDetail.status == true)
            {
                SetString("mobilenumber", MobileNumber);
              
                PlayerPrefs.Save();
                isLoggedIn = true;
                toast.GetComponent<ToastFactory>().SendToastyToast(userDetail.message);
                if(Type == "register")
                {
                    RegisterCanvas.SetActive(false);
                    verifyRegisterCanvas.SetActive(true);
                }

            }
            else
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(userDetail.message);
            }
        }
    }
    public void verifyOTP()
    {
        _email = Email.GetComponent<TMP_InputField>().text;
        _name = Name.GetComponent<TMP_InputField>().text;
        OTP = otp1.GetComponent<TMP_InputField>().text; /*+ otp2.GetComponent<TMP_InputField>().text + otp3.GetComponent<TMP_InputField>().text + otp4.GetComponent<TMP_InputField>().text;*/
        _ref_code = ref_code.GetComponent<TMP_InputField>().text;



        if (OTP != "" && _email != "" && _name != "")
        {
            StartCoroutine(CheckOTP(MobileNumber, OTP, PlayerPrefs.GetString("FireBaseToken"), _email, _name,_ref_code));
        }
        else
        {
            toast.GetComponent<ToastFactory>().SendToastyToast("OTP field is required.");
        }
    }

    public string AccessToken(string access_token)
    {
        return access_token;
    }

    IEnumerator CheckOTP(string mobileNumber, string OTP, string device_token, string email, string name1,string ref_code)
    {
        device_token = "12345";
        string uri = "http://betx99.ap-south-1.elasticbeanstalk.com/api/user/verify-login-with-otp";
        WWWForm form1 = new WWWForm();
        form1.AddField("mobile_number", mobileNumber);
        form1.AddField("otp", OTP);
        form1.AddField("device_token", device_token);
        form1.AddField("email", email);
        form1.AddField("name", name1);
        form1.AddField("referral_code", ref_code);

        UnityWebRequest request = UnityWebRequest.Post(uri, form1);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            toast.GetComponent<ToastFactory>().SendToastyToast(request.error);
        }
        else
        {
            UserAuthentication userauth = JsonUtility.FromJson<UserAuthentication>(request.downloadHandler.text);

            if (userauth.status == true)
            {
                userID = UserID(userauth.user.id);
                SceneManager.LoadScene(2);
                accessToken = AccessToken(userauth.access_token);
                PlayerPrefs.SetString("Authorization", accessToken);
                PlayerPrefs.SetInt("userID", userID);
                PlayerPrefs.SetString("mobile_number", MobileNumber);
                PlayerPrefs.SetString("email", _email);
                PlayerPrefs.SetString("Name", name1);

                PlayerPrefs.SetInt("isLoggedin", 1);
                PlayerPrefs.Save();
                isLoggedIn = true;
                toast.GetComponent<ToastFactory>().SendToastyToast(userauth.message);
            }
            if (userauth.status == false)
            {
                isLoggedIn = false;
                toast.GetComponent<ToastFactory>().SendToastyToast(userauth.message);

            }
        }
    }

    private int UserID(int id)
    {
        return id;
    }

    public void GetOTPlogin()
    {

        MobileNumber = codeIND + mobileNumberLogin.text;
        if (MobileNumber.Length == 13)
        {
            string a = "login";
            StartCoroutine(CallLogin(MobileNumber, a));
            LoginCanvas.SetActive(false);
            LoginOTPCanvas.SetActive(true);
            PlayerPrefs.SetString("10digitnumber", mobileNumberLogin.text);
            PlayerPrefs.Save();
        }
        else
        {
            toast.GetComponent<ToastFactory>().SendToastyToast("Incorrect Mobile Number");
        }


    }

    public void LoginToApp()
    {
        MobileNumber = codeIND + mobileNumberLogin.text;
        OTPlogin = LoginOTPtext.text; ;

        StartCoroutine(LoginOTP(MobileNumber, OTPlogin, PlayerPrefs.GetString("FireBaseToken")));
    }

    IEnumerator LoginOTP(string mobileNumber, string OTP, string device_token)
    {
        device_token = "12345";
        string uri = "http://betx99.ap-south-1.elasticbeanstalk.com/api/user/verify-login";
        WWWForm form1 = new WWWForm();
        form1.AddField("mobile_number", mobileNumber);
        form1.AddField("otp", OTP);
        form1.AddField("device_token", device_token);


        UnityWebRequest request = UnityWebRequest.Post(uri, form1);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
        }
        else
        {
            Root root = JsonUtility.FromJson<Root>(request.downloadHandler.text);

            if (root.status == true)
            {
                userID = root.user.id;

                accessToken =root.access_token;
                isLoggedIn = true;
                PlayerPrefs.SetString("Authorization", accessToken);
                PlayerPrefs.SetString("mobile_number", MobileNumber);
                PlayerPrefs.SetInt("userID", userID);
                PlayerPrefs.SetInt("isLoggedin", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene(2);
                toast.GetComponent<ToastFactory>().SendToastyToast(root.message);
            }
            if (root.status == false)
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(root.message);
            }
        }
    }
}
