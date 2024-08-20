using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LavaTilemap : SerializedMonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Player player;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        boxCollider2D = GetComponent<BoxCollider2D>();
        player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        switch (GameData.Instance.GetResolutionRatio())
        {
            case 16:
            case 8:
            case 4:
            case 2:
            case 1:
                player.OnHit();
                break;
        }
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
    }
}
