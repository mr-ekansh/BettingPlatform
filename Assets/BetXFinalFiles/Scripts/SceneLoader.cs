using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
   void Awake()
   {
      Screen.sleepTimeout = SleepTimeout.NeverSleep;
   }

   public Slider slider;
   public TextMeshProUGUI text;

   IEnumerator LoadAsynchronously (int sceneIndex)
   {
      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
      while (!operation.isDone)  
      {
         float progress = Mathf.Clamp01(operation.progress / .9f);
         if(slider != null)
         {
            slider.value = progress;
         }    
         yield return null;
      }
   }
   
   public void LoginPage()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      SceneManager.LoadScene(1);
   }
  
   public void Verified()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      SceneManager.LoadScene(2);
   }

   public void HomePage()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      SceneManager.LoadScene(3);
   }

   public void ReferPage()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      SceneManager.LoadScene(12);
   }
 
   public void WalletScreen()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      PlayerPrefs.SetInt("walletscreencash", 0);
      SceneManager.LoadScene(6);
   }
 
   public void GaliScreen()
   {
      Screen.orientation = ScreenOrientation.Portrait;
      SceneManager.LoadScene(5);
   }
   public void OpenMatka()
   {
      Screen.orientation = ScreenOrientation.Portrait;
      SceneManager.LoadScene(4);
   }

   public void OpenDragonVsTiger()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      StartCoroutine(LoadAsynchronously(7));
   }

   public void OpenRoulette()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      StartCoroutine(LoadAsynchronously(9));
   }

   public void OpenMyMatches()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      SceneManager.LoadScene(11);
   }

   public void Addcash()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      PlayerPrefs.SetInt("walletscreencash", 1);
      SceneManager.LoadScene(6);
   }

   public void Withdrawcash()
   {
      Screen.orientation = ScreenOrientation.Landscape;
      PlayerPrefs.SetInt("walletscreencash", 2);
      SceneManager.LoadScene(6);
   }
}

