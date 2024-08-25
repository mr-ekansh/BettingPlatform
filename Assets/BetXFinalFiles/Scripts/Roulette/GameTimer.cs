using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private RouletteAPI RouletteAPIHandler;
    int remainingtime;
    private int AddTime;
    public bool timerIsRunning;
    private int seconds;
    public GameObject stoppanel;
    public GameObject startpanel;
    public GameObject resultPanel;
    [SerializeField] TMP_Text timerText;
    public int isWinner;
    public int profit;
    public TMP_Text profitText;
    public GameObject winnerImage;
    public GameObject retryImage;
    public Animator outerringanim;
    public Animator innerringanim;
    public Animator ballanim;
    public GameObject[] spinboxes;
    public GameObject ball;
    public GameObject BG;
    public AudioSource wheel_audio;
    public AudioSource result_audio;
    public AudioSource win_audio;
    public AudioSource loose_audio;
    public Animator wintextanim;
    public AudioSource coinadd;
    public ToastFactory toast;
    public Animator panel_anim;
    public AudioClip balltable;
    public AudioClip ballroll_aud;
    public GameObject resultImage;
    public GameObject gamesoonPanel;


    private void Start()
    {
        RouletteAPIHandler = GameObject.FindGameObjectWithTag("RouletteAPIHandler").GetComponent<RouletteAPI>();
        resultPanel = RouletteAPIHandler.GetComponent<RouletteAPI>().resultsPAnel;
        profit = RouletteAPIHandler.GetComponent<RouletteAPI>().Profit;
        profitText.text = "₹"+profit.ToString();
    }
    
    public IEnumerator timer()
    {
        AddTime = PlayerPrefs.GetInt("Roulette_Game_time");
        if(AddTime < 20)
        {
            while(AddTime>0)
            {
                timerText.text = (AddTime - 1).ToString();
                AddTime--;
                yield return new WaitForSecondsRealtime(1);
            }
            StartCoroutine(RouletteAPIHandler.CallAPI());
            yield break;
        }
        gamesoonPanel.SetActive(false);
        startpanel.SetActive(true);
        outerringanim.speed = 0;
        innerringanim.speed = 0;
        wintextanim.speed = 0;
        ballanim.enabled = true;
        ballanim.speed = 0;
        StartCoroutine(RouletteAPIHandler.GetPastResultAPI());
        yield return new WaitForSecondsRealtime(1);
        timerIsRunning = true;
        remainingtime = AddTime - 19;
        startpanel.SetActive(false);
        while(timerIsRunning == true)
        {
            yield return new WaitForSecondsRealtime(1);
            if(remainingtime>1)
            {
                remainingtime -= 1;
                timerText.text = remainingtime.ToString();
                AddTime -= 1;
            }
            else
            {
                timerIsRunning = false ;
            }
        }
        stoppanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        timerText.text = "0";
        yield return new WaitForSecondsRealtime(2);
        AddTime -= 2;
        if(panel_anim.enabled == false)
        {
            panel_anim.enabled = true;
        }
        panel_anim.Play("rouletteexit", 0, 0f);
        yield return new WaitForSecondsRealtime(1);
        AddTime -= 1;
        wheel_audio.Play();
        outerringanim.speed = 1;
        innerringanim.speed = 1;
        ballanim.speed  = 1;
        yield return new WaitForSecondsRealtime(1.5f);
        AddTime -= 3;
        StartCoroutine(RouletteAPIHandler.GetRouletteResult());
        stoppanel.SetActive(false);
        yield return new WaitForSecondsRealtime(8.3f);
        AddTime -= 8;
        panel_anim.Play("rouletteentry",0,0f);
        yield return new WaitForSecondsRealtime(0.2f);
        if(System.String.IsNullOrEmpty(RouletteAPIHandler.wincheck))
        {
            ballanim.speed = 0;
            ballanim.enabled = false;
            outerringanim.speed = 0;
            innerringanim.speed = 0;
            wheel_audio.Stop();
            resultEmpty();
            toast.GetComponent<ToastFactory>().SendToastyToast("Result Not Declared");
            toast.GetComponent<ToastFactory>().SendToastyToast("Betting Amount Refunded");
            yield break;
        }
        resultPanel.SetActive(true);
        profitText.text = "₹" + RouletteAPIHandler.Profit.ToString();
        if(RouletteAPIHandler.isWinner == 1)
        {
            win_audio.time = 1f;
            win_audio.Play();
            winnerImage.SetActive(true);
            retryImage.SetActive(false);
        }
        else
        {
            loose_audio.Play();
            winnerImage.SetActive(false);
            retryImage.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(1);
        if(RouletteAPIHandler.isWinner == 1)
        {
            wintextanim.Play("rwintext", 0, 0f);
            coinadd.time = 0.3f;
            coinadd.Play();
            wintextanim.speed = 1;
        }
        yield return new WaitForSecondsRealtime(1);
        AddTime -= 2;
        RouletteAPIHandler.ResetTable("ChipOnTable");
        resultPanel.SetActive(false);
        resultImage.SetActive(false);
        startpanel.SetActive(true);
        profitText.text = "₹0";
        ball.transform.SetParent(BG.transform);
        ball.transform.localPosition = new Vector3(533,0,0);
        RouletteAPIHandler.totalamountbet.text = "YOUR BET AMOUNT: ₹00";
        yield return new WaitForSecondsRealtime(1);
        AddTime -= 1;
        StartCoroutine(RouletteAPIHandler.CallAPI());
    }

    private void resultEmpty()
    {
        RouletteAPIHandler.ResetTable("ChipOnTable");
        resultPanel.SetActive(false);
        startpanel.SetActive(true);
        profitText.text = "₹0";
        ball.transform.SetParent(BG.transform);
        ball.transform.localPosition = new Vector3(533,0,0);
        RouletteAPIHandler.totalamountbet.text = "YOUR BET AMOUNT: ₹00";
        StartCoroutine(RouletteAPIHandler.EmptyResult());
        StartCoroutine(RouletteAPIHandler.CallAPI());
    }

    public void BallPlacement(int number)
    {
        for(int i = 0; i<=36; i++)
        {
            if(i.Equals(number))
            {
                Vector3 position = spinboxes[i].transform.localPosition;
                float b = 3f;
                StartCoroutine(startLerp(ball,position,b));
            }
        }
    }

    private IEnumerator startLerp(GameObject chip_position, Vector3 box_position, float timeremaining)
    {
        Vector3 centervalue = new Vector3(0,-440,0);
        if(chip_position.transform.localPosition.x<1 && chip_position.transform.localPosition.y<0)
        {
            while(timeremaining>0)
            {
                chip_position.transform.localPosition = Vector3.Slerp(chip_position.transform.localPosition - centervalue,new Vector3(0,-900,0) - centervalue,timeremaining*Time.deltaTime*1) + centervalue;
                timeremaining -= Time.deltaTime;
                yield return null;
            }
        }
        timeremaining = 3f;
        ballanim.speed = 0;
        ballanim.enabled = false;
        while(timeremaining>0)
        {
            chip_position.transform.localPosition = Vector3.Slerp(chip_position.transform.localPosition - centervalue,box_position - centervalue,timeremaining*Time.deltaTime*1) + centervalue;
            timeremaining -= Time.deltaTime;
            yield return null;
        }
        outerringanim.speed = 0;
        innerringanim.speed = 0;
        wheel_audio.Stop();
        wheel_audio.clip = ballroll_aud; 
        wheel_audio.time = 0f;
        RouletteAPIHandler.Checknocolour();
        RouletteAPIHandler.showresult.text = RouletteAPIHandler.winningNumber.ToString();
        resultImage.SetActive(true);
        result_audio.Play();
    }
}