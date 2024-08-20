using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool ishit;
    GameObject playerTransitionShower;
    PlayerMoveController playerMoveController;
    Animator animator;
    [SerializeField]
    KeyFloater keyFloaterGold, keyFloaterSilver;

    private void Awake()
    {
        GameData.Instance.ResolutionRatioChangedEvent += OnResolutionRatioChanged;
        GameData.Instance.PauseStateChangedEvent += OnPauseStateChanged;
    }

    private void Start()
    {
        ishit = false;
        playerMoveController = GetComponent<PlayerMoveController>();
        animator = GetComponent<Animator>();
        playerMoveController.SetEnable(true);
        playerTransitionShower = transform.Find("PlayerTransitionShower").gameObject;
        playerTransitionShower.GetComponent<Animator>().SetFloat("alpha", 0);
        keyFloaterGold.gameObject.SetActive(false);
        keyFloaterSilver.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameData.Instance.ResolutionRatioChangedEvent -= OnResolutionRatioChanged;
        GameData.Instance.PauseStateChangedEvent -= OnPauseStateChanged;
    }

    private void OnPauseStateChanged(object sender, PauseStateChangedEventArgs args)
    {
        playerMoveController.SetEnable(!args.IsPause);
    }

    public void OnHit()
    {
        if (!ishit)
        {
            ishit = true;
            SoundManager.Instance.SceneEffectPlayStr("11");
            playerMoveController.SetEnable(false);
            playerMoveController.SetVelocity(0, Vector2.zero);
            animator.Play("Hit");
        }
    }

    public void OnDeathAnimFinish()
    {
        LevelManager.Instance.RestartScene();
    }

    private void OnResolutionRatioChanged(object sender, ResolutionRatioChangedEventArgs args)
    {
        animator.SetFloat("resolutionRatio", args.CurResolutionRatio);
        animator.SetFloat("alpha", 1);
        if (args.CurResolutionRatio == args.PrevResolutionRatio)
            return;
        playerTransitionShower.GetComponent<Animator>().SetFloat("resolutionRatio", args.PrevResolutionRatio);
        GameObject fromObj, toObj;
        fromObj = playerTransitionShower;
        toObj = gameObject;
        StartCoroutine(PlayTransitionAnim(fromObj, toObj));
    }

    IEnumerator PlayTransitionAnim(GameObject fromObj, GameObject toObj)
    {
        float deltaAlpha = GlobalParam.transformAnimDeltaAlpha;
        float deltaTime = GlobalParam.transformAnimDeltaTime;
        Animator animatorFrom, animatorTo;
        float alphaFrom, alphaTo;
        animatorFrom = fromObj.GetComponent<Animator>();
        animatorTo = toObj.GetComponent<Animator>();
        alphaFrom = 1;
        alphaTo = 0;

        while (alphaFrom > 0)
        {
            alphaFrom -= deltaAlpha;
            alphaTo += deltaAlpha;
            alphaTo = Mathf.Min(alphaTo, 1f);
            int parameterCount = animatorFrom.parameterCount;
            for (int i = 0; i < parameterCount; i++)
            {
                AnimatorControllerParameter parameter = animator.GetParameter(i);
                if (parameter.name == "resolutionRatio")
                    continue;
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    animatorFrom.SetBool(parameter.name, animatorTo.GetBool(parameter.name));
                else if (parameter.type == AnimatorControllerParameterType.Int)
                    animatorFrom.SetInteger(parameter.name, animatorTo.GetInteger(parameter.name));
                else if (parameter.type == AnimatorControllerParameterType.Float)
                    animatorFrom.SetFloat(parameter.name, animatorTo.GetFloat(parameter.name));
            }
            animatorFrom.SetFloat("alpha", alphaFrom);
            animatorTo.SetFloat("alpha", alphaTo);
            yield return new WaitForSeconds(deltaTime);
        }
    }

    public void EnableKeyFloaterGold(bool enable)
    {
        keyFloaterGold.gameObject.SetActive(enable);
    }

    public void EnableKeyFloaterSilver(bool enable)
    {
        keyFloaterSilver.gameObject.SetActive(enable);
    }
}
