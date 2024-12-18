using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private IngredientCreator ingredientCreator;

    public IngredientCreator GetIngredientCreator()
    {
        return ingredientCreator;
    }
}
