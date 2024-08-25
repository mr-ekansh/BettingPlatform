using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Messaging;
using UnityEngine;

public class Firebasenotifs : MonoBehaviour 
{
    void Awake() 
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived (object sender, Firebase.Messaging.TokenReceivedEventArgs token) 
    {
        PlayerPrefs.SetString("FireBaseToken", token.Token);
    }
    public void OnMessageReceived (object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
    }

}