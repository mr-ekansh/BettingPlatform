using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JodiAPI : MonoBehaviour
{
    [Serializable]
    public class Jodi
    {
        public bool status;
        public string message;
    }
    [SerializeField] private string access_token;
    public GameObject[] number;
    public TMP_Text nameofarea;
    public TMP_Text totalamount;
    [SerializeField] public TMP_Text WalletAmount;
    public int[] toSum;
    public int amount;

    private string Accept;
    private string bearer = "Bearer ";
    public ToastFactory toast;
    public GameObject number_prefab;
    public GameObject number_content;
    public Button placebet_btn;

    void Start()
    {
        setnumbers();
        Screen.orientation = ScreenOrientation.Portrait;
        nameofarea.text = PlayerPrefs.GetString("Matka_Live_name");
        WalletAmount.text = PlayerPrefs.GetString("WalletAmount");
    }

    void Update()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(4);
        }
    }

    private void setnumbers()
    {
        for(int i =1; i<100; i++)
        {
            GameObject numbers = Instantiate(number_prefab, number_content.transform);
            numbers.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
        }
    }

    public void PlaceBetJodi()
    {
        StartCoroutine(JodiBet(PlayerPrefs.GetInt("Matka_Live_id")));
    }


    IEnumerator JodiBet(int livegameid)
    {
        access_token = PlayerPrefs.GetString("Authorization");
        Accept = "application/json";
        WWWForm idform = new WWWForm();
        idform.AddField("user", PlayerPrefs.GetInt("userID"));
        idform.AddField("live_game_id",livegameid);

        for (int x = 0; x < 100; x++)
        {
            Transform myNumber = number_content.transform.GetChild(x);
            string valueEntered = myNumber.GetComponent<AddBet>().ValueEntered;
            bool istoadd = myNumber.GetComponent<AddBet>().isToAdd;
            if (istoadd == true)
            {
                idform.AddField("number["+myNumber.GetChild(0).GetComponent<TMP_Text>().text.ToString()+"]", valueEntered);
            }
            else
            {
                continue;
            }

        }
        Dictionary<string, string> headers = idform.headers;
        headers["Authorization"] = bearer + access_token;
        UnityWebRequest JodiAPI = UnityWebRequest.Post("http://betx99.ap-south-1.elasticbeanstalk.com/api/user/play-jodi-game", idform);

        JodiAPI.SetRequestHeader("Authorization", bearer + access_token);
        JodiAPI.SetRequestHeader("Accept", Accept);
        yield return JodiAPI.SendWebRequest();
        if (JodiAPI.error != null)
        {
            placebet_btn.interactable = true;
        }
        else
        {
            Jodi jodi = JsonUtility.FromJson<Jodi>(JodiAPI.downloadHandler.text);

            if (jodi.status == true)
            {
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>().OpenMatka();
                toast.GetComponent<ToastFactory>().SendToastyToast(jodi.message);
            }
            else
            {
                toast.GetComponent<ToastFactory>().SendToastyToast(jodi.message);
                placebet_btn.interactable = true;
            }
        }
    }

    public void TotalShow()
    {
        Transform myNumber = number_content.transform.GetChild(PlayerPrefs.GetInt("selectednumber"));
        myNumber.GetComponent<AddBet>().AmountToAdd();
        if(totalamount.text == "")
        {
            totalamount.text = myNumber.GetComponent<AddBet>().ValueEntered;
        }
        else
        {
            int x = int.Parse(totalamount.text);
            int y = int.Parse(myNumber.GetComponent<AddBet>().ValueEntered);
            totalamount.text = (x+y).ToString();
        }
    }

    public void Canceldeal(int value)
    {
        int x = int.Parse(totalamount.text);
        totalamount.text = (x-value).ToString();
    }
}


