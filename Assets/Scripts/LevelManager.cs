using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeEventArgs : EventArgs
{
    public Sprite Sprite { get; set; }
}

public class LevelManager
{
    // 场景改变事件
    public event EventHandler<SceneChangeEventArgs> SceneChangeEvent;

    private LevelManager() { }
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            instance ??= new LevelManager();
            return instance;
        }
    }

    public void SwitchScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCount)
        {
            Debug.LogError("没有下一个场景");
            return;
        }
        ScreenCapturer.Instance.Do(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SwitchScene(string sceneName)
    {
        ScreenCapturer.Instance.Do(sceneName);
    }

    public void SwitchScene(int buildIndex)
    {
        ScreenCapturer.Instance.Do(buildIndex);
    }

    public void RestartScene()
    {
        ScreenCapturer.Instance.Do(SceneManager.GetActiveScene().buildIndex);
    }

    async public void AfterScreenCapture(Texture2D texture2D)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        await Task.Delay(1);
        PlayTransitionAnim(texture2D);
    }

    async public void AfterScreenCapture(Texture2D texture2D,string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        await Task.Delay(1);
        PlayTransitionAnim(texture2D);
    }

    async public void AfterScreenCapture(Texture2D texture2D,int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        await Task.Delay(1);
        PlayTransitionAnim(texture2D);
    }

    public void PlayTransitionAnim(Texture2D texture2D)
    {
        SceneChangeEvent?.Invoke(this, new() { Sprite = Sprite.Create(texture2D, new Rect(Vector2.zero, texture2D.Size()), new Vector2(0.5f, 0.5f)) });
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
