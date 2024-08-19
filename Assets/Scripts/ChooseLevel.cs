using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public GameObject[] Levels;
    public GameObject[] LevelsCollection;
    private void Awake()
    {
        PlayerPrefs.SetInt("L1", 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject level in Levels)
        {
            if(PlayerPrefs.GetInt("L"+level.name,0) == 1)
            {
                level.SetActive(true);
            }  
        }
        foreach (GameObject levelcollection in LevelsCollection)
        {
            if (PlayerPrefs.GetInt("LC" + levelcollection.name, 0) == 1)
            {
                levelcollection.SetActive(true);
            }
        }
    }

}
