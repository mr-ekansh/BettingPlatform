using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangespriteDvT : MonoBehaviour
{
    private GameTimerDvT timer;

    void Start()
    {
        timer = GameObject.FindWithTag("DvTHandler").GetComponent<GameTimerDvT>();
    }

    public void ChangeSpriteAnim()
    {
        timer.Halfanimation();
    }
}
