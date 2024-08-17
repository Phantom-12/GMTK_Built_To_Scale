using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField]
    private PlayerAfterImageSprite prefab;
    [SerializeField]
    private int defaultCapacity;
    [SerializeField]
    private int maxSize;

    public static ObjectPool<PlayerAfterImageSprite> Pool{get;private set;}

    private void Awake()
    {
        Pool=new ObjectPool<PlayerAfterImageSprite>(OnCreatePoolItem,OnGetPoolItem,OnReleasePoolItem,OnDestroyPoolItem,true,defaultCapacity,maxSize);
    }

    private PlayerAfterImageSprite OnCreatePoolItem()
    {
        return Instantiate(prefab,parent:this.transform);
    }

    private void OnGetPoolItem(PlayerAfterImageSprite item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReleasePoolItem(PlayerAfterImageSprite item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyPoolItem(PlayerAfterImageSprite item)
    {
        Destroy(item.gameObject);
    }
}
