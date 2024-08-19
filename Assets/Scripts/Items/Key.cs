using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Key : SerializedMonoBehaviour
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
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Sprite> noKeySprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    private LockedDoor lockedDoor;

    private SpriteRenderer spriteRenderer;
    private Player player;
    private GameObject indicator;
    private GameObject transitionShower;

    private bool used;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<Player>();
        indicator = transform.Find("Indicator").gameObject;
        transitionShower = transform.Find("TransitionShower").gameObject;
    }

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (used)
            return;
        switch (GameData.Instance.GetResolutionRatio())
        {
            case 16:
                GetKey();
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
        if (!used)
            spriteRenderer.sprite = sprites[args.CurResolutionRatio];
        else
            spriteRenderer.sprite = noKeySprites[args.CurResolutionRatio];
        if (args.CurResolutionRatio == 16 && !used)
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
        if (!used)
            fromObj.GetComponent<SpriteRenderer>().sprite = sprites[prevResolutionRatio];
        else
            fromObj.GetComponent<SpriteRenderer>().sprite = noKeySprites[prevResolutionRatio];
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

    private void GetKey()
    {
        SoundManager.Instance.SceneEffectPlayStr("14");
        player.EnableKeyFloaterGold(true);
        lockedDoor.SetHasKey();
        used = true;
        spriteRenderer.sprite = noKeySprites[GameData.Instance.GetResolutionRatio()];
    }
}
