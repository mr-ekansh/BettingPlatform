using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WalletAPI : MonoBehaviour
{
    [Serializable]
    public class ExchangeWinningAmount
    {
        public bool status;
        public string message;
    }

    [Serializable]
    public class DepositLimits
    {
        public string minimum_amount;
        public string maximum_amount;
    }
    [Serializable]
    public class Root
    {
        public bool status;
        public string message;
        public Wallet wallet;
        public DepositLimits deposit_limits;
        public string withdraw_button;
        public string withdraw_message;
    }

    [Serializable]
    public class Wallet
    {
        public int id;
        public int user_id;
        public int deposit_balance;
        public int winning_balance;
        public float bonus_balance;
        public string created_at;
        public DateTime updated_at;
        public float referral_balance;
    }

    [SerializeField] private string access_token;
    [SerializeField] public TMP_Text walletText1;
    [SerializeField] public TMP_Text depositText;
    [SerializeField] public TMP_Text commissionText;
    [SerializeField] public TMP_Text bonusText;
    [SerializeField] public TMP_Text topAmount;
    [SerializeField] public RawImage image;
    public ToastFactory toast;
    private string Accept;
    private string bearer = "Bearer ";
    public GameObject _paycanvas;
    public GameObject paymentSuccess;
    public GameObject paymentFailed;
    public string withdrawstatus;
    public Image btn_img;
    public Sprite disbaled_img;
    public TMP_Text status_txt;
    public GameObject[] wallet_elements;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        int x = PlayerPrefs.GetInt("walletscreencash");
        if(x==1)
        {
            wallet_elements[1].SetActive(true);
            wallet_elements[0].SetActive(false);
            wallet_elements[2].SetActive(false);
        }
        else if(x==2)
        {
            wallet_elements[2].SetActive(true);
            wallet_elements[0].SetActive(false);
            wallet_elements[1].SetActive(false);
        }
        else
        {
            wallet_elements[0].SetActive(true);
            wallet_elements[2].SetActive(false);
            wallet_elements[1].SetActive(false);
        }
        int userid = PlayerPrefs.GetInt("userID");
        StartCoroutine(GetWalletPage(userid));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(3);
        }
    }

    public IEnumerator GetWalletPage(int id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest WalletAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/get-wallet", idform);

        WalletAPI.SetRequestHeader("Authorization", bearer + access_token);
        WalletAPI.SetRequestHeader("Accept", Accept);

        yield return WalletAPI.SendWebRequest();
        if (WalletAPI.error != null)
        {
        }
        else
        {
            Root root = JsonUtility.FromJson<Root>(WalletAPI.downloadHandler.text);

            if (root.status != true)
            {
            }
            else
            {
                depositText.text = "₹" + root.wallet.deposit_balance.ToString();
                PlayerPrefs.SetString("WalletAmount", root.wallet.deposit_balance.ToString());
                commissionText.text = "₹" + root.wallet.referral_balance.ToString();
                PlayerPrefs.SetFloat("Bonus", root.wallet.bonus_balance);  
                PlayerPrefs.SetFloat("Commission", root.wallet.referral_balance);  

                walletText1.text = "₹" + root.wallet.winning_balance.ToString();

                bonusText.text = "₹" + root.wallet.bonus_balance.ToString();
                topAmount.text = root.wallet.deposit_balance.ToString();
                PlayerPrefs.SetString("WinningBalance", walletText1.text);
                withdrawstatus = root.withdraw_button;
                if(withdrawstatus == "off")
                {
                    btn_img.sprite = disbaled_img;
                    status_txt.text = root.withdraw_message;
                }
               
                PlayerPrefs.Save();
                string profilepath = PlayerPrefs.GetString("ProfileImagePath");
                StartCoroutine(GetAvatarImage(profilepath, image));
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

    public void CheckStatus(string paystatus)
    {
        _paycanvas.SetActive(true);
        if(paystatus == "true")
        {
            paymentSuccess.SetActive(true);
        }
        else
        {
            paymentFailed.SetActive(true);
        }
    }
}
