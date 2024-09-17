using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : SerializedMonoBehaviour
{
    // tilemap层枚举
    public enum TilemapLayer
    {
        platform,
        lava,
        decoration,
    }

    // tile信息
    [Serializable]
    public struct TileInfos
    {
        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public TileBase _16x;

        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public TileBase _8x;

        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public TileBase _4x;

        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public TileBase _2x;

        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public TileBase _1x;

        [VerticalGroup("attributes")]
        public TilemapLayer tilemapLayers;
    }

    // tilemap层信息
    [Serializable]
    public struct TilemapLayerInfo
    {
        public MonoScript monoScript;
        public bool needCollider;
        public bool isTriger;
        public LayerMask layerMask;
    }

    // tilemap层到层信息的映射
    [SerializeField]
    Dictionary<TilemapLayer, TilemapLayerInfo> layersToInfos;

    // 16xtile到信息的映射
    Dictionary<TileBase, TileInfos> tileToInfo;


    // 配置
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
    public List<TileInfos> tileInfos = new();

    [DisableInPlayMode]
    [Button("生成")]
    public void Generate()
    {
        string[] resolutionNames = { "16x", "8x", "4x", "2x", "1x" };
        var parent = transform.parent;
        MakeInfoMap();
        foreach (var resolutionName in resolutionNames)
        {
            Dictionary<TilemapLayer, Tilemap> layersToTilemaps = new();
            GameObject tilemapsGo = new($"Tilemaps_{resolutionName}");
            tilemapsGo.transform.SetParent(parent);
            foreach (var i in layersToInfos)
            {
                // tilemap层枚举
                TilemapLayer tilemapLayer = i.Key;
                // tilemap层信息
                TilemapLayerInfo tilemapLayerInfo = i.Value;
                // tilemap层Gameobject
                GameObject tilemapLayerGo = new($"Tilemaps_{tilemapLayer}_{resolutionName}");
                tilemapLayerGo.transform.SetParent(tilemapsGo.transform);
                layersToTilemaps.Add(tilemapLayer, tilemapLayerGo.AddComponent<Tilemap>());
                tilemapLayerGo.AddComponent<TilemapRenderer>();
                tilemapLayerGo.layer = LayerMaskToLayer(tilemapLayerInfo.layerMask);
                if (tilemapLayerInfo.needCollider)
                {
                    var tilemapCollider2D = tilemapLayerGo.AddComponent<TilemapCollider2D>();
                    var rigidbody2D = tilemapLayerGo.AddComponent<Rigidbody2D>();
                    var compositeCollider2D = tilemapLayerGo.AddComponent<CompositeCollider2D>();
                    tilemapCollider2D.usedByComposite = true;
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                    compositeCollider2D.isTrigger = tilemapLayerInfo.isTriger;
                    compositeCollider2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
                }
                if (tilemapLayerInfo.monoScript)
                {
                    var type = tilemapLayerInfo.monoScript.GetClass();
                    tilemapLayerGo.AddComponent(type);
                }
            }

            var tilemapTemplate = GetComponent<Tilemap>();
            tilemapTemplate.CompressBounds();
            var bound = tilemapTemplate.cellBounds;
            Dictionary<TilemapLayer, TileBase[]> layersToTileArr = new();
            foreach (var tilemapLayer in layersToInfos.Keys)
            {
                layersToTileArr.Add(tilemapLayer, new TileBase[bound.size.x * bound.size.y * bound.size.z]);
            }
            var tilesArr = tilemapTemplate.GetTilesBlock(bound);
            for (int index = 0; index < tilesArr.Length; index++)
            {
                if (!tilesArr[index])
                    continue;
                var ok = tileToInfo.TryGetValue(tilesArr[index], out TileInfos tileInfo);
                if (!ok)
                {
                    Debug.LogError("地图中使用了非16x瓦片，或此瓦片未注册到映射表");
                    return;
                }
                FieldInfo fieldInfo = tileInfo.GetType().GetField($"_{resolutionName}");
                layersToTileArr[tileInfo.tilemapLayers][index] = fieldInfo.GetValue(tileInfo) as TileBase;
            }
            foreach (var kv in layersToTilemaps)
            {
                var layer = kv.Key;
                var tilemap = kv.Value;
                tilemap.SetTilesBlock(bound, layersToTileArr[layer]);
            }
        }
        Debug.Log("FIN");
    }

    [DisableInPlayMode]
    [Button("TEST")]
    public void TEST()
    {
        MakeInfoMap();
        var tilemapTemplate = GetComponent<Tilemap>();
        tilemapTemplate.CompressBounds();
        var bound = tilemapTemplate.cellBounds;
        Debug.Log(bound);
        var arr = tilemapTemplate.GetTilesBlock(bound);
        for (int index = 0; index < arr.Length; index++)
        {
            if (!arr[index])
                continue;
            TileInfos tileInfo;
            var ok = tileToInfo.TryGetValue(arr[index], out tileInfo);
            if (!ok)
            {
                Debug.LogError("地图中使用了非16x瓦片，或此瓦片未注册到映射表");
                return;
            }
            Debug.Log(arr[index]);
            Debug.Log(tileInfo._16x);
        }
        Debug.Log("FIN");
    }

    private void MakeInfoMap()
    {
        tileToInfo = new();
        foreach (var info in tileInfos)
        {
            tileToInfo.Add(info._16x, info);
        }
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        if (layerMask.value == 0)
            return 0;
        int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
        if (Mathf.RoundToInt(Mathf.Pow(2, layer)) != layerMask.value)
        {
            Debug.LogError("请仅设置一个层");
            return 0;
        }
        return layer;
    }
}