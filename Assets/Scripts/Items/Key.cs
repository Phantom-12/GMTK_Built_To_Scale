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
    private SpriteRenderer spriteRenderer;
    private Action OnTriggerEnter2DHandler;
    [SerializeField]
    private LockedDoor lockedDoor;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter2DHandler();
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        spriteRenderer.sprite = sprites[args.CurResolutionRatio];
        switch (args.CurResolutionRatio)
        {
            case 16:
                OnTriggerEnter2DHandler = PickedUp;
                break;
            case 8:
            case 4:
            case 2:
            case 1:
                OnTriggerEnter2DHandler = DoNothing;
                break;
        }
    }

    private void DoNothing()
    {
    }

    private void PickedUp()
    {
    }
}
