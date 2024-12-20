using UnityEngine;

public class CategoryIndex : MonoBehaviour
{
    [SerializeField] private int categoryIndex;

    public int GetCategoryIndex
    {
        get => categoryIndex;
        set => categoryIndex = value;
    }
}
