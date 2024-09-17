using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip enterAudioClip;
    public AudioClip exitAudioClip;
    bool play1 = false;
    bool play2 = false;
    [SerializeField]
    float sTime = 0.5f;
    bool start = false;
    [SerializeField]
    float eTime = 1f;
    bool end = false;
    [SerializeField]
    float lastTime = 5.5f;

    void Update()
    {
        sTime -= Time.deltaTime;
        if (sTime < 0)
        {
            if (!start)
            {
                if (!play1)
                {
                    audioSource.PlayOneShot(enterAudioClip, 1f);
                    play1 = true;
                    start = true;
                }

            }
            if (start)
            {
                lastTime -= Time.deltaTime;
                if (lastTime <= 0)
                {
                    if (!play2)
                    {
                        audioSource.PlayOneShot(exitAudioClip, 1f);
                        play2 = true;
                    }
                    eTime -= Time.deltaTime;
                    if (eTime <= 0)
                    {
                        end = true;
                    }
                }
            }
        }
        if (end)
        {
            ScreenCapturer.Instance.Do("StartScene");
        }
    }
}
