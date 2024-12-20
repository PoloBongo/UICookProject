using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndexRecipe : MonoBehaviour
{
    [Header("Settings Pages")]
    [SerializeField] private List<TMP_Text> titleRecipe;
    [SerializeField] private List<TMP_Text> ingredientsRecipe;
    [SerializeField] private List<TMP_Text> descriptionRecipe;

    [Header("UI Elements")]
    [SerializeField] private ListRecipesCreator listRecipesCreator;
    [SerializeField] private RaycastRecipe raycastRecipe;
    [SerializeField] private GameObject uiAddRecipe;
    
    private InputActionManager inputActionManager;
    private PlayerInputAction playerInputAction;
    
    private int index = 0;

    private void Start()
    {
        if (!inputActionManager) inputActionManager = InputActionManager.Instance;
        playerInputAction = inputActionManager.GetPlayerInputAction();
        ShowRecipes();
    }

    public void SwitchPageLeft()
    {
        if (index > 0)
        {
            index--;
            ShowRecipes();
        }
    }

    public void SwitchPageRight()
    {
        if (index < listRecipesCreator.recipes.Count - 1)
        {
            index++;
            ShowRecipes();
        }
    }

    private void ShowRecipes()
    {
        if (index >= 0 && index < listRecipesCreator.recipes.Count)
        {
            if (index == 0)
            {
                var currentRecipe = listRecipesCreator.recipes[index];
                titleRecipe[1].text = currentRecipe.name;
                ingredientsRecipe[1].text = GetIngredientsText(currentRecipe.ingredients);
                descriptionRecipe[1].text = currentRecipe.descriptionRecipe;

                titleRecipe[0].text = "";
                ingredientsRecipe[0].text = "";
                descriptionRecipe[0].text = "";
            }
            else
            {
                var currentRecipe = listRecipesCreator.recipes[index - 1];
                titleRecipe[1].text = currentRecipe.name;
                ingredientsRecipe[1].text = GetIngredientsText(currentRecipe.ingredients);
                descriptionRecipe[1].text = currentRecipe.descriptionRecipe;

                var previousRecipe = listRecipesCreator.recipes[index];
                titleRecipe[0].text = previousRecipe.name;
                ingredientsRecipe[0].text = GetIngredientsText(previousRecipe.ingredients);
                descriptionRecipe[0].text = previousRecipe.descriptionRecipe;
                
                
                StartAnimationLitMotion();
            }
        }
    }

    private string GetIngredientsText(List<IngredientCreator> ingredients)
    {
        if (ingredients == null || ingredients.Count == 0)
            return "Aucun ingrédient";

        return string.Join(", ", ingredients.ConvertAll(ingredient => ingredient.name));
    }


    public void CloseRecipeBook()
    {
        if (!raycastRecipe.GetAntiAutoOpen())
        {
            raycastRecipe.OpenCloseRecipeBook();
            raycastRecipe.SetAntiAutoOpen(true);
        }
    }

    private void StartAnimationLitMotion()
    {
        for (int i = 0; i < titleRecipe.Count; i++)
        {
            titleRecipe[i].GetComponentInChildren<InOutBounceControlable>().StartAnimationBounce();
            ingredientsRecipe[i].GetComponentInChildren<InOutBounceControlable>().StartAnimationBounce();
            descriptionRecipe[i].GetComponentInChildren<InOutBounceControlable>().StartAnimationBounce();
        }
    }

    public void ShowFormAddRecipe()
    {
        uiAddRecipe.SetActive(true);
        playerInputAction.Player.Movement.Disable();
    }
    
    public void HiddeFormAddRecipe()
    {
        uiAddRecipe.SetActive(false);
        playerInputAction.Player.Movement.Enable();
    }
}
