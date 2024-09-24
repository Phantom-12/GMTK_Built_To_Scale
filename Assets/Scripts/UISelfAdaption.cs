using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISelfAdaption : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform CanvasRectTransform= GetComponent<RectTransform>();
        foreach (RectTransform rectTransform  in transform)
        {
            if(rectTransform.name != "BackGround" && rectTransform.name != "Adaption")
            {
                if (CanvasRectTransform.rect.width < 1920f)
                {
                    if (rectTransform.anchoredPosition.x > 0)
                    {
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (rectTransform.rect.width * (rectTransform.localScale.x * (1f -CanvasRectTransform.rect.width / 1920f))) /2f, rectTransform.anchoredPosition.y);
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                    }
                    else
                    {
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (rectTransform.rect.width * (rectTransform.localScale.x * (1f - CanvasRectTransform.rect.width / 1920f))) /2f, rectTransform.anchoredPosition.y);
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                    }
                    rectTransform.localScale = new Vector2(rectTransform.localScale.x * (CanvasRectTransform.rect.width / 1920f), rectTransform.localScale.y * (CanvasRectTransform.rect.width / 1920f));
                }
                else
                {
                    if (rectTransform.anchoredPosition.x > 0)
                    {
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                    }
                    else
                    {
                        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                    }
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
