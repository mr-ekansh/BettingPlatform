using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chipscroll : MonoBehaviour
{
    public ScrollRect _scroller;
    public GameObject scroll_content;
    public Toggle[] chips_tgl;
    public Transform[] chips_transform;
    public AudioSource chip_scrollaud;
    public Transform Arrow_trns;
    
    void Update()
    { 
        for(int i = 0; i<chips_tgl.Length; i++)
        {
            if(chips_tgl[i].isOn == true)
            {
                Arrow_trns.SetParent(chips_transform[i],false);
                chips_transform[i].localScale = new Vector3(0.1f,0.098f, 0);
            }
            else
            {
                chips_transform[i].localScale = new Vector3(0.09f, 0.088f, 0);
            }
        }
    }
    public void OnScroll()
    {
        chip_scrollaud.Play();
    }
    public void rightscroll()
    {
        if(scroll_content.transform.localPosition.x<-4)
        {
            StartCoroutine(left());
        }
    }
    public void leftscroll()
    {
        if(scroll_content.transform.localPosition.x>-135)
        {
            StartCoroutine(right());
        }
    }

    private IEnumerator left()
    {
        int i = 0;
        while(i<7)
        {
            _scroller.horizontalNormalizedPosition -= 0.0203f;
            yield return new WaitForSeconds(0);
            i++;
        }
    }

    private IEnumerator right()
    {
        int i = 0;
        while(i<7)
        {
            _scroller.horizontalNormalizedPosition += 0.0203f;
            yield return new WaitForSeconds(0);
            i++;
        }
    }
}

