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

public class SceneChangeEventArgs1 : EventArgs
{
    public Camera Camera { get; set; }
    public Scene OldScene { get; set; }
}

public class LevelManager
{
    // 场景改变事件
    public event EventHandler<SceneChangeEventArgs> SceneChangeEvent;
    public event EventHandler<SceneChangeEventArgs1> SceneChangeEvent1;

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

    async public void AfterScreenCapture(Texture2D texture2D, string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        await Task.Delay(1);
        PlayTransitionAnim(texture2D);
    }

    async public void AfterScreenCapture(Texture2D texture2D, int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        await Task.Delay(1);
        PlayTransitionAnim(texture2D);
    }

    public void PlayTransitionAnim(Texture2D texture2D)
    {
        SceneChangeEvent?.Invoke(this, new() { Sprite = Sprite.Create(texture2D, new Rect(Vector2.zero, texture2D.Size()), new Vector2(0.5f, 0.5f)) });
    }

    public void SwitchScene1()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCount)
        {
            Debug.LogError("没有下一个场景");
            return;
        }
        GameObject oldCamera = new();
        SwitchPrework(ref oldCamera);
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        PlayTransitionAnim1(oldCamera.GetComponent<Camera>(), curScene);
    }

    public void SwitchScene1(string sceneName)
    {
        GameObject playerOld = new();
        SwitchPrework(ref playerOld);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void SwitchScene1(int buildIndex)
    {
        GameObject playerOld = new();
        SwitchPrework(ref playerOld);
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
    }

    public void RestartScene1()
    {
        GameObject oldCamera = new();
        SwitchPrework(ref oldCamera);
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Additive);
        PlayTransitionAnim1(oldCamera.GetComponent<Camera>(), curScene);
    }

    private void SwitchPrework(ref GameObject oldCamera)
    {
        Scene curScene = SceneManager.GetActiveScene();
        var items = curScene.GetRootGameObjects();
        Dictionary<string, bool> names = new(){
            {"Global Light 2D",true},
            {"GameController",true},
            {"EventSystem",true},
        };
        foreach (var item in items)
        {
            item.transform.position = item.transform.position + new Vector3(1000, 1000);
            if (names.ContainsKey(item.name))
            {
                item.SetActive(false);
                UnityEngine.Object.DestroyImmediate(item);
            }
            else if (item.name == "Main Camera")
            {
                item.layer = 8;
                oldCamera = item;
                oldCamera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Player");
                oldCamera.GetComponent<Camera>().depth = 10;
            }
            else if (item.name == "Player Camera")
            {
                item.layer = 8;
            }
            else if (item.name == "PlayerAndFloater")
            {
                item.transform.Find("Player").gameObject.layer = 8;
            }
        }
    }

    public void PlayTransitionAnim1(Camera camera, Scene oldScene)
    {
        SceneChangeEvent1?.Invoke(this, new() { Camera = camera, OldScene = oldScene });
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
