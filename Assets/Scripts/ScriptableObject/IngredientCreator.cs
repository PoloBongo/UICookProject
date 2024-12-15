using UnityEngine;

[CreateAssetMenu(fileName = "IngredientCreator", menuName = "Scriptable Objects/IngredientCreator")]
public class IngredientCreator : ScriptableObject
{
    public Sprite icon;
    public string name;
    public string description;
}
