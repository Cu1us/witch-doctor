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
    public Color defaultColor;
    Color startColor;
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

    public static bool cauldronIsMidAnimation = false;

    public ParticleSystem bubbles;
    public Material bubblesMaterial;

    public ParticleSystem[] P_Company;
    public ParticleSystem[] P_Ambition;
    public ParticleSystem[] P_Money;
    public ParticleSystem[] P_Peace;


    Tween potionTween;
    Tween surfaceTween;

    void Start()
    {
        targetColor = startColor = defaultColor;
        SetBubblesColor(defaultColor);
        currentWaveSpeed = startWaveSpeed = targetWaveSpeed = GetTargetWaveSpeed();
    }
    public void AddIngredient(Ingredient toAdd, Color color)
    {
        if (cauldronIsMidAnimation) return;
        bubbles.Emit(10);
        AudioManager.Play("Bottle Pour");
        ingredients.Add(toAdd);
        colors.Add(color);
        UpdateColor();
        AudioManager.Play("Splash");
        CheckIngredients();
    }

    void SetBubblesEmitting(bool active)
    {
        var emission = bubbles.emission;
        emission.enabled = active;
    }
    void SetBubblesColor(Color color)
    {
        bubblesMaterial.SetColor("_Color", color);
    }

    private void CheckIngredients()
    {
        if (ingredients.Count > GameManager.currentClient.neededIngredients.Length)
        {
            cauldronIsMidAnimation = true;
            SetBubblesEmitting(false);
            Fail();
        }
        else if (HasWrongIngredients())
        {
            cauldronIsMidAnimation = true;
            SetBubblesEmitting(false);
            Fail();
        }
        else if (HasAllIngredients())
        {
            cauldronIsMidAnimation = true;
            SetBubblesEmitting(false);
            Invoke(nameof(Succeed), animationDuration / 2f);
        }
    }

    public void Fail()
    {
        OnPotionFail?.Invoke();
        AudioManager.Play("Puff Sad");
        GameManager.Instance.FailPotion();
        Invoke(nameof(PlaySoundTromb), 0.5f);
        Invoke(nameof(PlaySoundWitch), 2.8f);
        transform.DOMoveY(surfaceEmptyY, surfaceSinkDur + 0.5f)
        .SetDelay(1.5f)
        .OnComplete(() =>
        {

            startWaveSpeed = targetWaveSpeed = currentWaveSpeed = GetTargetWaveSpeed();
            transform.DOMoveY(surfaceFilledY, surfaceSinkDur);

            Invoke(nameof(BackToGameplayAfterFail), surfaceSinkDur / 3.5f);
        });
    }

    public void PlaySoundWitch()
    {
        AudioManager.Play("Witch fail");
    }

    public void PlaySoundTromb()
    {
        AudioManager.Play("Trombone");
    }

    public void Succeed()
    {
        AudioManager.Play("Puff Glad");
        colors.Clear();
        winPotion.gameObject.SetActive(true);
        winPotion.SetColor(targetColor);
        AudioManager.Play("Harp Happy");
        winPotion.DOKill();
        transform.DOKill();
        OnPotionSucceed?.Invoke();
        GameManager.Instance.SucceedPotion();
        transform.DOMoveY(surfaceEmptyY, surfaceSinkDur);
        potionTween = winPotion.transform.DOMoveY(potionTopY, winPotionRiseDur).SetDelay(1);
        Invoke(nameof(PutBackWinPotion), winPotionSpinDuration);
        Invoke(nameof(DoSplash), 1f);
    }

    void DoSplash()
    {
        Splash(ingredients.ToArray());
        ingredients.Clear();
    }

    private void Splash(Ingredient[] content)
    {
        List<ParticleSystem> particleSystems = new();
        foreach (Ingredient ingredient in content)
        {
            ParticleSystem[] systems = ingredient switch
            {
                Ingredient.COMPANY => P_Company,
                Ingredient.AMBITION => P_Ambition,
                Ingredient.PEACE => P_Peace,
                _ => P_Money
            };
            foreach (ParticleSystem system in systems)
            {
                particleSystems.Add(system);
            }
        }
        int countPer = 50 / particleSystems.Count;
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Emit(countPer);
        }
    }

    void PutBackWinPotion()
    {
        transform.DOMoveY(surfaceFilledY, surfaceSinkDur);
        startColor = targetColor = currentColor = defaultColor;
        startWaveSpeed = targetWaveSpeed = currentWaveSpeed = GetTargetWaveSpeed();
        winPotion.transform.DOJump(new Vector3(winPotion.transform.position.x, potionTopY, potionGoneZ), potionGoneJumpHeight, 1, potionJumpAwayDur)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            winPotion.transform.position = new Vector3(winPotion.transform.position.x, potionBottomY, potionHereZ);
            winPotion.gameObject.SetActive(false);
        });
        Invoke(nameof(BackToGameplayAfterSuccess), surfaceSinkDur / 2f);
    }

    void BackToGameplayAfterSuccess()
    {
        GameManager.Instance.NextClient();
        SetBubblesEmitting(true);
        cauldronIsMidAnimation = false;
    }
    void BackToGameplayAfterFail()
    {
        ingredients.Clear();
        colors.Clear();
        startColor = targetColor = currentColor = defaultColor;
        SetBubblesEmitting(true);
        cauldronIsMidAnimation = false;
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
        SetBubblesColor(currentColor);
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