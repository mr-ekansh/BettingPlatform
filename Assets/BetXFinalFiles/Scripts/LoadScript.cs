using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadScript : MonoBehaviour
{
    public TMP_Text load;
    void Start()
    {
        StartCoroutine(loading());
    }

    private IEnumerator loading()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.4f);
            load.text = "Loading.";
            yield return new WaitForSeconds(0.4f);
            load.text = "Loading..";
            yield return new WaitForSeconds(0.4f);
            load.text = "Loading...";
            yield return new WaitForSeconds(0.4f);
            load.text = "Loading";
        }
    }

}
