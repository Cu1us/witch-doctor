using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CatIngredient : IngredientObject
{
    public override void Interact()
    {
        base.Interact();
        AudioManager.Play("Cat Angry");
        if (Cauldron.cauldronIsMidAnimation) return;
        transform.DOScale(transform.localScale * 0.5f, 2);
    }
}
