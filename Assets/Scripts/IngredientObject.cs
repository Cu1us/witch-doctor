using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : BaseInteractable
{
    public Ingredient ingredientValue;
    public Color ingredientColor;

    public override void Interact()
    {
        if (Cauldron.cauldronIsMidAnimation) return;
        base.Interact();
        GameManager.PutThingInCauldron(transform, ingredientValue, ingredientColor);
    }
}
