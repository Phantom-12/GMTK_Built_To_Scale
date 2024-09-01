using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MechanicalGate : SerializedMonoBehaviour
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
    private bool lockedByCircuit = true;
    [SerializeField]
    public bool lockedByKey = true;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private GameObject transitionShower;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        transitionShower = transform.Find("TransitionShower").gameObject;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        if (CheckLocked())
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
        if (CheckLocked())
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

    public void UnlockByCircuit()
    {
        lockedByCircuit = false;
        CheckLocked();
    }

    public void LockByCircuit()
    {
        lockedByCircuit = true;
        CheckLocked();
    }

    public void UnlockByKey()
    {
        lockedByKey = false;
        CheckLocked();
    }

    private bool CheckLocked()
    {
        bool locked = lockedByCircuit || lockedByKey;
        spriteRenderer.sprite = locked ? lockedSprites[GameData.Instance.GetResolutionRatio()] : unlockedSprites[GameData.Instance.GetResolutionRatio()];
        boxCollider2D.enabled = locked;
        return locked;
    }
}
