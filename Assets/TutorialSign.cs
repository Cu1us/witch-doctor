using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialSign : MonoBehaviour
{
    public float upY;
    public float downY;
    public float tweenDuration;
    public bool moving = false;
    bool isVisible;

    public void ToggleVisible()
    {
        if (moving) return;
        if (isVisible)
        {
            moving = true;
            isVisible = false;
            transform.DOMoveY(upY, tweenDuration).SetEase(Ease.OutQuart).OnComplete(() => moving = false);
        }
        else
        {
            moving = true;
            isVisible = true;
            transform.DOMoveY(downY, tweenDuration).SetEase(Ease.OutBounce).OnComplete(() => moving = false);
        }
    }
    void Update()
    {
        if (isVisible && !moving && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleVisible();
        }
    }
}
