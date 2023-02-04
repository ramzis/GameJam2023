using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectiveController : MonoBehaviour
{
    public event Action OnObjectivesStarted;
    public event Action OnObjectivesExhausted;

    [SerializeField]
    private List<RecipeData> recipes;

    private RecipeData currentRecipe;

    private void Start()
    {
        currentRecipe = recipes[0];
        OnObjectivesStarted?.Invoke();
    }

    public bool EvaluateRecipe(HashSet<ItemData> ingredients)
    {
        return currentRecipe.ingredients.TrueForAll((x) =>
            ingredients.Contains(x));
    }
}
