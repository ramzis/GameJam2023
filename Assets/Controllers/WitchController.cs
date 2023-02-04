using System;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    public event Action OnRequestEvaluateRecipe;
    public event Action OnWitchPraiseRecipe;

    private Witch witch;

    private void OnEnable()
    {
        witch = FindObjectOfType<Witch>();
        if (witch == null) throw new MissingComponentException("Witch required in scene!");

        witch.OnRequestEvaluateRecipe += OnRequestEvaluateRecipeHandler;
        witch.OnWitchPraiseRecipe += OnWitchPraiseRecipeHandler;
    }

    private void OnDisable()
    {
        witch.OnRequestEvaluateRecipe -= OnRequestEvaluateRecipeHandler;
        witch.OnWitchPraiseRecipe -= OnWitchPraiseRecipeHandler;
    }

    public void ProvideCurrentRecipe(RecipeData recipe)
    {
        witch.ProvideRecipe(recipe);
    }

    public void EvaluateRecipe(bool correct)
    {
        witch.EvaluateRecipe(correct);
    }

    public void BrewPotion(HashSet<ItemData> items)
    {

    }

    private void OnRequestEvaluateRecipeHandler()
    {
        OnRequestEvaluateRecipe?.Invoke();
    }

    private void OnWitchPraiseRecipeHandler()
    {
        OnWitchPraiseRecipe?.Invoke();
    }
}
