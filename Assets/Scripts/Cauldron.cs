using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Cauldron : MonoBehaviour
{
    [SerializeField] new public Renderer renderer;
    public List<Ingredient> ingredients;
    public List<Color> colors;
    public float animationDuration;
    public AnimationCurve animationCurve;
    [SerializeField] Color startColor;
    Color targetColor;
    float colorAnimStartTime;
    Color currentColor;
    float currentWaveSpeed;
    float startWaveSpeed;
    float targetWaveSpeed;
    public float waveSpeedAnimationDuration;

    public float surfaceFilledY;
    public float surfaceEmptyY;
    public float surfaceSinkDur;
    public float potionTopY;
    public float potionBottomY;
    public float potionHereZ;
    public float potionGoneZ;
    public float potionGoneJumpHeight;
    public float potionJumpAwayDur;

    public float winPotionRiseDur;
    public float winPotionSpinDuration;

    public UnityEvent OnPotionSucceed;
    public UnityEvent OnPotionFail;

    public WinPotion winPotion;

    bool midAnimation = false;

    Tween potionTween;
    Tween surfaceTween;

    void Start()
    {
        targetColor = startColor;
        currentWaveSpeed = startWaveSpeed = targetWaveSpeed = GetTargetWaveSpeed();
    }
    public void AddIngredient(Ingredient toAdd, Color color)
    {
        if (midAnimation) return;
        ingredients.Add(toAdd);
        colors.Add(color);
        UpdateColor();
        CheckIngredients();
    }



    private void CheckIngredients()
    {
        if (ingredients.Count > GameManager.currentClient.neededIngredients.Length)
        {
            midAnimation = true;
            Fail();
        }
        else if (HasWrongIngredients())
        {
            midAnimation = true;
            Fail();
        }
        else if (HasAllIngredients())
        {
            midAnimation = true;
            Invoke(nameof(Succeed), animationDuration);
        }
    }

    public void Fail()
    {
        OnPotionFail?.Invoke();
        GameManager.Instance.FailPotion();
    }

    public void Succeed()
    {
        winPotion.gameObject.SetActive(true);
        winPotion.DOKill();
        transform.DOKill();
        OnPotionSucceed?.Invoke();
        GameManager.Instance.SucceedPotion();
        surfaceTween = transform.DOMoveY(surfaceEmptyY, surfaceSinkDur);
        potionTween = winPotion.transform.DOMoveY(potionTopY, winPotionRiseDur).SetDelay(1);
        Invoke(nameof(PutBackWinPotion), winPotionSpinDuration);
    }

    void PutBackWinPotion()
    {
        surfaceTween.Rewind(false);
        winPotion.transform.DOJump(new Vector3(winPotion.transform.position.x, potionTopY, potionGoneZ), potionGoneJumpHeight, 1, potionJumpAwayDur)
        .OnComplete(() =>
        {
            winPotion.transform.position = new Vector3(winPotion.transform.position.x, potionBottomY, potionHereZ);
            winPotion.gameObject.SetActive(false);
        });
        Invoke(nameof(BackToGameplay), surfaceSinkDur / 2f);
    }

    void BackToGameplay()
    {
        midAnimation = false;
    }

    float GetTargetWaveSpeed()
    {
        return 200f * ingredients.Count + 360f;
    }

    void UpdateColor()
    {
        colorAnimStartTime = Time.time;
        startColor = currentColor;
        float r = 0, g = 0, b = 0;
        for (int i = 0; i < colors.Count; i++)
        {
            r += colors[i].r;
            g += colors[i].g;
            b += colors[i].b;
        }
        r /= colors.Count;
        g /= colors.Count;
        b /= colors.Count;
        targetColor = new Color(r, g, b);
        startWaveSpeed = currentWaveSpeed;
        targetWaveSpeed = GetTargetWaveSpeed();
    }

    void Update()
    {
        float progress = animationCurve.Evaluate(Mathf.Clamp01((Time.time - colorAnimStartTime) / animationDuration));
        float waveProgress = animationCurve.Evaluate(Mathf.Clamp01((Time.time - colorAnimStartTime) / waveSpeedAnimationDuration));

        currentColor = Color.Lerp(startColor, targetColor, progress);
        currentWaveSpeed = Mathf.Lerp(startWaveSpeed, targetWaveSpeed, waveProgress);
        //Debug.Log($"Current: {-currentWaveSpeed}, progress: {waveProgress}");

        renderer.material.SetColor("_Color", currentColor);
        renderer.material.SetFloat("_WaveSpeed", -currentWaveSpeed);
    }

    public bool HasAllIngredients()
    {
        foreach (Ingredient requiredIngredient in GameManager.currentClient.neededIngredients)
        {
            if (!ingredients.Contains(requiredIngredient))
            {
                return false;
            }
        }
        return ingredients.Count == GameManager.currentClient.neededIngredients.Length;
    }
    public bool HasWrongIngredients()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (!GameManager.currentClient.neededIngredients.Contains(ingredient))
            {
                return true;
            }
        }
        return false;
    }

    [ContextMenu("Reassign private component references")]
    protected virtual void Reset()
    {
        renderer = GetComponent<Renderer>();
    }
}

public enum Ingredient
{
    COMPANY,
    RICHES,
    AMBITION,
    PEACE
}