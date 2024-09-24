using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGSelfAdaption : MonoBehaviour
{
    [SerializeField]
    RectTransform CanvasRectTransform;
    [SerializeField]
    RectTransform parentRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        if(parentRectTransform != null)
        {
            parentRectTransform.localScale = new Vector2(parentRectTransform.localScale.x * (CanvasRectTransform.rect.width / 1920f),parentRectTransform.localScale.y);
        }
        else
        {
            if (gameObject.GetComponent<Transform>() != null)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * MathF.Round(CanvasRectTransform.rect.width / 1920f, 3), gameObject.transform.localScale.y, 1);
            }
        }

    }

}
