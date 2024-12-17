using System;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject placard;
    [SerializeField] private GameObject panelInfoItem;

    private GameObject selectedCheck;
    private GameObject currentObj;
    private bool canClick = true;
    private string stockTextDescrpTemp;
    
    public delegate void OnHandClick(string buttonName, GameObject currentObj = null);
    public static event OnHandClick OnSelectedHand;
    
    private void OnEnable()
    {
        canClick = true;
        InOutElasticY.OnFinishMotionElastic += HandleCheckCanClick;
    }

    private void HandleCheckCanClick(bool _canClick)
    {
        canClick = _canClick;
    }
    
    
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
                    stockTextDescrpTemp = ingredients[i].ingredient[j].description;
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
        if (canClick)
        {
            if (selectedCheck) selectedCheck.SetActive(false);
            newSelectedCheck.SetActive(panelInfoItem.activeSelf);
            selectedCheck = newSelectedCheck;
            canClick = false;
        }
    }

    private void OnDisable()
    {
        InOutElasticY.OnFinishMotionElastic -= HandleCheckCanClick;
        if (selectedCheck) selectedCheck.SetActive(false);
        placard.layer = LayerMask.NameToLayer("Closet");
    }

    public void SetTextDescriptionItem()
    {
        TMP_Text text = panelInfoItem.GetComponentInChildren<TMP_Text>();
        if (!text) Debug.LogError("text de l'item description introuvable");
        text.text = stockTextDescrpTemp;
    }
}
