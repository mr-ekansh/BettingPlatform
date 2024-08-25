using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetEditProfile : MonoBehaviour
{
    public TMP_InputField EditName;
    public TMP_InputField Editemail;
    public TMP_InputField Editcity;
    public TMP_Text Editdob;
    public TMP_Dropdown EditGender;    
    public RawImage halfProfileImage;
    public RawImage fullProfileImage;
    public Slider slider;
    public TMP_Text percenttext;
    void Start()
    {   percenttext.text = PlayerPrefs.GetInt("ProfilePercentage").ToString() + "% PROFILE COMPLETED";
        float value;
        value = PlayerPrefs.GetInt("ProfilePercentage");
        value = value/100;
        slider.value = value;
        EditName.text = PlayerPrefs.GetString("Name");
        Editemail.text = PlayerPrefs.GetString("email");
        Editcity.text = PlayerPrefs.GetString("city");
        string imagepath = PlayerPrefs.GetString("ProfileImagePath");
        if(System.String.IsNullOrEmpty(PlayerPrefs.GetString("date")))
        {}
        else
        {
            Editdob.text = PlayerPrefs.GetString("date");
        }

        if(PlayerPrefs.GetString("gender") == "Male")
        {
            EditGender.value = 0;
        }
        else
        {
            EditGender.value = 1;
        }
        StartCoroutine(GetAvatarImage(imagepath, halfProfileImage));
    }
    
    IEnumerator GetAvatarImage(string x, RawImage y)
    {
        UnityWebRequest ImageGames = UnityWebRequestTexture.GetTexture(x);
        yield return ImageGames.SendWebRequest();

        if (ImageGames.error != null)
        {
        }
        else
        {               
            Texture2D img = ((DownloadHandlerTexture)ImageGames.downloadHandler).texture;
            y.texture = img;
            fullProfileImage.texture = img;
        }
    }
}
