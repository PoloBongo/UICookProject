using System;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour
{
    [SerializeField] private List<Vector3> allPositions;
    [SerializeField] private List<Vector3> slotPositions;
    public List<Vector3> SlotPositions { get => slotPositions; set => slotPositions = value; }
    
    private List<Vector3> cpySlotPositions;
    public List<Vector3> CPYSlotPositions { get => cpySlotPositions; set => cpySlotPositions = value; }

    public delegate void SetupCopySlotPositions(List<Vector3> slotPositions);
    public static event SetupCopySlotPositions OnSetupCopySlotPositionsFunc;
    
    private void Start()
    {
        cpySlotPositions = allPositions;
    }

    public void DeclenchEvent()
    {
        OnSetupCopySlotPositionsFunc?.Invoke(allPositions);
    }
}
