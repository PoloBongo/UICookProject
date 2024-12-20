using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEditor;

public class SetupFormOptions : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownRecipe;
    [SerializeField] private ListIngredients ingredients;
    [SerializeField] private ListRecipesCreator getRecipeFormInput;
    [SerializeField] private GameObject brutForcePrefab;

    [Header("UI Elements")]
    [SerializeField] private GameObject showIngredientsList;
    [SerializeField] private GameObject showPanelIngredientsList;
    [SerializeField] private GameObject closeIngredientsList;
    [SerializeField] private List<TMP_Text> ingredientsList;
    [SerializeField] private TMP_InputField ingredientNameInput;
    [SerializeField] private TMP_InputField ingredientDescrInput;
    
    private string getTitleFormInput;
    private string getDescrFormInput;
    private List<IngredientCreator> getIngredientsFormInput;
    
    public delegate void OnShowNotification(string message);
    public static event OnShowNotification OnShowNotificationFunc;
    
    private void OnEnable()
    {
        dropdownRecipe.ClearOptions();
        getIngredientsFormInput = new List<IngredientCreator>();
        List<string> ingredientNames = new List<string>();
        
        ingredientNames.Add("Nothing");
        foreach (var ingredient in ingredients.ingredients)
        {
            ingredientNames.Add(ingredient.name);
        }
        dropdownRecipe.AddOptions(ingredientNames);
    }

    private void OnDisable()
    {
        dropdownRecipe.ClearOptions();
    }

    public void SendForm()
    {
        foreach (var recipeName in getRecipeFormInput.recipes)
        {
            if (recipeName.recipeName == getTitleFormInput)
            {
                return;
            }
        }
        RecipeCreator newRecipe = ScriptableObject.CreateInstance<RecipeCreator>();
        newRecipe.name = getTitleFormInput;
        newRecipe.recipeName = getTitleFormInput;
        newRecipe.ingredients = getIngredientsFormInput;
        newRecipe.descriptionRecipe = getDescrFormInput;
        newRecipe.recipePrefab = brutForcePrefab;

        if (getTitleFormInput == "")
        {
            OnShowNotificationFunc?.Invoke("Vous devez entrer un nom de recette!");
            return;
        }
        
        if (getIngredientsFormInput.Count <= 0)
        {
            OnShowNotificationFunc?.Invoke("Vous devez entrer des ingredients!");
            return;
        }
        
        for (int i = 0; i < getRecipeFormInput.recipes.Count; i++)
        {
            if (newRecipe.name == getRecipeFormInput.recipes[i].name) return;
        }
        string path = $"{Application.persistentDataPath}/{getTitleFormInput}.json";
        string jsonData = JsonUtility.ToJson(newRecipe);
        File.WriteAllText(path, jsonData);
        OnShowNotificationFunc?.Invoke("Nouvelle recette ' " + newRecipe.recipeName + " ' crée avec succès !");
        getRecipeFormInput.recipes.Add(newRecipe);
        ResetInputField();
        CloseAllIngredients();
    }

    public void OnValueChangedTitle(string _title)
    {
        getTitleFormInput = _title;
    }
    
    public void OnValueChangedDescr(string _descr)
    {
        getDescrFormInput = _descr;
    }
    
    public void OnValueChangedDropdown(int _option)
    {
        int selectedOption = _option - 1;

        if (selectedOption >= 0 && selectedOption < ingredients.ingredients.Count)
        {
            IngredientCreator selectedIngredient = ingredients.ingredients[selectedOption];

            getIngredientsFormInput.Add(selectedIngredient);
            ShowAllIngredientsFromRecipe();
        }
    }

    public void ShowAllIngredients()
    {
        closeIngredientsList.SetActive(true);
        showIngredientsList.SetActive(false);
        showPanelIngredientsList.SetActive(true);
        ShowAllIngredientsFromRecipe();
    }
    
    public void CloseAllIngredients()
    {
        closeIngredientsList.SetActive(false);
        showIngredientsList.SetActive(true);
        showPanelIngredientsList.SetActive(false);
        ShowAllIngredientsFromRecipe(false);
    }
    
    private void ShowAllIngredientsFromRecipe(bool _show = true)
    {

        foreach (var ingredientText in ingredientsList)
        {
            ingredientText.text = "";
        }

        for (int i = 0; i < getIngredientsFormInput.Count; i++)
        {
            ingredientsList[i].text = _show ? getIngredientsFormInput[i].name : "";
        }
    }

    private void ResetInputField()
    {
        getTitleFormInput = "";
        getDescrFormInput = "";
        getIngredientsFormInput = new List<IngredientCreator>();
        ingredientNameInput.text = "";
        ingredientDescrInput.text = "";
    }
}
