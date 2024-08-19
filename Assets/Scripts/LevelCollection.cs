using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollection : MonoBehaviour
{
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Sprite> sprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private string LevelCollectionNumber;
    private GameObject indicator;
    private GameObject transitionShower;


    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        indicator = transform.Find("Indicator").gameObject;
        transitionShower = transform.Find("TransitionShower").gameObject;
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
        spriteRenderer.sprite = sprites[args.CurResolutionRatio];
        if (args.CurResolutionRatio == 16)
            indicator.SetActive(true);
        else
            indicator.SetActive(false);
        PlayTransitionAnim(args.CurResolutionRatio, args.PrevResolutionRatio);
    }

    private void PlayTransitionAnim(int curResolutionRatio, int prevResolutionRatio)
    {
        if (curResolutionRatio == prevResolutionRatio)
            return;
        StopAllCoroutines();
        GameObject fromObj, toObj;
        fromObj = transitionShower;
        toObj = gameObject;
        fromObj.GetComponent<SpriteRenderer>().sprite = sprites[prevResolutionRatio];
        StartCoroutine(TransitionAnim(fromObj, toObj));
    }

    IEnumerator TransitionAnim(GameObject fromObj, GameObject toObj)
    {
        float deltaAlpha = GlobalParam.transformAnimDeltaAlpha;
        float deltaTime = GlobalParam.transformAnimDeltaTime;
        SpriteRenderer srFrom, srTo;
        Color colorFrom, colorTo;
        srFrom = fromObj.GetComponent<SpriteRenderer>();
        srTo = toObj.GetComponent<SpriteRenderer>();
        colorFrom = srFrom.color;
        colorTo = srTo.color;
        colorFrom.a = 1;
        colorTo.a = 0;

        fromObj.SetActive(true);
        while (colorFrom.a > 0)
        {
            colorFrom.a -= deltaAlpha;
            colorTo.a += deltaAlpha;
            srFrom.color = colorFrom;
            srTo.color = colorTo;
            yield return new WaitForSeconds(deltaTime);
        }
        fromObj.SetActive(false);
    }

    private void GetLevelCollection()
    {
        SoundManager.Instance.SceneEffectPlayStr("14");
        PlayerPrefs.SetInt("LC" + LevelCollectionNumber, 1);
        Destroy(gameObject);
    }
}
