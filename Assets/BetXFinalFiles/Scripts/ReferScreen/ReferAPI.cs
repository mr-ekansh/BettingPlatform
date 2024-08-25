using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class ReferAPI : MonoBehaviour
{
    [Serializable]
    public class Refer
    {
        public bool status;
        public string message;
        public float commision;
        public int total_people_added;
        public string invitation_link;
        public Wallet wallet;
        public string app_url;
    }
    
    [Serializable]
    public class Exchange
    {
        public bool status;
        public string message;
        public Wallet wallet;
    }
    
    [Serializable]
    public class myoffer
    {
        public bool status;
        public string message;
        public List<Offers> offers;
    }

    [Serializable]
    public class Offers
    {
        public int id;
        public string title;
        public string image;
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

    public TMP_Text wallet_txt;
    public RawImage profile_img;
    private string access_token;
    private int user_id;
    private string Accept;
    private string bearer = "Bearer ";
    public TMP_Text commission_txt;
    public TMP_Text peepsadded;
    public TMP_Text invitecode;
    public Texture2D sprite;
    private string showURL;
    public ToastFactory toast;
    public GameObject offers_prefab;
    public GameObject offers_content;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        wallet_txt.text = PlayerPrefs.GetString("WalletAmount");
        StartCoroutine(GetImage(PlayerPrefs.GetString("ProfileImagePath"), profile_img));
        StartCoroutine(GetRefer());
        StartCoroutine(GetOffers());   
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

    IEnumerator GetRefer()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        user_id = PlayerPrefs.GetInt("userID");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", user_id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest HomeAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/get-commision", idform);

        HomeAPI.SetRequestHeader("Authorization", bearer + access_token);
        HomeAPI.SetRequestHeader("Accept", Accept);

        yield return HomeAPI.SendWebRequest();
        if (HomeAPI.error != null)
        {
        }
        else
        {                                                             
            Refer home = JsonUtility.FromJson<Refer>(HomeAPI.downloadHandler.text);
            commission_txt.text = "₹" + home.wallet.referral_balance.ToString();
            peepsadded.text = home.total_people_added.ToString();
            invitecode.text = home.invitation_link;
            wallet_txt.text = home.wallet.deposit_balance.ToString();
            showURL = home.app_url;
        }
    } 


    IEnumerator GetOffers()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        user_id = PlayerPrefs.GetInt("userID");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", user_id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest HomeAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/offers", idform);

        HomeAPI.SetRequestHeader("Authorization", bearer + access_token);
        HomeAPI.SetRequestHeader("Accept", Accept);

        string storagepath = "https://elasticbeanstalk-ap-south-1-445780004054.s3.ap-south-1.amazonaws.com/";
        yield return HomeAPI.SendWebRequest();
        if (HomeAPI.error != null)
        {
        }
        else
        {                                                             
            myoffer home = JsonUtility.FromJson<myoffer>(HomeAPI.downloadHandler.text);
            for(int i =0; i<home.offers.Count; i++)
            {
                GameObject mm_prefab =  Instantiate(offers_prefab, offers_content.transform);
                StartCoroutine(GetImage(storagepath+home.offers[i].image,mm_prefab.GetComponent<RawImage>()));
            }
        }
    }

    public void CommisionEX()
    {
        StartCoroutine(ExchangeCOM());
    }

    IEnumerator ExchangeCOM()
    {
        access_token = PlayerPrefs.GetString("Authorization");
        user_id = PlayerPrefs.GetInt("userID");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user_id", user_id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest HomeAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/exchange-commision", idform);

        HomeAPI.SetRequestHeader("Authorization", bearer + access_token);
        HomeAPI.SetRequestHeader("Accept", Accept);

        yield return HomeAPI.SendWebRequest();
        if (HomeAPI.error != null)
        {
        }
        else
        {                                                             
            Exchange home = JsonUtility.FromJson<Exchange>(HomeAPI.downloadHandler.text);
            if(home.status == true)
            {
                home.wallet.deposit_balance.ToString();
            }
        }
    }              

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = invitecode.text;
        toast.GetComponent<ToastFactory>().SendToastyToast("Referral Code Copied");
    }  

    public void ShareAPP()
    { 
        string filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
        File.WriteAllBytes( filePath, sprite.EncodeToPNG() );
        new NativeShare().AddFile( filePath )
        .SetText( "Hey, Let's join BetX99.\n\nIt's super easy and fun to play different game on BetX99 and earn money.\n\n\U0001F449 Choose a game that you want to play.\n\U0001F449Choose numbers that you want to place bet at.\n\U0001F449Place bets on those numbers that you selected and win money.\n\nSign Up using my referral code.\n\n"+"*"+invitecode.text+"*"+"\n\nDownload the app from here.\n"+showURL+"\n\nGet yours, share your code with your freinds and get 10% of the amount they add in their wallet for the first time.\nAlso get 3% commission of the amount they placed on games.\n\nFor more information, visit our website\n"+showURL )
        .SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
        .Share();
    }     
}
