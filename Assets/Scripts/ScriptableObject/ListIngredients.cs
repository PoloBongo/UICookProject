using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListIngredients", menuName = "Scriptable Objects/ListIngredients")]
public class ListIngredients : ScriptableObject
{
    public List<IngredientCreator> ingredients;
}
