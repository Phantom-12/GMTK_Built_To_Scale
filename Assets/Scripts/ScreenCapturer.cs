using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapturer : MonoBehaviour
{
    private ScreenCapturer() { }
    public static ScreenCapturer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Do()
    {
        StartCoroutine(DoCor());
    }

    public void Do(string sceneName)
    {
        StartCoroutine(DoCor(sceneName));
    }

    public void Do(int buildIndex)
    {
        StartCoroutine(DoCor(buildIndex));
    }

    IEnumerator DoCor()
    {
        yield return new WaitForEndOfFrame();
        var tex = ScreenCapture.CaptureScreenshotAsTexture();
        LevelManager.Instance.AfterScreenCapture(tex);
    }

    IEnumerator DoCor(string sceneName)
    {
        yield return new WaitForEndOfFrame();
        var tex = ScreenCapture.CaptureScreenshotAsTexture();
        LevelManager.Instance.AfterScreenCapture(tex, sceneName);
    }

    IEnumerator DoCor(int buildIndex)
    {
        yield return new WaitForEndOfFrame();
        var tex = ScreenCapture.CaptureScreenshotAsTexture();
        LevelManager.Instance.AfterScreenCapture(tex, buildIndex);
    }
}
