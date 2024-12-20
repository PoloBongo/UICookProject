using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    private InputActionManager inputActionManager;
    private PlayerInputAction playerInputAction;
    
    [Header("Copy")]
    [SerializeField] private List<ContentClosetIngredient> cpyIngredients;
    [SerializeField] private List<ContentClosetIngredientSprite> cpyRawImages;
    [SerializeField] private List<Transform> listTab;
    [SerializeField] private List<GameObject> closet;
    [SerializeField] private List<InOutElasticYFrigo> animOffset;
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
    
    public delegate void OnShowNotification(string message);
    public static event OnShowNotification OnShowNotificationFunc;
    
    // generated item & closet
    private int categoryNumber;
    private List<int> itemsPerCategory = new List<int>();
    
    private void OnEnable()
    {
        ResetData();
        categoryNumber = Random.Range(1, 5);
        for (int i = 0; i < categoryNumber; i++)
        {
            closet[i].SetActive(true);
        }
        
        itemsPerCategory.Clear();

        for (int i = 0; i < categoryNumber; i++)
        {
            int randomItemCount = Random.Range(2, 16);
            itemsPerCategory.Add(randomItemCount);
        }
        Debug.Log(categoryNumber);
        Debug.Log($"Categories : {categoryNumber}, Items par catégorie : {string.Join(", ", itemsPerCategory)}");

        canClick = true;
        InOutElasticY.OnFinishMotionElastic += HandleCheckCanClick;
        
        AssignRandomIngredients();
    }

    private void Start()
    {
        if (!inputActionManager) inputActionManager = InputActionManager.Instance;
        playerInputAction = inputActionManager.GetPlayerInputAction();
    }

    private void HandleCheckCanClick(bool _canClick)
    {
        canClick = _canClick;
    }
    
    private void AssignRandomIngredients()
    {
        List<List<IngredientCreator>> availableIngredientsByCategory = cpyIngredients
            .Select(c => new List<IngredientCreator>(c.ingredient))
            .ToList();

        List<List<RawImage>> availableRawImagesByCategory = cpyRawImages
            .Select(c => new List<RawImage>(c.rawImage))
            .ToList();

        for (int i = 0; i < categoryNumber; i++)
        {
            ContentClosetIngredient newIngredientCategory = new ContentClosetIngredient
            {
                ingredient = new List<IngredientCreator>(),
                maxSlot = itemsPerCategory[i],
                actualSlotUsed = 0
            };

            ContentClosetIngredientSprite newRawImageCategory = new ContentClosetIngredientSprite
            {
                rawImage = new List<RawImage>()
            };

            for (int j = 0; j < itemsPerCategory[i]; j++)
            {
                if (availableIngredientsByCategory[i].Count > 0 && availableRawImagesByCategory[i].Count > 0)
                {
                    int ingredientIndex = Random.Range(0, availableIngredientsByCategory[i].Count);
                    int rawImageIndex = ingredientIndex;

                    newIngredientCategory.ingredient.Add(availableIngredientsByCategory[i][ingredientIndex]);
                    newRawImageCategory.rawImage.Add(availableRawImagesByCategory[i][rawImageIndex]);

                    availableIngredientsByCategory[i].RemoveAt(ingredientIndex);
                    availableRawImagesByCategory[i].RemoveAt(rawImageIndex);

                    newIngredientCategory.actualSlotUsed++;
                }
                else
                {
                    break;
                }
            }

            ingredients.Add(newIngredientCategory);
            rawImages.Add(newRawImageCategory);
        }

        InitItemPanel();
    }

    private void ResetData()
    {
        foreach (Transform tab in listTab)
        {
            for( int i = 0; i < tab.childCount; ++i )
            {
                tab.GetChild(i).gameObject.SetActive(false);
            }
        }

        foreach (var closetSingle in closet)
        {
            closetSingle.SetActive(false);
        }
        ingredients.Clear();
        rawImages.Clear();
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
                else
                {
                    rawImages[i].rawImage[j].gameObject.SetActive(false);
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
                if (ingredients[i].actualSlotUsed < 19)
                {
                    for (int j = 0; j < ingredients[i].ingredient.Count; j++)
                    {
                        if (!ingredients[i].ingredient[j])
                        {
                            rawImages[i].rawImage[j].gameObject.SetActive(true);
                            rawImages[i].rawImage[j].texture = stockGameObjectForDrop.GetComponent<Item>().GetIngredientCreator().icon;
                            ingredients[i].ingredient[j] = stockGameObjectForDrop.GetComponent<Item>().GetIngredientCreator();
                            ingredients[i].actualSlotUsed++;
                            OnShowNotificationFunc?.Invoke("Vous venez de déposer : " + ingredients[i].ingredient[j].name);
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
            OnShowNotificationFunc?.Invoke("Vous avez pris " + currentObj.name);
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
        playerInputAction.Player.Enable();
        this.gameObject.SetActive(false);
    }
    
    public void HiddeCheckIcon(GameObject newSelectedCheck)
    {
        if (canClick)
        {
            ShowCheckIcon(newSelectedCheck);
        }
        else
        {
            OnShowNotificationFunc?.Invoke("Veuillez attendre afin de pouvoir choisir un nouvel ingrédient.");
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
        ResetData();
    }

    public void SetTextDescriptionItem()
    {
        TMP_Text text = panelInfoItem.GetComponentInChildren<TMP_Text>();
        if (!text) Debug.LogError("text de l'item description introuvable");
        text.text = stockTextDescrpTemp;
    }
}
