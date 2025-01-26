using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CatIngredient : IngredientObject
{
    public override void Interact()
    {
        base.Interact();
        if (Cauldron.cauldronIsMidAnimation) return;
        AudioManager.Play("Cat Angry");
        transform.DOScale(transform.localScale * 0.5f, 2);
    }
}
