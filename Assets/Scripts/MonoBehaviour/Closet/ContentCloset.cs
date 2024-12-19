using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

[Serializable]
class ContentClosetIngredient
{
    public List<IngredientCreator> ingredient;
    public int maxSlot;
    public int actualSlotUsed;
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
    
    [Header("Camera hands")]
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;

    private GameObject selectedCheck;
    private GameObject currentObj;
    private int currentCategoryIndex;
    private int currentItemIndex;
    private bool canClick = true;
    private bool switchActivePanelInfo = false;
    private string stockTextDescrpTemp;

    private GameObject stockGameObjectForDrop;
    
    public delegate void OnHandClick(string buttonName, GameObject currentObj = null);
    public static event OnHandClick OnSelectedHand;
    
    public delegate void OnShowIconCheckEvent(GameObject newSelectedCheck, bool isShow, bool isShowIcon);
    public static event OnShowIconCheckEvent OnShowIconCheck;
    
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
        InitItemPanel();
    }

    private void InitItemPanel()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = 0; j < ingredients[i].ingredient.Count; j++)
            {
                if (ingredients[i].ingredient[j])
                {
                    ingredients[i].maxSlot = ingredients[i].ingredient.Count;
                    ingredients[i].actualSlotUsed = j+1;
                    rawImages[i].rawImage[j].gameObject.SetActive(true);
                    rawImages[i].rawImage[j].texture = ingredients[i].ingredient[j].icon;
                }
            }
        }
    }
    
    private bool CheckChildInHand(int _chooseHand)
    {
        if (_chooseHand == -1) return false;

        Camera handCamera = _chooseHand == 0 ? leftHandCamera : _chooseHand == 1 ? rightHandCamera : null;
    
        if (handCamera != null && handCamera.transform.childCount > 0)
        {
            stockGameObjectForDrop = handCamera.transform.GetChild(0).gameObject;
            return true;
        }
        return false;
    }

    public void DropItemInCloset(int _chooseHand)
    {
        if (CheckChildInHand(_chooseHand))
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                if (ingredients[i].actualSlotUsed != ingredients[i].maxSlot)
                {
                    for (int j = 0; j < ingredients[i].ingredient.Count; j++)
                    {
                        if (!ingredients[i].ingredient[j])
                        {
                            rawImages[i].rawImage[j].gameObject.SetActive(true);
                            rawImages[i].rawImage[j].texture = stockGameObjectForDrop.GetComponent<Item>().GetIngredientCreator().icon;
                            ingredients[i].ingredient[j] = stockGameObjectForDrop.GetComponent<Item>().GetIngredientCreator();
                            ingredients[i].actualSlotUsed++;
                            Destroy(stockGameObjectForDrop);
                        }
                    }
                }
            }
        }
    }

    public void ChooseHand(string handName)
    {
        if (currentObj != null && selectedCheck.activeSelf)
        {
            ShowCheckIcon(selectedCheck);
            OnSelectedHand?.Invoke(handName, currentObj);
            RemoveItemFromCloset();
        }
    }

    public void SetCurrentObj(ItemMetadata itemMetadata)
    {
        currentObj = null;
        int category = itemMetadata.CategoryIndex;
        RawImage rawImage = itemMetadata.Image;

        for (int i = 0; i < ingredients[category].ingredient.Count; i++)
        {
            if (ingredients[category].ingredient[i] &&
                ingredients[category].ingredient[i].prefab.name == rawImage.texture.name)
            {
                currentObj = ingredients[category].ingredient[i].prefab;
                stockTextDescrpTemp = ingredients[category].ingredient[i].description;
                currentCategoryIndex = category;
                currentItemIndex = i;
                return;
            }
        }
    }

    private void RemoveItemFromCloset()
    {
        ingredients[currentCategoryIndex].ingredient[currentItemIndex] = null;
        rawImages[currentCategoryIndex].rawImage[currentItemIndex].gameObject.SetActive(false);
        ingredients[currentCategoryIndex].actualSlotUsed--;
        InitItemPanel();
    }

    public void HiddeCloset()
    {
        this.gameObject.SetActive(false);
    }
    
    public void HiddeCheckIcon(GameObject newSelectedCheck)
    {
        if (canClick)
        {
            ShowCheckIcon(newSelectedCheck);
        }
    }

    private void ShowCheckIcon(GameObject _newSelectedCheck)
    {
        switchActivePanelInfo = !switchActivePanelInfo;
        if (selectedCheck) selectedCheck.SetActive(false);
        panelInfoItem.SetActive(switchActivePanelInfo);
        selectedCheck = _newSelectedCheck;
        OnShowIconCheck?.Invoke(_newSelectedCheck, canClick, panelInfoItem.activeSelf);
        canClick = false;
    }

    private void OnDisable()
    {
        InOutElasticY.OnFinishMotionElastic -= HandleCheckCanClick;
        if (selectedCheck) selectedCheck.SetActive(false);
        placard.layer = LayerMask.NameToLayer("Closet");
        canClick = true;
        switchActivePanelInfo = false;
    }

    public void SetTextDescriptionItem()
    {
        TMP_Text text = panelInfoItem.GetComponentInChildren<TMP_Text>();
        if (!text) Debug.LogError("text de l'item description introuvable");
        text.text = stockTextDescrpTemp;
    }
}
