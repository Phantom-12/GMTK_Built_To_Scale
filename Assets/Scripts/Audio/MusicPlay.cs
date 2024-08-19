using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlay : MonoBehaviour
{
    [SerializeField]
    private bool isGaming;
    [SerializeField]
    private bool iscomic;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("chooseMusic",0f);
    }
    void chooseMusic()
    {
        if (isGaming)
        {
            SoundManager.Instance.PlaySound();
            SoundManager.Instance.MusicStop();
        }
        if (!isGaming)
        {
            SoundManager.Instance.MusicPlayStr("1");
            SoundManager.Instance.StopSound();
        }
        if (iscomic)
        {
            SoundManager.Instance.MusicStop();
        }

    }
}
