using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DvTCancelAPI : MonoBehaviour
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
        public object created_at;
        public DateTime updated_at;
        public int referral_balance;
    }

    private object access_token;
    private string bearer;
    private string Accept;
    public int userID;
    public int liveID;
    public ToastFactory toast;
    public TMP_Text total_bets;
    public ChipDvT _manager;
    
    public void CancelBet()
    {
        total_bets.text = "Your Bet Amount: ₹00";
        userID = PlayerPrefs.GetInt("userID");
        liveID = PlayerPrefs.GetInt("DTLiveID");
        StartCoroutine(GetRouletteCancel(liveID, userID));
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
        UnityWebRequest RouletteCancelAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/dragontiger/cancel", idform);
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
                PlayerPrefs.SetString("WalletAmount", cancel.wallet.deposit_balance.ToString());
                ResetCancelTable("ChipOnTable");
                _manager.tie = 0;
                _manager.dragon = 0;
                _manager.tiger = 0;
                _manager.TotalBets[0].text = "total bet: ₹0";
                _manager.TotalBets[1].text = "total bet: ₹0";
                _manager.TotalBets[2].text = "total bet: ₹0";
                toast.GetComponent<ToastFactory>().SendToastyToast(cancel.message);
            }
        }
    }
}
