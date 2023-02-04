using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe Data", menuName = "Data/Recipe", order = 1)]
public class RecipeData : ScriptableObject
{
    [Header("Metadata")]
    public string title = "Recipe";

    public List<ItemData> ingredients;
}