using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;

public class BigSpike : SerializedMonoBehaviour
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

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Player player;
    private GameObject transitionShower;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        player = FindFirstObjectByType<Player>();
        transitionShower = transform.Find("TransitionShower").gameObject;
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        switch (GameData.Instance.GetResolutionRatio())
        {
            case 16:
                player.OnHit();
                break;
            case 8:
                player.OnHit();
                break;
            case 4:
                player.OnHit();
                break;
            case 2:
            case 1:
                break;
        }
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        spriteRenderer.sprite = sprites[args.CurResolutionRatio];
        if (args.CurResolutionRatio == 1)
            boxCollider2D.enabled = false;
        else
            boxCollider2D.enabled = true;
        PlayTransitionAnim(args.CurResolutionRatio, args.PrevResolutionRatio);
    }

    private void PlayTransitionAnim(int curResolutionRatio, int prevResolutionRatio)
    {
        spriteRenderer.sprite = sprites[curResolutionRatio];
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
}
