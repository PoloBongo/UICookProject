using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cook : MonoBehaviour
{
    private List<GameObject> ingredientsInFour = new List<GameObject>();
    private List<GameObject> ingredientsInFourAvailableForRecette = new List<GameObject>();
    [Header("Slider Values")]
    [SerializeField] private Slider slider;
    [Header("List Recipes")]
    [SerializeField] private ListRecipesCreator listRecipesCreator;
    int totalIngredients;

    [Header("Particles")] 
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private List<ParticleSystem> particles;
    private Coroutine coroutineCooking;

    [SerializeField] private RaycastContainer raycastContainer;
    [SerializeField] private DropdownSlots dropdownSlots;
    [SerializeField] private GameObject parent;

    private int totalIndexToRemove;
    
    public delegate void OnShowNotification(string message);
    public static event OnShowNotification OnShowNotificationFunc;
    
    private void Start()
    {
        totalIngredients = 0;
        totalIngredients = 0;
        ingredientsInFour = new List<GameObject>();
        coroutineCooking = null;
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
    int validIngredientsCount = 0;
    List<GameObject> ingredientsInFourAndInRecette = ingredientsInFour
        .Select(obj => obj.gameObject)
        .ToList();

    for (int recipeIndex = 0; recipeIndex < listRecipesCreator.recipes.Count; recipeIndex++)
    {
        var recipe = listRecipesCreator.recipes[recipeIndex];
        int totalIngredients = recipe.ingredients.Count;

        if (ingredientsInFour.Count < totalIngredients)
        {
            Debug.Log("Pas assez d'ingrédients dans le four pour la recette " + recipe.name);
            continue;
        }

        List<IngredientCreator> recipeIngredients = ReturnIngredientsFromRecipe(recipeIndex); 
        List<IngredientCreator> availableIngredients = ingredientsInFour
            .Select(obj => obj.GetComponent<Item>().GetIngredientCreator())
            .ToList();

        validIngredientsCount = 0;

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
            OnShowNotificationFunc?.Invoke("Vous avez cuisiner un : " + recipe.name);
            GameObject newIngredient = Instantiate(recipe.recipePrefab, ingredientsInFour[0].transform.position, Quaternion.identity, parent.transform);

            foreach (var deleteIngredientCuit in ingredientsInFourAndInRecette)
            {
                totalIndexToRemove++;
                if (totalIndexToRemove < ingredientsInFourAndInRecette.Count)
                {
                    dropdownSlots.CanTakeIngredient(dropdownSlots.GetSlotIndex(deleteIngredientCuit));
                    raycastContainer.ReleaseSlotWithIndex(1);
                }
                Destroy(deleteIngredientCuit.gameObject);
            }

            dropdownSlots.GetGameObjectAssociateToUsedSlotFour(newIngredient, 0);
            break; // Exit after the first valid recipe is found, remove this if you want to keep checking all recipes
        }
        else
        {
            Debug.Log("Recette invalide ou ingrédients manquants pour " + recipe.name);
        }
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

    public void SliderActivate()
    {
        switch (slider.value)
        {
            case > 0:
                particlePrefab.SetActive(true);
                foreach (var particle in particles)
                {
                    particle.Play();
                }
                coroutineCooking ??= StartCoroutine(StartingCooking(slider.value));
                break;
            case 0:
                foreach (var particle in particles)
                {
                    particle.Stop();
                }
                particlePrefab.SetActive(false);
                StopCoroutine(StartingCooking(slider.value));
                coroutineCooking = null;
                break;
        }
    }

    IEnumerator StartingCooking(float _sliderValue)
    {
        slider.interactable = false;
        GetFourChild();
        ingredientsInFourAvailableForRecette = ingredientsInFour
            .Select(obj => obj.gameObject)
            .ToList();
        float newSliderValue = _sliderValue * 0.1f;
        float newCalcul = (1 / newSliderValue) + 2;
        yield return new WaitForSeconds(newCalcul);
        
        foreach (var ingredient in ingredientsInFourAvailableForRecette)
        {
            if (ingredient && ingredient.gameObject.GetComponent<MeshFilter>()) ingredient.gameObject.GetComponent<MeshFilter>().mesh = ingredient.gameObject.GetComponent<Item>().GetIngredientCreator().meshCuit;
        }
        CheckIsValidRecipe();
        Debug.Log("End of coroutine");
        coroutineCooking = null;
        slider.interactable = true;
        dropdownSlots.SelectedIndex = 0;
    }
}
