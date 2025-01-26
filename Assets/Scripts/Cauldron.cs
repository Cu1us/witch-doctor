using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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

    void Start()
    {
        targetColor = startColor;
        currentWaveSpeed = startWaveSpeed = targetWaveSpeed = GetTargetWaveSpeed();
    }
    public void AddIngredient(Ingredient toAdd, Color color)
    {
        ingredients.Add(toAdd);
        colors.Add(color);
        UpdateColor();
        CheckIngredients();
    }

    private void CheckIngredients()
    {

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