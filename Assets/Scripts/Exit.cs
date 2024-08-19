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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("L" + sceneName, 1);
            ScreenCapturer.Instance.Do(sceneName);
        }
    }

}
