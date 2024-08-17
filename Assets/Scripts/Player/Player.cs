using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMoveController playerMoveController;
    Animator animator;

    private void Awake()
    {
        GameData.Instance.PauseStateChangedEvent += OnPauseStateChanged;
    }

    private void Start()
    {
        playerMoveController = GetComponent<PlayerMoveController>();
        animator = GetComponent<Animator>();
        playerMoveController.SetEnable(true);
    }

    private void OnPauseStateChanged(object sender, PauseStateChangedEventArgs args)
    {
        playerMoveController.SetEnable(!args.IsPause);
    }

    public void OnHit()
    {
        playerMoveController.SetEnable(false);
        playerMoveController.SetVelocity(0, Vector2.zero);
        animator.Play("Hit");
    }

    public void OnDeathAnimFinish()
    {
        LevelManager.Instance.RestartScene();
    }

    private void OnDestroy()
    {
        GameData.Instance.PauseStateChangedEvent -= OnPauseStateChanged;
    }
}
