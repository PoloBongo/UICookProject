using UnityEngine;

[CreateAssetMenu(fileName = "IngredientCreator", menuName = "Scriptable Objects/IngredientCreator")]
public class IngredientCreator : ScriptableObject
{
    public GameObject prefab;
    public Mesh meshCuit;
    public Texture2D icon;
    public string name;
    public string description;
}
