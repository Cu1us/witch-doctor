using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<Ingredient> ingredients;

    public void AddIngredient(Ingredient toAdd)
    {
        ingredients.Add(toAdd);
        CheckIngredients();
    }

    private void CheckIngredients()
    {

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
}

public enum Ingredient
{
    COMPANY,
    RICHES,
    AMBITION,
    PEACE
}