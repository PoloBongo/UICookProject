using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeCreator", menuName = "Scriptable Objects/RecipeCreator")]
public class RecipeCreator : ScriptableObject
{
    public string recipeName;
    public List<IngredientCreator> ingredients;
    public string descriptionRecipe;
}
