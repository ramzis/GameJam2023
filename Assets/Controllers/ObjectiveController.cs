using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectiveController : MonoBehaviour
{
    public event Action OnObjectivesStarted;
    public event Action OnObjectiveProgressed;
    public event Action OnObjectivesExhausted;

    private RecipeData currentRecipe;

    private void Start()
    {
        OnObjectivesStarted?.Invoke();
    }

    public void SetCurrentRecipe(RecipeData recipe)
    {
        currentRecipe = recipe;
    }

    public bool EvaluateRecipe(HashSet<ItemData> ingredients)
    {
        return currentRecipe.ingredients.TrueForAll((x) =>
            ingredients.Contains(x));
    }
}
