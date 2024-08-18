using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class ResolutionRatioChangedEventArgs : EventArgs
{
    public int PrevResolutionRatio { get; set; } //改变前分辨率
    public int CurResolutionRatio { get; set; } //当前分辨率
}

public class PauseStateChangedEventArgs : EventArgs
{
    public bool IsPause { get; set; }
}

public class GameData
{
    // 分辨率及其变化事件
    private int resolutionRatio = 0; // 1 2 4 8 16
    private Dictionary<int, bool> resolutionRatioEnable = new(){
        {16,true},
        {8,true},
        {4,true},
        {2,true},
        {1,true},
    };
    public event EventHandler<ResolutionRatioChangedEventArgs> ResolutionRatioChangedEvent;

    // 暂停及其改变事件
    private bool isPause = false;
    public event EventHandler<PauseStateChangedEventArgs> PauseStateChangedEvent;

    private GameData() { }
    private static GameData instance;
    public static GameData Instance
    {
        get
        {
            instance ??= new GameData();
            return instance;
        }
    }

    private void OnResolutionRatioChanged(ResolutionRatioChangedEventArgs args)
    {
        ResolutionRatioChangedEvent?.Invoke(this, args);
    }

    public int GetResolutionRatio()
    {
        return resolutionRatio;
    }

    public void SetResolutionRatio(int resolutionRatio)
    {
        if (!resolutionRatioEnable.ContainsKey(resolutionRatio))
        {
            Debug.LogError("只能设置分辨率为1,2,4,8,16之一，你设置了" + resolutionRatio);
            return;
        }
        if (!resolutionRatioEnable[resolutionRatio])
        {
            Debug.Log("设置了暂时不能使用的值：" + resolutionRatio);
            return;
        }
        if (this.resolutionRatio == resolutionRatio)
            return;
        ResolutionRatioChangedEventArgs args = new()
        {
            PrevResolutionRatio = resolutionRatioEnable.ContainsKey(this.resolutionRatio) ? this.resolutionRatio : resolutionRatio,
            CurResolutionRatio = resolutionRatio
        };
        this.resolutionRatio = resolutionRatio;
        OnResolutionRatioChanged(args);
    }

    public void InitResolutonRatio(List<int> avaliableResolutionRatios, int startResolutionRatio)
    {
        resolutionRatio = 0;
        var keys = new List<int>(resolutionRatioEnable.Keys);
        for (int i = 0; i < keys.Count; i++)
            resolutionRatioEnable[keys[i]] = false;
        foreach (var i in avaliableResolutionRatios)
        {
            if (!resolutionRatioEnable.ContainsKey(i))
            {
                Debug.LogError("只能初始化分辨率为1,2,4,8,16之一，你设置了" + i);
                return;
            }
            resolutionRatioEnable[i] = true;
        }
        SetResolutionRatio(startResolutionRatio);
    }

    public void ChangeResolutionRatioEnable(int ratio)
    {
        resolutionRatioEnable[ratio] = true;
    }

    public bool GetResolutionRatioEnable(int ratio)
    {
        return resolutionRatioEnable[ratio];
    }

    private void OnPauseStateChanged(PauseStateChangedEventArgs args)
    {
        PauseStateChangedEvent?.Invoke(this, args);
    }

    public bool GetIsPause()
    {
        return isPause;
    }

    public void SetIsPause(bool pause)
    {
        if (isPause == pause)
            return;
        PauseStateChangedEventArgs args = new()
        {
            IsPause = pause
        };
        isPause = pause;
        OnPauseStateChanged(args);
    }
}
