using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Category Data", menuName = "Data/Category", order = 1)]
public class CategoryData : ScriptableObject
{
    [Header("Metadata")]
    public ItemData.Category category;

    [Header("Visuals")]
    public Sprite slotPlaceholder;
}