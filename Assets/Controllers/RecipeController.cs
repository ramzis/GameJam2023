using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour
{
    private int currentRecipeIndex;
    [SerializeField]
    private List<RecipeData> recipes;
    [SerializeField]
    private RecipeData poisonRecipe;

    private void Start()
    {
        if(recipes == null) recipes = new List<RecipeData>();
        currentRecipeIndex = 0;
    }

    public RecipeData NextRecipe()
    {
        if (currentRecipeIndex >= recipes.Count) return null;

        return recipes[currentRecipeIndex++];
    }

    public RecipeData CurrentRecipe()
    {
        if (currentRecipeIndex >= recipes.Count) return null;

        return recipes[currentRecipeIndex];
    }

    public RecipeData PoisonRecipe()
    {
        return poisonRecipe;
    }
}
