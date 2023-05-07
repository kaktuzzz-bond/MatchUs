using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GamePointer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private PointerController _pointerController;

    private const float FadeDuration = 0.05f;


    private void Awake()
    {
        _pointerController = PointerController.Instance;
    }


    public void Show()
    {
        gameObject.SetActive(true);

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


    public GamePointer SetPosition(Vector3 position)
    {
        transform.position = position;

        return this;
    }


    public GamePointer SetParent(Transform parent)
    {
        transform.SetParent(parent);

        return this;
    }


    public GamePointer SetName()
    {
        transform.name = transform.tag;

        return this;
    }


    public GamePointer Subscribe()
    {
        _pointerController.OnPointersHidden += Hide;

        return this;
    }
}