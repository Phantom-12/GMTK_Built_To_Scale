using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    Button button;
    [SerializeField] string sceneName;
    [SerializeField] bool rePlayMusic;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if(sceneName == "StartScene")
            {
                if(rePlayMusic)
                {
                    SoundManager.Instance.MusicPlayStr("1");
                    SoundManager.Instance.StopSound();
                }
            }
            ScreenCapturer.Instance.Do(sceneName);
        });
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

