using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListRecipesCreator", menuName = "Scriptable Objects/ListRecipesCreator")]
public class ListRecipesCreator : ScriptableObject
{
    public List<RecipeCreator> recipes;
}
