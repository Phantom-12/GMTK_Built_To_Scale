using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTBUTTON : MonoBehaviour
{
    public void Click(int resolutionRatio)
    {
        GameData.Instance.SetResolutionRatio(resolutionRatio);
    }

    public void Pause(bool enable)
    {
        GameController.Instance.SetPause(enable);
    }

    public void RestartScene()
    {
        LevelManager.Instance.RestartScene();
    }
}
