using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CrossingAPI : MonoBehaviour
{

    public class Crossing
    {
        public bool status;
        public string message;
    }
    [SerializeField] private string access_token;
    public TMP_Text nameofarea;

    [SerializeField] public TMP_Text WalletAmount;
    public TMP_InputField amount;
    public string inputamount;
    public TMP_InputField numberarray;
    public string numberarr;
    public string avatarpath;
    public RawImage avatar;
    public RawImage avatar2;
    [SerializeField] public int[] intarray;
    public TMP_Text[] numbers;
    public TMP_Text[] amounts;

    public int[] toSum;
    private string Accept;
    private string bearer = "Bearer ";
    public ArrayExpansion ae;
    public ToastFactory toast;
    public Button placebet_btn;

    void Start()
    {
        WalletAmount.text = PlayerPrefs.GetString("WalletAmount");
        nameofarea.text = PlayerPrefs.GetString("Matka_Live_name");

        avatarpath = PlayerPrefs.GetString("ProfileImagePath");
        StartCoroutine(GetAvatarImage(avatarpath, avatar));
    }
    public void PlaceBetCrossing()
    {
        inputamount = amount.text;
        numberarr = numberarray.text;
        StartCoroutine(CrossingBet());
    }

    IEnumerator CrossingBet()
    {
        string user_id = PlayerPrefs.GetInt("userID").ToString();
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user", user_id);
        idform.AddField("amount", inputamount);
        idform.AddField("live_game_id", PlayerPrefs.GetInt("Matka_Live_id"));
        int[] arr = numberarr.ToString().Select(o => Convert.ToInt32(o) - 48).ToArray();

        int n = arr.Length;

        intarray = arr;


        for (int x = 0; x < n; x++)
        {
            idform.AddField("numbers[]", arr[x].ToString() + arr[x].ToString());
            for (int j = x + 1; j < n; j++)
            {
                idform.AddField("numbers[]", arr[x].ToString() + arr[j].ToString());
                idform.AddField("numbers[]", arr[j].ToString() + arr[x].ToString());
            }
        }

        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest CrossingAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/play-crossing-game", idform);

        CrossingAPI.SetRequestHeader("Authorization", bearer + access_token);
        CrossingAPI.SetRequestHeader("Accept", Accept);


        yield return CrossingAPI.SendWebRequest();
        if (CrossingAPI.error != null)
        {
            placebet_btn.interactable = true;
        }
        else
        {
            Crossing crossing = JsonUtility.FromJson<Crossing>(CrossingAPI.downloadHandler.text);

            if (crossing.status == true)
            {
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>().OpenMatka();
                toast.GetComponent<ToastFactory>().SendToastyToast(crossing.message);
            }
            else
            {
                placebet_btn.interactable = true;
                toast.GetComponent<ToastFactory>().SendToastyToast(crossing.message);
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
            avatar2.texture = img;
        }
    }
    public int SumArray(int[] toBeSummed)
    {
        int sum = 0;
        foreach (int item in toBeSummed)
        {
            sum += item;
        }
        return sum;
    }
}

