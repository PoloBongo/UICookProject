using UnityEngine;
using UnityEngine.UI;

public class ItemMetadata : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private int categoryIndex;
    
    public RawImage Image => image;
    public int CategoryIndex { get => categoryIndex; set => categoryIndex = value; }
}
