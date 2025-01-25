using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Witch Client")]
public class Client : ScriptableObject
{
    public Texture2D clipboardTexture;
    public Ingredient[] neededIngredients;
}
