using System;
using UnityEngine;

public class DropdownInitIndex : MonoBehaviour
{
    [SerializeField] private DropdownSlots dropdownSlots;
    private void OnEnable()
    {
        if (dropdownSlots) dropdownSlots.SelectedIndex = 0;
    }
}
