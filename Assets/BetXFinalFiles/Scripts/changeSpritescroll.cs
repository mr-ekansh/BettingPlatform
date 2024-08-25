using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSpritescroll : MonoBehaviour
{
    public Sprite newSprite;
    public Image img;
    public Sprite oldSprite;

    public void ChangeSprite()
    {
        if(img.sprite == oldSprite)
        {
            img.sprite = newSprite;
        }
        else
        {
            img.sprite = oldSprite;
        }
    }
}
