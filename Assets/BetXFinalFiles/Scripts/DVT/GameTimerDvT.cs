using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameTimerDvT : MonoBehaviour
{
    private int AddTime;
    private int startTime;
    private int resultTime;
    private int remainingtime;
    public int showTime;
    public GameObject stoppanel;
    private int seconds;
    public TextMeshProUGUI timerText;
    public int livegameId;
    public bool timerIsRunning;
    public GameObject startpanel;
    public GameObject resultPanel;
    public Button Tiger;
    public Button Dragon;
    public Button Tie;
    private DragonTigerBetAPI DragonHandler;
    private DvTResultAPI resultapi;
    public Animator tigercardanim;
    public Animator dragoncardanim;
    public Animator wintextanim;
    private Sprite tigersprite;
    private Sprite dragonsprite;
    public Sprite[] spadecardsprites;
    public Sprite[] diamcardsprites;
    public Sprite[] clubcardsprites;
    public Sprite[] heartcardsprites;
    public Image tigerimage;
    public Image dragonimage;
    private int cardnumber;
    public GameObject WinDragon;
    public GameObject WinTiger;
    public GameObject WinTie;
    public GameObject winimage;
    public GameObject retryimage;
    public TMP_Text profit;
    public Sprite defsprite;
    public int winwallet;
    public AudioSource cardflip_audio;
    public AudioSource tiger_audio;
    public AudioSource dragon_audio;
    public AudioSource win_audio;
    public GameObject[] fakechips;
    private int[] randpositions = {-678,-386,-82,216,522};
    private int[] randpositions1 = {-678,-386,-82};
    private int[] randpositions2 = {216,522};
    public Transform localchipParent;
    public Transform[] mainChipParent;
    private Coroutine _chiproutine;
    public AudioSource chip_audio;
    public AudioSource resultclock_audio;
    public ChipDvT _manager;
    public GameObject gamesoonpanel;

    void Start()
    {
        DragonHandler = GameObject.FindWithTag("DvTHandler").GetComponent<DragonTigerBetAPI>();
        resultapi = GameObject.FindWithTag("DvTHandler").GetComponent<DvTResultAPI>();
        timerIsRunning = true;
        gamesoonpanel.SetActive(true);
        resultPanel.SetActive(false);
        wintextanim.speed = 0;
        tigercardanim.speed = 0;
        dragoncardanim.speed = 0;
        WinTiger.SetActive(false);
        WinDragon.SetActive(false);
        WinTie.SetActive(false);
        winimage.SetActive(false);
        retryimage.SetActive(false);
        dragon_audio.Play();
        tiger_audio.Play();
    }
    public void ResetTable(string Tag)
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

    public IEnumerator timer()
    {
        AddTime = PlayerPrefs.GetInt("DvT_Game_time");
        if(AddTime<15)
        {
            while(AddTime>0)
            {
                timerText.text = (AddTime - 1).ToString();
                AddTime--;
                yield return new WaitForSecondsRealtime(1);
            }
            yield return new WaitForSecondsRealtime(AddTime);
            StartCoroutine(DragonHandler.CallAPI());
            yield break;
        }
        gamesoonpanel.SetActive(false);
        startpanel.SetActive(true);
        StartCoroutine(resultapi.DragonPastResults()); 
        yield return new WaitForSecondsRealtime(2);
        _manager.tie = 0;
        _manager.dragon = 0;
        _manager.tiger = 0;
        _manager.TotalBets[0].text = "total bet: ₹0";
        _manager.TotalBets[1].text = "total bet: ₹0";
        _manager.TotalBets[2].text = "total bet: ₹0";
        timerIsRunning = true;
        remainingtime = AddTime - 14;
        startpanel.SetActive(false);
        Tiger.interactable = true;
        Dragon.interactable = true;
        Tie.interactable = true;
        _chiproutine = StartCoroutine(FakechipPlace());
        while(timerIsRunning == true)
        {
            yield return new WaitForSecondsRealtime(1);
            if(remainingtime>0)
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
        StopCoroutine(_chiproutine);
        resultclock_audio.time = 0f;
        resultclock_audio.Play();
        stoppanel.SetActive(true);
        Tiger.interactable = false;
        Dragon.interactable = false;
        Tie.interactable = false;
        yield return new WaitForSecondsRealtime(5);
        StartCoroutine(resultapi.DvTBetResult());
        yield return new WaitForSecondsRealtime(1);
        int x = Random.Range(0,4);
        int y = Random.Range(0,4);
        int i = PlayerPrefs.GetInt("tigercardanim");
        int j = PlayerPrefs.GetInt("dragoncardanim");
        if(x.Equals(0))
        {
            tigersprite = spadecardsprites[i-1];
        }  
        else if(x.Equals(1))
        {
            tigersprite = clubcardsprites[i-1];
        }  
        else if(x.Equals(2))
        {
            tigersprite = heartcardsprites[i-1];
        }  
        else
        {
            tigersprite = diamcardsprites[i-1];
        } 
        if(y.Equals(0))
        {
            dragonsprite = spadecardsprites[j-1];
        }  
        else if(y.Equals(1))
        {
            dragonsprite = clubcardsprites[j-1];
        }  
        else if(y.Equals(2))
        {
            dragonsprite = heartcardsprites[j-1];
        }  
        else
        {
            dragonsprite = diamcardsprites[j-1];
        }        
        yield return new WaitForSecondsRealtime(1);
        resultclock_audio.Stop();
        stoppanel.SetActive(false);
        tigercardanim.Play("cardflip",0,0f);
        dragoncardanim.Play("dragoncard",0,0f);
        cardflip_audio.Play();
        tigercardanim.speed = 1;
        dragoncardanim.speed = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        string Tag = "ChipOnTable";
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tag);
        foreach (GameObject target in gameObjects)
        {
            if (target.CompareTag(Tag))
            {
                Destroy(target);
            }
        }
        if(resultapi.winner.Equals("Tiger"))
        {
            tiger_audio.Play();
            WinTiger.SetActive(true);
            WinDragon.SetActive(false);
            WinTie.SetActive(false);
        }
        else if(resultapi.winner.Equals("Dragon"))
        {
            dragon_audio.time = 1f;
            dragon_audio.Play();
            WinTiger.SetActive(false);
            WinDragon.SetActive(true);
            WinTie.SetActive(false);
        }
        else
        {
            WinTiger.SetActive(false);
            WinDragon.SetActive(false);
            WinTie.SetActive(true);
        }
        fakechipsback();
        yield return new WaitForSecondsRealtime(2.5f);
        if(resultapi.isWinner.Equals(0))
        {
            profit.text = "₹" + resultapi.dTProfit.ToString();
            winimage.SetActive(false);
            retryimage.SetActive(true);
        }
        else
        {
            profit.text = "₹" + resultapi.dTProfit.ToString();
            winimage.SetActive(true);
            retryimage.SetActive(false);
        }
        resultPanel.SetActive(true);
        PlayerPrefs.SetString("WalletAmount",winwallet.ToString());
        yield return new WaitForSecondsRealtime(1);
        if(resultapi.isWinner.Equals(1))
        {
            win_audio.time = 0.3f;
            win_audio.Play();
            wintextanim.Play("wintext",0,0f);
            wintextanim.speed = 1;
        }
        yield return new WaitForSecondsRealtime(1);
        resetgame();
        yield return new WaitForSecondsRealtime(1);
        winimage.SetActive(false);
        retryimage.SetActive(false);
        resultPanel.SetActive(false);
        profit.text = "₹0";
        startpanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        wintextanim.speed = 0;
        StartCoroutine(DragonHandler.CallAPI());
    }

    private IEnumerator FakechipPlace()
    {
        while(true)
        {
            chip_audio.Play();
            float k = Random.Range(0.4f,0.7f);
            float l = Random.Range(5,11);
            for (int m = 0; m<l; m++)
            {
                int i = Random.Range(0,7);
                int j = Random.Range(0,5);
                int s = Random.Range(0,3);
                float x;
                float y;
                if(s==2)
                {
                    x = Random.Range(-147f,147f);
                    y = Random.Range(-97f,31f);
                }
                else
                {
                    x = Random.Range(-150f,150f);
                    y = Random.Range(-190f,125f);
                }
                Vector3 position = new Vector3(x,y,0);
                GameObject chip = Instantiate(fakechips[i],localchipParent);
                chip.transform.localPosition = new Vector3(randpositions[j],8,0);
                chip.transform.SetParent(mainChipParent[s]);
                chip.tag = "FakeChips";
                float b = 1f;
                StartCoroutine(startLerp(chip, position,b));
            }
            yield return new WaitForSeconds(k);
        }
    }
    
    private void fakechipsback()
    {
        string sTag = "FakeChips";
        GameObject[] sgameObject = GameObject.FindGameObjectsWithTag(sTag);
        int l = Random.Range(0,2);
        foreach (GameObject target in sgameObject)
        {   
            if (target.CompareTag(sTag))
            {
                int k = Random.Range(0,3);
                target.transform.SetParent(localchipParent);
                float b = 2f;
                if(l == 1)
                {
                    int j = Random.Range(0,3);
                    Vector3 position = new Vector3(randpositions1[j],8,0);
                    StartCoroutine(ChipsBack(target, position,b));
                }
                else
                {
                    int j = Random.Range(0,2);
                    Vector3 position = new Vector3(randpositions2[j],8,0);
                    StartCoroutine(ChipsBack(target, position,b));
                }
            }
        }
    }

    public void Halfanimation()
    {
        tigerimage.sprite = tigersprite;
        dragonimage.sprite = dragonsprite;
    } 

    private void resetgame()
    {
        DragonHandler.totalbet_amount.text = "Your Bet Amount: ₹00";
        tigerimage.sprite = defsprite;
        dragonimage.sprite = defsprite;
        WinTiger.SetActive(false);
        WinDragon.SetActive(false);
        WinTie.SetActive(false);
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

    private IEnumerator ChipsBack(GameObject chip_position, Vector3 box_position, float timeremaining)
    {
        while(timeremaining>0)
        {
            chip_position.transform.localPosition = Vector2.Lerp(chip_position.transform.localPosition,box_position,timeremaining*Time.deltaTime*5);
            timeremaining -= Time.deltaTime;
            yield return null;
        }
        Destroy(chip_position);
    }
}


