using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
class ContentClosetIngredient
{
    public List<IngredientCreator> ingredient;
}


[Serializable]
class ContentClosetIngredientSprite
{
    public List<RawImage> rawImage;
}

public class ContentCloset : MonoBehaviour
{
    [Header("ContentCloset")]
    [SerializeField] private List<ContentClosetIngredient> ingredients;
    [SerializeField] private List<ContentClosetIngredientSprite> rawImages;

    private GameObject selectedCheck;
    private GameObject currentObj;
    
    public delegate void OnHandClick(string buttonName, GameObject currentObj = null);
    public static event OnHandClick OnSelectedHand;
    
    
    private void Start()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = 0; j < ingredients[i].ingredient.Count; j++)
            {
                rawImages[i].rawImage[j].gameObject.SetActive(true);
                rawImages[i].rawImage[j].texture = ingredients[i].ingredient[j].icon;
            }
        }
    }

    public void ChooseHand(string handName)
    {
        if (currentObj != null)
        {
            OnSelectedHand?.Invoke(handName, currentObj);
        }
    }

    public void SetCurrentObj(RawImage image)
    {
        currentObj = null;
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = 0; j < ingredients[i].ingredient.Count; j++)
            {
                if (ingredients[i].ingredient[j].prefab.name == image.texture.name)
                {
                    currentObj = ingredients[i].ingredient[j].prefab;
                }
            }
        }
    }

    public void HiddeCloset()
    {
        this.gameObject.SetActive(false);
    }
    
    public void HiddeCheckIcon(GameObject newSelectedCheck)
    {
        newSelectedCheck.SetActive(true);
        if (selectedCheck) selectedCheck.SetActive(false);
        selectedCheck = newSelectedCheck;
    }
}
