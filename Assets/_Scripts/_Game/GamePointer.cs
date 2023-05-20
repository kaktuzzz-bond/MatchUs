using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GamePointer : LinkedPoolObject
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private PointerController _pointerController;

    private const float FadeDuration = 0.05f;


    private void Awake()
    {
        _pointerController = ChipController.Instance.PointerController;
    }


    public override void Show()
    {
        _pointerController.OnPointersHidden += Hide;

        base.Show();

        spriteRenderer
                .DOFade(1f, FadeDuration);
    }


    private void Hide()
    {
        _pointerController.OnPointersHidden -= Hide;

        spriteRenderer
                .DOFade(0f, FadeDuration)
                .onComplete += () => _pointerController.ReleasePointer(this);
    }
}