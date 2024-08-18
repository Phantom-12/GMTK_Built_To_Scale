using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public GameObject[] Levels;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject level in Levels)
        {
            level.GetComponent<Button>().interactable = ResolutionScalerManager.Instance.GetLevelEnable(level.name);
            if (ResolutionScalerManager.Instance.GetLevelEnable(level.name))
            {
                level.GetComponent<Image>().color = Color.white;
            }
        }
    }

}
