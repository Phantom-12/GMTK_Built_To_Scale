using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSelfAdaption : MonoBehaviour
{
    [SerializeField]
    RectTransform CanvasRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        CanvasRectTransform= GetComponent<RectTransform>();
        gameObject.transform.localScale = new Vector3(MathF.Round(CanvasRectTransform.rect.width / 1920f,3),CanvasRectTransform.rect.height / 1080f, 1);
    }

}
