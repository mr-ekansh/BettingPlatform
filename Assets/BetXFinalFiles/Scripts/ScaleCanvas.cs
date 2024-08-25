using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleCanvas : MonoBehaviour
{
    public CanvasScaler[] _canvas;
    public Camera maincam;

    void Update()
    {
        if(maincam.aspect > 1.5f)
        {
            for(int i = 0; i<_canvas.Length; i++)
            {
                _canvas[i].matchWidthOrHeight = 1;
            }
        }
        else
        {
            for(int i = 0; i<_canvas.Length; i++)
            {
                _canvas[i].matchWidthOrHeight = 0;
            }
        }
    }
}
