using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSelfAdaption : MonoBehaviour
{
    [SerializeField]
    private GameObject Bg;
    // Start is called before the first frame update
    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Bg.transform.localScale = new Vector3(MathF.Round(screenWidth/1920f,3), MathF.Round(screenHeight /1080f,3), 1);
    }

}
