using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionShower : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    RectMask2D rectMask2D;
    [SerializeField]
    RectTransform effect;
    [SerializeField]
    Image lastSceneImage;

    [SerializeField]
    float AnimInterval = 0.02f;
    [SerializeField]
    float AnimDeltaX = 20f;

    private float height, width;

    private void Awake()
    {
        LevelManager.Instance.SceneChangeEvent += OnSceneChange;
        LevelManager.Instance.SceneChangeEvent1 += OnSceneChange1;
    }

    private void Start()
    {
        height = canvas.GetComponent<RectTransform>().rect.height;
        width = canvas.GetComponent<RectTransform>().rect.width;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        LevelManager.Instance.SceneChangeEvent -= OnSceneChange;
        LevelManager.Instance.SceneChangeEvent1 -= OnSceneChange1;
    }

    private void OnSceneChange(object sender, SceneChangeEventArgs args)
    {
        Play(args.Sprite);
    }

    private void OnSceneChange1(object sender, SceneChangeEventArgs1 args)
    {
        Play1(args.Camera, args.OldScene);
    }

    public void Play(Sprite sprite)
    {
        gameObject.SetActive(true);
        lastSceneImage.sprite = sprite;
        StartCoroutine(Anim());
    }

    private IEnumerator Anim()
    {
        float startX = width, endX = -width;
        var padding = rectMask2D.padding;
        float nowX = startX;
        while (nowX >= endX)
        {
            nowX -= AnimDeltaX;
            effect.anchoredPosition = new Vector2(nowX, 0);
            padding.z = width - (nowX + width / 2);
            rectMask2D.padding = padding;
            yield return new WaitForSeconds(AnimInterval);
        }
        gameObject.SetActive(false);
    }

    public void Play1(Camera camera, Scene oldScene)
    {
        gameObject.SetActive(true);
        StartCoroutine(Anim1(camera, oldScene));
    }

    private IEnumerator Anim1(Camera camera, Scene oldScene)
    {
        float startX = width, endX = -width;
        var padding = rectMask2D.padding;
        float nowX = startX;
        while (nowX >= endX)
        {
            nowX -= AnimDeltaX;
            camera.rect = new Rect(0, 0, (nowX + width) / 2 / width, 1);
            camera.transform.position = new Vector3((1 - (nowX + width) / 2) * 10, camera.transform.position.y, 0);
            effect.anchoredPosition = new Vector2(nowX, 0);
            padding.z = width - (nowX + width / 2);
            rectMask2D.padding = padding;
            yield return new WaitForSeconds(AnimInterval);
        }
        gameObject.SetActive(false);
        SceneManager.UnloadSceneAsync(oldScene);
    }
}
