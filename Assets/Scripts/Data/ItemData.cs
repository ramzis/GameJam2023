using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Data/Item", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Metadata")]
    public string title = "Item";
    public Type type = Type.Ingredient;
    public Category category = Category.Root;

    [Header("Visuals")]
    public Sprite available;
    public Sprite extracted;
    public Sprite empty;

    public enum Type
    {
        Ingredient,
        Trash
    }

    public enum Category
    {
        Root,
        Sap,
        Mushroom,
        Any,
    }
}