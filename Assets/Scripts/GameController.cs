using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("1,2,4,8,16")]
    List<int> avaliableResolutionRatios = new List<int>();
    [SerializeField]
    int startResolutionRatio;

    private GameController() { }
    public static GameController Instance { get; private set; }

    private readonly Dictionary<string, Transform> name2Tilemap = new();
    private readonly Dictionary<Transform, List<Transform>> tilemap2Children = new();

    private void Awake()
    {
        PlayerPrefs.SetInt("16x", 1);
        PlayerPrefs.SetInt("8x", 0);
        PlayerPrefs.SetInt("4x", 0);
        PlayerPrefs.SetInt("2x", 0);
        PlayerPrefs.SetInt("LC1", 0);
        Instance = this;
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        for (int i = 0; i < avaliableResolutionRatios.Count;)
        {
            if (PlayerPrefs.GetInt(avaliableResolutionRatios[i].ToString() + "x", 0) == 1)
            {
                i++;
            }
            else
            {
                avaliableResolutionRatios.Remove(avaliableResolutionRatios[i]);
            }
        }
    }

    private void Start()
    {
        var sceneMaps = GameObject.Find("SceneMaps");
        for (var i = 0; i < sceneMaps.transform.childCount; i++)
        {
            var t = sceneMaps.transform.GetChild(i);
            name2Tilemap.Add(t.name, t.transform);
            tilemap2Children.Add(t, new());
            var children = t.GetComponentsInChildren<Tilemap>();
            foreach (var c in children)
                tilemap2Children[t].Add(c.transform);
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
            foreach (var t in tilemap2Children[fromObj.transform])
            {
                var tilemap = t.GetComponent<Tilemap>();
                var color = tilemap.color;
                color.a = colorFrom.a;
                tilemap.color = color;
            }
            tilemapTo.color = colorTo;
            foreach (var t in tilemap2Children[toObj.transform])
            {
                var tilemap = t.GetComponent<Tilemap>();
                var color = tilemap.color;
                color.a = colorTo.a;
                tilemap.color = color;
            }
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
    public void addResolution(int ratio)
    {
        avaliableResolutionRatios.Add(ratio);
        GameData.Instance.InitResolutonRatio(avaliableResolutionRatios, ratio);
        FindFirstObjectByType<UIDragClamp>().Init();
    }
}
