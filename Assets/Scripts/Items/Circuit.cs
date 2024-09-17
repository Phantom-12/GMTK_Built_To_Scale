using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;

public class Circuit : SerializedMonoBehaviour
{
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Tilemap> disconnectedTilemaps = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Tilemap> connectedTilemaps = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    private MechanicalGate mechanicalGate;

    private int currentResolutionRatio;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        foreach (var i in disconnectedTilemaps.Values)
            if (i)
                i.gameObject.SetActive(false);
        foreach (var i in connectedTilemaps.Values)
            if (i)
                i.gameObject.SetActive(false);
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
        if (mechanicalGate.lockedByKey)
        {
            if (!disconnectedTilemaps[args.PrevResolutionRatio] || !disconnectedTilemaps[args.CurResolutionRatio])
                return;
        }
        else
        {
            if (!connectedTilemaps[args.PrevResolutionRatio] || !connectedTilemaps[args.CurResolutionRatio])
                return;
        }
        if (mechanicalGate.lockedByKey)
        {
            disconnectedTilemaps[args.PrevResolutionRatio].gameObject.SetActive(false);
            disconnectedTilemaps[args.CurResolutionRatio].gameObject.SetActive(true);
        }
        else
        {
            connectedTilemaps[args.PrevResolutionRatio].gameObject.SetActive(false);
            connectedTilemaps[args.CurResolutionRatio].gameObject.SetActive(true);
        }
        currentResolutionRatio = args.CurResolutionRatio;
        CheckConnect();

        GameObject fromObj, toObj;
        if (mechanicalGate.lockedByKey)
        {
            fromObj = disconnectedTilemaps[args.PrevResolutionRatio].gameObject;
            toObj = disconnectedTilemaps[args.CurResolutionRatio].gameObject;
        }
        else
        {
            fromObj = connectedTilemaps[args.PrevResolutionRatio].gameObject;
            toObj = connectedTilemaps[args.CurResolutionRatio].gameObject;
        }
        PlayTransitionAnim(args.CurResolutionRatio, args.PrevResolutionRatio, fromObj, toObj);
    }

    private void PlayTransitionAnim(int curResolutionRatio, int prevResolutionRatio, GameObject fromObj, GameObject toObj)
    {
        if (curResolutionRatio == prevResolutionRatio)
            return;
        StopAllCoroutines();
        foreach (var i in disconnectedTilemaps.Values)
            if (i)
                i.gameObject.SetActive(false);
        foreach (var i in connectedTilemaps.Values)
            if (i)
                i.gameObject.SetActive(false);
        fromObj.SetActive(true);
        toObj.SetActive(true);
        StartCoroutine(PlayTransitionAnim(fromObj, toObj));
    }

    IEnumerator PlayTransitionAnim(GameObject fromObj, GameObject toObj)
    {
        float deltaAlpha = GlobalParam.transformAnimDeltaAlpha;
        float deltaTime = GlobalParam.transformAnimDeltaTime;
        Tilemap tilemapFrom, tilemapTo;
        Color colorFrom, colorTo;
        tilemapFrom = fromObj.GetComponent<Tilemap>();
        tilemapTo = toObj.GetComponent<Tilemap>();
        colorFrom = tilemapFrom.color;
        colorTo = tilemapTo.color;
        colorFrom.a = 1;
        colorTo.a = 0;

        toObj.SetActive(true);
        while (colorFrom.a > 0)
        {
            colorFrom.a -= deltaAlpha;
            colorTo.a += deltaAlpha;
            tilemapFrom.color = colorFrom;
            tilemapTo.color = colorTo;
            yield return new WaitForSeconds(deltaTime);
        }
        fromObj.SetActive(false);
    }

    public void CheckConnect()
    {
        if (currentResolutionRatio <= 4 && !mechanicalGate.lockedByKey)
            Connect();
        else if (currentResolutionRatio > 4 && !mechanicalGate.lockedByKey)
            Disconnect();
    }

    private void Connect()
    {
        SoundManager.Instance.SceneEffectPlayStr("12");
        if (disconnectedTilemaps[GameData.Instance.GetResolutionRatio()])
            disconnectedTilemaps[GameData.Instance.GetResolutionRatio()].gameObject.SetActive(false);
        if (connectedTilemaps[GameData.Instance.GetResolutionRatio()])
            connectedTilemaps[GameData.Instance.GetResolutionRatio()].gameObject.SetActive(true);
        mechanicalGate.UnlockByCircuit();
    }

    private void Disconnect()
    {
        if (connectedTilemaps[GameData.Instance.GetResolutionRatio()])
            connectedTilemaps[GameData.Instance.GetResolutionRatio()].gameObject.SetActive(true);
        if (disconnectedTilemaps[GameData.Instance.GetResolutionRatio()])
            disconnectedTilemaps[GameData.Instance.GetResolutionRatio()].gameObject.SetActive(false);
        mechanicalGate.LockByCircuit();
    }
}
