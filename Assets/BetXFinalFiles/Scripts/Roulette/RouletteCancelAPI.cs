using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class RouletteCancelAPI : MonoBehaviour
{
    [Serializable]
    public class Cancel
    {
        public bool status;
        public string message;
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

    private object access_token;
    private string bearer;
    private string Accept;
    public int userID;
    public int liveID;
    public TMP_Text totalamountbet;
    public ToastFactory toast;

    void Start()
    {
    }

    void Update()
    {

    }
    public void CancelBet()
    {
        totalamountbet.text = "YOUR BET AMOUNT: ₹00";
        userID = PlayerPrefs.GetInt("userID");
        liveID = PlayerPrefs.GetInt("RouletteLiveID");
        StartCoroutine(GetRouletteCancel(liveID, userID));
        ResetCancelTable("ChipOnTable");
    }
    public void ResetCancelTable(string Tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tag);
        foreach (GameObject target in gameObjects)
        {
            if (target.CompareTag(Tag))
            {
                Destroy(target);
            }

        }

    }
    IEnumerator GetRouletteCancel(int id, int user_id)
    {
        access_token = PlayerPrefs.GetString("Authorization");

        bearer = "Bearer ";
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("live_game_id", id);
        idform.AddField("user", user_id);
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        headers["Accept"] = Accept;
        UnityWebRequest RouletteCancelAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/roulette/cancel", idform);
        RouletteCancelAPI.SetRequestHeader("Authorization", bearer + access_token);
        RouletteCancelAPI.SetRequestHeader("Accept", Accept);
        yield return RouletteCancelAPI.SendWebRequest();
        if (RouletteCancelAPI.error != null)
        {
        }
        else
        {
            Cancel cancel = JsonUtility.FromJson<Cancel>(RouletteCancelAPI.downloadHandler.text);
            if (cancel.status != true)
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(cancel.message); 
            }
            else
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(cancel.message); 
                PlayerPrefs.SetString("WalletAmount", cancel.wallet.deposit_balance.ToString());
            }
        }
    }
}
