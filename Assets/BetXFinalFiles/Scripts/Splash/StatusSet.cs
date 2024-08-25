using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusSet : MonoBehaviour
{
    private int loadSceneLevel;
    void Awake()
    {
        PlayerPrefs.SetInt("notice",1);
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        LoadLoginScene();
    }  
    
    private void LoadLoginScene()
    {
        if(!PlayerPrefs.HasKey("isLoggedin"))
        {
            loadSceneLevel = 1;
            StartCoroutine(sceneLoading(loadSceneLevel));
        }
        else
        {
            loadSceneLevel = 3;
            StartCoroutine(sceneLoading(loadSceneLevel));
        }
    }
 
    private IEnumerator sceneLoading(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)

        {
            yield return null;
        }
    }
}
