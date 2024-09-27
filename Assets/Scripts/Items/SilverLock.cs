using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class SilverLock : SerializedMonoBehaviour
{
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    readonly Dictionary<int, Sprite> lockedSprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    readonly Dictionary<int, Sprite> unlockedSprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    private readonly bool isNeedkey = true;
    private bool hasKey = false;
    private bool locked = true;

    private SpriteRenderer spriteRenderer;
    private Player player;
    [SerializeField]
    private MechanicalGate mechanicalGate;
    [SerializeField]
    private Circuit circuit;
    private GameObject transitionShower;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<Player>();
        transitionShower = transform.Find("TransitionShower").gameObject;
    }

    private void Start()
    {
        if (!isNeedkey)
            Unlock();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        switch (GameData.Instance.GetResolutionRatio())
        {
            case 16:
            case 8:
            case 4:
            case 2:
            case 1:
                if (hasKey)
                    Unlock();
                break;
        }
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        if (locked)
            spriteRenderer.sprite = lockedSprites[args.CurResolutionRatio];
        else
            spriteRenderer.sprite = unlockedSprites[args.CurResolutionRatio];
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
        if (locked)
            fromObj.GetComponent<SpriteRenderer>().sprite = lockedSprites[prevResolutionRatio];
        else
            fromObj.GetComponent<SpriteRenderer>().sprite = unlockedSprites[prevResolutionRatio];
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

    public void SetHasKey()
    {
        hasKey = true;
    }

    private void Unlock()
    {
        SoundManager.Instance.SceneEffectPlayStr("9");
        locked = false;
        spriteRenderer.sprite = unlockedSprites[GameData.Instance.GetResolutionRatio()];
        player.EnableKeyFloaterSilver(false);
        mechanicalGate.UnlockByKey();
        circuit.CheckConnect();
    }
}
