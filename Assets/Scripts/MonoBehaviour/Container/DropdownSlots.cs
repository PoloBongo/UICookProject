using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropdownSlots : MonoBehaviour
{
    [SerializeField] private Slots slots;
    
    private TMP_Dropdown dropdown;
    private Dictionary<int, GameObject> slotAssociations = new Dictionary<int, GameObject>();
    
    public delegate void OnChangeIndexDropdownContainer(int index);
    public static event OnChangeIndexDropdownContainer OnChangeIndexDropdownContainerFunc;
    
    public delegate void OnHandClick(GameObject currentObj);
    public static event OnHandClick OnSelectedHand;
    
    private int selectedIndex;

    private void OnEnable()
    {
        RaycastContainer.SendGameObjectAssociateToUsedSlotFunc += GetGameObjectAssociateToUsedSlot;
        RaycastPickable.CanTakeIngredientFunc += CanTakeIngredient;
    }

    private void OnDisable()
    {
        RaycastContainer.SendGameObjectAssociateToUsedSlotFunc -= GetGameObjectAssociateToUsedSlot;
        RaycastPickable.CanTakeIngredientFunc -= CanTakeIngredient;
    }

    private void Start()
    {
        selectedIndex = 0;
        dropdown = GetComponent<TMP_Dropdown>();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    private void OnDropdownValueChanged(int value)
    {
        string selectedOption = dropdown.options[value].text;
        int newSelectedIndex = int.Parse(selectedOption);
        newSelectedIndex--;
        OnChangeIndexDropdownContainerFunc?.Invoke(newSelectedIndex);
        selectedIndex = newSelectedIndex;
    }

    private void GetGameObjectAssociateToUsedSlot(GameObject _gameObject, int slotIndex)
    {
        if (slotAssociations.ContainsKey(slotIndex))
        {
            Debug.LogWarning($"Le slot {slotIndex} est déjà occupé par {slotAssociations[slotIndex].name}");
            return;
        }
        slotAssociations[slotIndex] = _gameObject;
        Debug.Log($"GameObject {_gameObject.name} associé à la position {slotIndex}");
    }
    
    private GameObject GetGameObjectAtSlot(int slotIndex)
    {
        return slotAssociations.GetValueOrDefault(slotIndex);
    }

    public void GetObjectAtPosition()
    {
        GameObject selectedObject = GetGameObjectAtSlot(selectedIndex);
        if (selectedObject)
        {
            OnSelectedHand?.Invoke(selectedObject);
        }
    }

    private void CanTakeIngredient()
    {
        slotAssociations.Remove(selectedIndex);
    }

    private void OnDestroy()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}