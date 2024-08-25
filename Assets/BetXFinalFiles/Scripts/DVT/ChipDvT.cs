using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class ChipDvT : MonoBehaviour
{
    private DragonTigerBetAPI DvTManager;
    private int amount;
    public ToastFactory toast;
    public Transform chip_content;
    public GameObject[] chips_obj;
    private GameObject my_prefab;
    public GameObject[] chip_prefabs;
    public AudioSource chip_audio;
    public TMP_Text[] TotalBets;
    public int tiger;
    public int dragon;
    public int tie;

    void Start()
    {
        DvTManager = GameObject.FindWithTag("DvTHandler").GetComponent<DragonTigerBetAPI>();
    }

    public void AddandReset(string name, Transform boxtransform)
    {
        chip_audio.Play();
        for(int i = 0; i<chips_obj.Length; i++)
        {
            if(chips_obj[i].GetComponent<Toggle>().isOn == true)
            {
                if(chips_obj[i].name == "AllIn")
                {
                    int s = int.Parse(PlayerPrefs.GetString("WalletAmount"));
                    if(s<10)
                    {
                        break;
                    }
                    else
                    {
                        amount = s;
                        my_prefab = chip_prefabs[i];
                        break;
                    }
                }
                else
                {
                    amount = int.Parse(chips_obj[i].name);
                    my_prefab = chip_prefabs[i];
                    break;
                }
            }
        }
        float a = float.Parse(PlayerPrefs.GetString("WalletAmount"));
        float h = PlayerPrefs.GetFloat("Bonus") + PlayerPrefs.GetFloat("Commission");
        float check = a+h;  
        if (amount <= check)
        {
            float x;
            float y;
            if(name == "Tie")
            {
                x = Random.Range(-139f,144f);
                y = Random.Range(-96f,28f);
            }
            else
            {
                x = Random.Range(-133f,133f);
                y = Random.Range(-190f,120f);
            }
            Vector3 position = new Vector3(x,y,0);
            GameObject chip = Instantiate(my_prefab,chip_content);
            chip.transform.SetParent(boxtransform);
            float b = 1f;
            StartCoroutine(startLerp(chip, position,b));

            DvTManager.PlaceBet(amount, name);
            if(name == "Tiger")
            {
                tiger+=amount;
                TotalBets[0].text = "total bet: ₹" + tiger.ToString();
            }
            else if(name == "Dragon")
            {
                dragon+=amount;
                TotalBets[1].text = "total bet: ₹" + dragon.ToString();
            }
            else
            {
                tie+=amount;
                TotalBets[2].text = "total bet: ₹" + tie.ToString();
            }
        }
        else
        {
            toast.GetComponent<ToastFactory>().SendToastyToast("Add Amount To Wallet");
        }
    }

    private IEnumerator startLerp(GameObject chip_position, Vector3 box_position, float timeremaining)
    {
        while(timeremaining>0)
        {
            chip_position.transform.localPosition = Vector2.Lerp(chip_position.transform.localPosition,box_position,timeremaining*Time.deltaTime*15);
            timeremaining -= Time.deltaTime;
            yield return null;
        }
    }
}



