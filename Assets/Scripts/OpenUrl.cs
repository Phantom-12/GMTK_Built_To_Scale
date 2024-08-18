using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public string url;

    public void Url()
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        Application.OpenURL(url);
    }
}
