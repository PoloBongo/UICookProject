using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cook : MonoBehaviour
{
    private List<GameObject> ingredientsInFour;
    [Header("List Recipes")]
    [SerializeField] private ListRecipesCreator listRecipesCreator;
    int totalIngredients = 3;
    
    private void GetFourChild()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.layer != LayerMask.GetMask("UI"))
            {
                ingredientsInFour.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }

    public void CheckIsValidRecipe()
    {
        GetFourChild();
        int recipeCount = 0;
        int validIngredientsCount = 0;

        for (int i = 0; i < totalIngredients; i++)
        {
            Debug.Log(ReturnIngredientsFromRecipe(recipeCount)[i]);
            if (ReturnIngredientsFromRecipe(recipeCount)[i] == ingredientsInFour[i].name)
            {
                validIngredientsCount++;
            }

            if (validIngredientsCount == totalIngredients)
            {
                Debug.Log("tous les ingrédients requis pour la recette sont dans le four manuelo");
                return;
            }

            if (listRecipesCreator.recipes[recipeCount].ingredients.Count == totalIngredients)
            {
                recipeCount++;
            }
        }
    }

    private List<string> ReturnIngredientsFromRecipe(int _recipeID)
    {
        List<string> ingredients = new List<string>();
        for (int i = 0; i < listRecipesCreator.recipes[_recipeID].ingredients.Count; i++)
        {
            ingredients.Add(listRecipesCreator.recipes[_recipeID].ingredients[i].name);
        }
        return ingredients;
    }
}
