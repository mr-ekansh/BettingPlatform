using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCanvas : MonoBehaviour
{
    public GameObject discanvas;
    public Animator myanim;
    public Animator halfAnim;

    public void Dismycanvas()
    {
        discanvas.SetActive(false);
        myanim.enabled = false;
    }

    public void Openanimation()
    {
        myanim.enabled = true;
        myanim.Play("halfprofileopen",0,0f);
    }

    public void HalfPanelClose()
    {
        halfAnim.enabled = true;
        halfAnim.Play("HalfProfiledrag",0,0f);
    }
}
