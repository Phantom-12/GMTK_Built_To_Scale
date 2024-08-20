using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollection : MonoBehaviour
{
    [SerializeField]
    private string LevelCollectionNumber;
    [SerializeField]
    private GameObject indicator;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        indicator = transform.Find("Indicator").gameObject;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("LC"+LevelCollectionNumber) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        switch (GameData.Instance.GetResolutionRatio())
        {
            case 16:
                GetLevelCollection();
                break;
            case 8:
            case 4:
            case 2:
            case 1:
                break;
        }
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        if (args.CurResolutionRatio == 16)
        {
            indicator.SetActive(true);
            animator.enabled = true;
        }
        else
        {
            indicator.SetActive(false);
            animator.enabled = false;
            spriteRenderer.sprite = null;
        }
    }



    private void GetLevelCollection()
    {
        SoundManager.Instance.SceneEffectPlayStr("14");
        PlayerPrefs.SetInt("LC" + LevelCollectionNumber, 1);
        Destroy(gameObject);
    }
}
