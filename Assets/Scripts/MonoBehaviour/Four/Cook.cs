using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cook : MonoBehaviour
{
    private List<GameObject> ingredientsInFour = new List<GameObject>();
    [Header("List Recipes")]
    [SerializeField] private ListRecipesCreator listRecipesCreator;
    int totalIngredients;
    
    private void Start()
    {
        totalIngredients = 0;
        ingredientsInFour = new List<GameObject>();
    }
    
    private void GetFourChild()
    {
        ingredientsInFour.Clear();
        int uiLayer = LayerMask.NameToLayer("UI");
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if (child.layer != uiLayer)
            {
                ingredientsInFour.Add(child);
            }
        }
    }

    public void CheckIsValidRecipe()
    {
        GetFourChild();
        int recipeCount = 0;
        int validIngredientsCount = 0;
        totalIngredients = listRecipesCreator.recipes[recipeCount].ingredients.Count;

        if (ingredientsInFour.Count < totalIngredients)
        {
            Debug.Log("Pas assez d'ingrédients dans le four !");
            return;
        }

        List<IngredientCreator> recipeIngredients = ReturnIngredientsFromRecipe(recipeCount);
        List<IngredientCreator> availableIngredients = ingredientsInFour
            .Select(obj => obj.GetComponent<Item>().GetIngredientCreator())
            .ToList();

        foreach (var recipeIngredient in recipeIngredients)
        {
            if (availableIngredients.Contains(recipeIngredient))
            {
                validIngredientsCount++;
                availableIngredients.Remove(recipeIngredient);
            }
        }

        if (validIngredientsCount == totalIngredients)
        {
            Debug.Log("Tous les ingrédients requis pour la recette sont dans le four !");
        }
        else
        {
            Debug.Log("Recette invalide ou ingrédients manquants.");
        }
    }


    private List<IngredientCreator> ReturnIngredientsFromRecipe(int _recipeID)
    {
        List<IngredientCreator> ingredients = new List<IngredientCreator>();
        for (int i = 0; i < listRecipesCreator.recipes[_recipeID].ingredients.Count; i++)
        {
            ingredients.Add(listRecipesCreator.recipes[_recipeID].ingredients[i]);
        }
        return ingredients;
    }
}
