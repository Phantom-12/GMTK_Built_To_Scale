using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionRatio : MonoBehaviour
{
    [SerializeField]
    private string resolutionRationumber;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(resolutionRationumber + "x") == 1)
        {
            Destroy(gameObject);
        }
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
            PlayerPrefs.SetInt(resolutionRationumber + "x", 1);
            int currentResolutionRatio = GameData.Instance.GetResolutionRatio();
            GameData.Instance.SetResolutionRatio(int.Parse(resolutionRationumber));
            if (currentResolutionRatio > GameData.Instance.GetResolutionRatio())
            {
                SoundManager.Instance.SceneEffectPlayStr("7");
            }
            if (currentResolutionRatio < GameData.Instance.GetResolutionRatio())
            {
                SoundManager.Instance.SceneEffectPlayStr("8");
            }
            Destroy(gameObject);
        }
    }
}
