using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagement : MonoBehaviour
{
    public Toggle _toggle;
    public AudioSource[] _audio;
    public Image toggleimage;
    public Sprite[] _sprite;
    public void OnToggle()
    {
        for(int i = 0; i<_audio.Length; i++)
        {
            _audio[i].mute = !_audio[i].mute;
        }
        if(_toggle.isOn == true)
        {
            toggleimage.sprite = _sprite[0];
        }
        else
        {
            toggleimage.sprite = _sprite[1];
        }
    }

}
