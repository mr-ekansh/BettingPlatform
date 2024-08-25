using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public int loadSceneLevel;
    public float waitTimetoLoadLevel;
    public Slider slider;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        LoadLoginScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadLoginScene()
    {
        StartCoroutine(sceneLoading(loadSceneLevel));
    }
 
    private IEnumerator sceneLoading(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)

        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            if(slider != null)
            {
                slider.value = progress;
            }
            if (text != null)
            {
                string progressPercent = (Mathf.Round(progress * 100)) + "%";
                text.text = progressPercent;
    
            }
            yield return null;

        }
    }
}
