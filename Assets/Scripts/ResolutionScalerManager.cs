using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResolutionScalerManager : MonoBehaviour
{
    private Dictionary<string, bool> ResolutionRatioEnable = new Dictionary<string, bool>();
    private Dictionary<string, bool> LevelEnable = new Dictionary<string, bool>();
    public static ResolutionScalerManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ResolutionRatioEnable.Add("16", true);
        ResolutionRatioEnable.Add("8", true);
        ResolutionRatioEnable.Add("4", false);
        ResolutionRatioEnable.Add("2", false);
        ResolutionRatioEnable.Add("1", false);
        LevelEnable.Add("1", true);
        LevelEnable.Add("2", false);
        LevelEnable.Add("3", false);
        LevelEnable.Add("4", false);
        LevelEnable.Add("5", false);
        LevelEnable.Add("6", false);
        LevelEnable.Add("7", false);
        LevelEnable.Add("8", false);
    }
    public void ChangeResolutionRatioEnable(string ratio)
    {
        ResolutionRatioEnable[ratio] = true;
    }
    public bool GetResolutionRatioEnable(string ratio)
    {
        return ResolutionRatioEnable[ratio];
    }
    public void ChangeLevelEnable(string number)
    {
        LevelEnable[number] = true;
    }
    public bool GetLevelEnable(string number)
    {
        return LevelEnable[number];
    }
}
