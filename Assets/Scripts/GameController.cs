using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("1,2,4,8,16")]
    List<int> avaliableResolutionRatios;
    [SerializeField]
    int startResolutionRatio;

    private GameController() { }
    public static GameController Instance { get; private set; }

    private readonly Dictionary<string, Transform> name2Tilemap = new();

    private void Awake()
    {
        Instance = this;
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
    }

    private void Start()
    {
        var sceneMaps = GameObject.Find("SceneMaps").GetComponentsInChildren<Tilemap>();
        foreach (var t in sceneMaps)
        {
            name2Tilemap.Add(t.name, t.transform);
            t.gameObject.SetActive(false);
        }
        SetPause(false);
        GameData.Instance.InitResolutonRatio(avaliableResolutionRatios, startResolutionRatio);
        FindFirstObjectByType<UIDragClamp>().Init();
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        if (args.CurResolutionRatio == args.PrevResolutionRatio)
        {
            name2Tilemap[GetMapTilemapName(args.PrevResolutionRatio)].gameObject.SetActive(true);
            return;
        }
        StopAllCoroutines();
        foreach (var transform in name2Tilemap.Values)
            transform.gameObject.SetActive(false);
        GameObject fromObj, toObj;
        fromObj = name2Tilemap[GetMapTilemapName(args.PrevResolutionRatio)].gameObject;
        toObj = name2Tilemap[GetMapTilemapName(args.CurResolutionRatio)].gameObject;
        fromObj.SetActive(true);
        toObj.SetActive(true);
        fromObj.GetComponent<TilemapCollider2D>().enabled = false;
        toObj.GetComponent<TilemapCollider2D>().enabled = true;
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

    private string GetMapTilemapName(int id)
    {
        return "Tilemap_" + id + "x";
    }

    public void SetPause(bool enable)
    {
        GameData.Instance.SetIsPause(enable);
        if (enable)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
