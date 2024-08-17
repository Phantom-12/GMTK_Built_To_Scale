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
    private int resolutionRatio = 16; // 1 2 4 8 16
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
        int[] posibleVal = { 1, 2, 4, 8, 16 };
        if (!posibleVal.Contains(resolutionRatio))
        {
            Debug.LogError("只能设置分辨率为1,2,4,8,16之一，你设置了" + resolutionRatio);
            return;
        }
        if (this.resolutionRatio == resolutionRatio)
            return;
        ResolutionRatioChangedEventArgs args = new()
        {
            PrevResolutionRatio = this.resolutionRatio,
            CurResolutionRatio = resolutionRatio
        };
        this.resolutionRatio = resolutionRatio;
        OnResolutionRatioChanged(args);
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
