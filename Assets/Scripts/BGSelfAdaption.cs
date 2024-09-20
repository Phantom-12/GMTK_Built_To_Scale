using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSelfAdaption : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        gameObject.transform.localScale = new Vector3(MathF.Round((screenWidth/screenHeight)/(1920f/1080f),3),1, 1);
    }

}
