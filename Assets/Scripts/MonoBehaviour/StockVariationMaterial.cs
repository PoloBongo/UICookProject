using UnityEngine;

public class StockVariationMaterial : MonoBehaviour
{
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material defaultMaterial;

    public Material GetHoverMaterial()
    {
        return hoverMaterial;
    }

    public Material GetDefaultMaterial()
    {
        return defaultMaterial;
    }
}
