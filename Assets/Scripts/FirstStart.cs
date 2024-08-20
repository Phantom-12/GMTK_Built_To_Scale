using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStart : MonoBehaviour
{
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject continueButton;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        if(PlayerPrefs.GetInt("FS", 0) == 1)
        {
            startButton.SetActive(false);
            continueButton.SetActive(true);
        }
    }

}
