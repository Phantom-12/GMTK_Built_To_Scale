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
            if(rectTransform.name != "BackGround" && rectTransform.name != "ScaleAdaption")
            {
                if (CanvasRectTransform.rect.width < 1920f)
                {
                    if (rectTransform.name == "MidAdaption")
                    {
                        foreach (RectTransform childRectTransform in rectTransform)
                        {
                            if (childRectTransform.anchoredPosition.x > 0)
                            {
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x + (childRectTransform.rect.width * (childRectTransform.localScale.x * (1f - CanvasRectTransform.rect.width / 1920f))) / 2f, childRectTransform.anchoredPosition.y);
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x + MathF.Abs(childRectTransform.anchoredPosition.x + childRectTransform.rect.width / 2f) / (CanvasRectTransform.rect.width - 1920f) / 2f, childRectTransform.anchoredPosition.y);
                            }
                            else if (childRectTransform.anchoredPosition.x < 0)
                            {
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x - (childRectTransform.rect.width * (childRectTransform.localScale.x * (1f - CanvasRectTransform.rect.width / 1920f))) / 2f, childRectTransform.anchoredPosition.y);
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x - MathF.Abs(childRectTransform.anchoredPosition.x - childRectTransform.rect.width / 2f) / (CanvasRectTransform.rect.width - 1920f) / 2f, childRectTransform.anchoredPosition.y);
                            }
                            else if (childRectTransform.anchoredPosition.x == 0) return;
                        }
                    }
                    else
                    {
                        if (rectTransform.anchoredPosition.x > 0)
                        {
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (rectTransform.rect.width * (rectTransform.localScale.x * (1f - CanvasRectTransform.rect.width / 1920f))) / 2f, rectTransform.anchoredPosition.y);
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                        }
                        else if (rectTransform.anchoredPosition.x < 0)
                        {
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (rectTransform.rect.width * (rectTransform.localScale.x * (1f - CanvasRectTransform.rect.width / 1920f))) / 2f, rectTransform.anchoredPosition.y);
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                        }
                        else if (rectTransform.anchoredPosition.x == 0) return;
                    }
                    rectTransform.localScale = new Vector2(rectTransform.localScale.x * (CanvasRectTransform.rect.width / 1920f), rectTransform.localScale.y * (CanvasRectTransform.rect.width / 1920f));
                }
                else if (Screen.width == 1920)
                {
                    return;
                }
                else if (CanvasRectTransform.rect.width > 1920f)
                {
                    if(rectTransform.name == "MidAdaption")
                    {
                        foreach (RectTransform childRectTransform in rectTransform)
                        {
                            if (childRectTransform.anchoredPosition.x > 0)
                            {
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x + MathF.Abs(childRectTransform.anchoredPosition.x + childRectTransform.rect.width / 2f) / (CanvasRectTransform.rect.width - 1920f) / 2f, childRectTransform.anchoredPosition.y);
                            }
                            else if (childRectTransform.anchoredPosition.x < 0)
                            {
                                childRectTransform.anchoredPosition = new Vector2(childRectTransform.anchoredPosition.x - MathF.Abs(childRectTransform.anchoredPosition.x - childRectTransform.rect.width / 2f) / (CanvasRectTransform.rect.width - 1920f) / 2f, childRectTransform.anchoredPosition.y);
                            }
                            else if (childRectTransform.anchoredPosition.x == 0) return;
                        }
                    }
                    else
                    {
                        if (rectTransform.anchoredPosition.x > 0)
                        {
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                        }
                        else if (rectTransform.anchoredPosition.x < 0)
                        {
                            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (CanvasRectTransform.rect.width - 1920f) / 2f, rectTransform.anchoredPosition.y);
                        }
                        else if (rectTransform.anchoredPosition.x == 0) return;
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
