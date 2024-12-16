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
    
    public delegate void OnHandClick(string buttonName, GameObject currentObj = null);
    public static event OnHandClick OnSelectedHand;
    
    private GameObject currentObj;
    
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
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = 0; j < ingredients[i].ingredient.Count; j++)
            {
                Debug.Log(ingredients[i].ingredient[j].prefab.name + "  prefab name + " + image.texture.name + " texture name");
                if (ingredients[i].ingredient[j].prefab.name == image.texture.name)
                {
                    currentObj = ingredients[i].ingredient[j].prefab;
                }
            }
        }
    }
}
