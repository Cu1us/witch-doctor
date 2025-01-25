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
    Color startColor;
    Color targetColor;
    float colorAnimStartTime;
    Color currentColor;

    public void AddIngredient(Ingredient toAdd, Color color)
    {
        ingredients.Add(toAdd);
        UpdateColor();
        CheckIngredients();
    }

    private void CheckIngredients()
    {

    }

    void UpdateColor()
    {
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
    }

    void Update()
    {
        float progress = Mathf.Clamp01((Time.time - colorAnimStartTime) / animationDuration);
        currentColor = Color.Lerp(startColor, targetColor, progress);

        renderer.material.SetColor("_Color", currentColor);
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