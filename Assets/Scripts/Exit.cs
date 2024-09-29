using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("CurrentLevel", int.Parse(sceneName) - 1);
        PlayerPrefs.Save();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (sceneName == "9")
            {
                SoundManager.Instance.StopSound();
            }
            PlayerPrefs.SetInt("L" + sceneName, 1);
            PlayerPrefs.SetInt("CurrentLevel", int.Parse(sceneName));
            PlayerPrefs.Save();
            ScreenCapturer.Instance.Do("Level" + sceneName);
        }
    }

}
