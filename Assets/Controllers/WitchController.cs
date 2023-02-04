using System;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    public event Action OnRequestEvaluateRecipe;

    private Witch witch;

    private void OnEnable()
    {
        witch = FindObjectOfType<Witch>();
        if (witch == null) throw new MissingComponentException("Witch required in scene!");

        witch.OnRequestEvaluateRecipe += OnRequestEvaluateRecipeHandler;
    }

    private void OnDisable()
    {
        witch.OnRequestEvaluateRecipe -= OnRequestEvaluateRecipeHandler;
    }

    public void ProvideCurrentRecipe(RecipeData recipe)
    {
        witch.ProvideRecipe(recipe);
    }

    public void EvaluateRecipe(bool correct)
    {
        witch.EvaluateRecipe(correct);
    }

    private void OnRequestEvaluateRecipeHandler()
    {
        OnRequestEvaluateRecipe?.Invoke();
    }
}
