using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class ChipPlaceRoulette : MonoBehaviour
{
    private int amount;
    private RouletteAPI roulettehandler;
    private int userID;
    private int live_id;
    public TMP_Text balanceuser;
    public ToastFactory toast;
    public GameObject[] chips_obj;
    public GameObject[] chip_prefabs;
    private GameObject my_prefab;
    public Transform chip_content;
    public AudioSource chip_audio;

    void Start()
    {
        roulettehandler = GameObject.FindWithTag("RouletteAPIHandler").GetComponent<RouletteAPI>();
    }

    void Update()
    {
        balanceuser.text = "₹"+PlayerPrefs.GetString("WalletAmount");
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
                        Debug.Log("Insufficient Balance");
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
            if (name == "0")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();

            }
            else if(name == "Even" || name == "Odd")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
            else if(name == "Black" || name == "Red")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
            else if (name == "Row1" || name == "Row2" || name == "Row3")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
            else if (name == "Column1" || name == "Column2" || name == "Column3")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
            else if (name == "1-18" || name == "19-36")
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
            else
            {
                GameObject chip = Instantiate(my_prefab, chip_content);
                chip.transform.SetParent(boxtransform);
                float b = 1f;
                StartCoroutine(startLerp(chip, boxtransform,b));
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.Log("Insufficient Balance");
            toast.GetComponent<ToastFactory>().SendToastyToast("Add Amount To Wallet");
        }
        userID = PlayerPrefs.GetInt("userID");
        live_id = PlayerPrefs.GetInt("RouletteLiveID");
        roulettehandler.RouletteCall(amount, name);
    }

    private IEnumerator startLerp(GameObject chip_position, Transform box_position, float timeremaining)
    {
        while(timeremaining>0)
        {
            chip_position.transform.position = Vector2.Lerp(chip_position.transform.position,box_position.position,timeremaining*Time.deltaTime*15);
            timeremaining -= Time.deltaTime;
            yield return null;
        }
    }
}
