using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class LockedDoor : SerializedMonoBehaviour
{
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Sprite> lockedSprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    [DictionaryDrawerSettings(KeyLabel = "分辨率（不应更改）", ValueLabel = "Sprite")]
    Dictionary<int, Sprite> unlockedSprites = new(){
        {16,null},
        {8,null},
        {4,null},
        {2,null},
        {1,null},
    };
    [SerializeField]
    private readonly bool isNeedkey = true;
    private bool locked = true;

    private SpriteRenderer spriteRenderer;
    private Action OnTriggerEnter2DHandler;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!isNeedkey)
            Unlock();
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
        if(locked)
            spriteRenderer.sprite = lockedSprites[args.CurResolutionRatio];
        else
            spriteRenderer.sprite = unlockedSprites[args.CurResolutionRatio];
        switch (args.CurResolutionRatio)
        {
            case 16:
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

    private void Unlock()
    {
        locked = false;
    }
}
