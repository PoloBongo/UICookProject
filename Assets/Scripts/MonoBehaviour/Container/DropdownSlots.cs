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
    
    public delegate void OnShowNotification(string message);
    public static event OnShowNotification OnShowNotificationFunc;
    
    private int selectedIndex;

    public int SelectedIndex
    {
        get => selectedIndex;
        set => selectedIndex = value;
    }

    private void OnEnable()
    {
        RaycastContainer.SendGameObjectAssociateToUsedSlotFunc += GetGameObjectAssociateToUsedSlot;
    }

    private void OnDisable()
    {
        RaycastContainer.SendGameObjectAssociateToUsedSlotFunc -= GetGameObjectAssociateToUsedSlot;
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
        selectedIndex = 0;
        string selectedOption = dropdown.options[value].text;
        int newSelectedIndex = int.Parse(selectedOption);
        newSelectedIndex--;
        OnChangeIndexDropdownContainerFunc?.Invoke(newSelectedIndex);
        selectedIndex = newSelectedIndex;
    }

    private void GetGameObjectAssociateToUsedSlot(GameObject _gameObject, int slotIndex)
    {
        // if (slotAssociations.ContainsKey(slotIndex))
        // {
        //     return;
        // }
        slotAssociations[slotIndex] = _gameObject;
    }
    
    public void GetGameObjectAssociateToUsedSlotFour(GameObject _gameObject, int slotIndex)
    {
        slotAssociations[slotIndex] = _gameObject;
    }
    
    private GameObject GetGameObjectAtSlot(int slotIndex)
    {
        return slotAssociations.GetValueOrDefault(slotIndex);
    }

    public void GetObjectAtPosition()
    {
        GameObject selectedObject = GetGameObjectAtSlot(selectedIndex);
        OnShowNotificationFunc?.Invoke("Vous venez de prendre : " + selectedObject.name);
        if (selectedObject)
        {
            OnSelectedHand?.Invoke(selectedObject);
        }
    }

    public void CanTakeIngredient(int _deletedIndex = -1)
    {
        if (_deletedIndex != -1) selectedIndex = _deletedIndex;
        slotAssociations.Remove(selectedIndex);
    }
    
    public int GetSlotIndex(GameObject obj)
    {
        foreach (var entry in slotAssociations)
        {
            if (entry.Value == obj)
            {
                return entry.Key;
            }
        }
        return -1;
    }

    private void OnDestroy()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}