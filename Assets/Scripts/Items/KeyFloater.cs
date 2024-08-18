using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KeyFloater : SerializedMonoBehaviour
{
    [SerializeField]
    float floatHeight;
    [SerializeField]
    float yScaler;
    [SerializeField]
    float xScaler;
    [SerializeField]
    float maxDistance;

    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Sprite> sprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };

    Transform player;
    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    private GameObject transitionShower;

    void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        player = FindFirstObjectByType<Player>().transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transitionShower = transform.Find("TransitionShower").gameObject;
    }

    void Start()
    {
    }

    void Update()
    {
        float fx = Mathf.Sign(player.position.x - transform.position.x) *
                Mathf.Max(0, Mathf.Abs(player.position.x - transform.position.x) - maxDistance) * xScaler;
        float fy = (floatHeight + player.position.y - transform.position.y) * yScaler;
        rigidbody2D.AddForce(new Vector2(fx, fy));
    }

    void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        spriteRenderer.sprite = sprites[args.CurResolutionRatio];
        PlayTransitionAnim(args.CurResolutionRatio, args.PrevResolutionRatio);
    }

    private void PlayTransitionAnim(int curResolutionRatio, int prevResolutionRatio)
    {
        if (!gameObject.activeSelf)
            return;
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
