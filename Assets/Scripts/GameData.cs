using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ResolutionRatioChangedEventArgs : EventArgs
{
    public int ResolutionRatio { get; set; }
}

public class GameData
{
    private int resolutionRatio; // 1 2 4 8 16
    private event EventHandler<ResolutionRatioChangedEventArgs> ResolutionRatioChangedEvent;

    private static readonly GameData instance = new();
    private GameData() { }
    public static GameData GetInstance()
    {
        return instance;
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
        if(this.resolutionRatio == resolutionRatio)
            return;
        this.resolutionRatio = resolutionRatio;
        ResolutionRatioChangedEventArgs args = new()
        {
            ResolutionRatio = resolutionRatio
        };
        OnResolutionRatioChanged(args);
    }
}
