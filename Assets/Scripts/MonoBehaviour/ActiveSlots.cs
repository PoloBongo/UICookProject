using System;
using UnityEngine;

public class ActiveSlots : MonoBehaviour
{
    [SerializeField] private Slots _slots;
    private void OnEnable()
    {
        _slots.DeclenchEvent();
    }
}
