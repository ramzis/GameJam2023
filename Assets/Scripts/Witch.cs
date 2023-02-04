using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour, IInteractable
{
    public event Action OnRequestEvaluateRecipe;
    public event Action OnWitchPraiseRecipe;

    private RecipeData currentRecipe;

    public enum State
    {
        HandingOutRecipe,
        WaitingForIngredients,
        ReactingToIngredients,
        Brewing
    }

    private State state;

    public void Interact()
    {
        switch (state)
        {
            case State.HandingOutRecipe:
                StartCoroutine(SayRecipe());
                state = State.WaitingForIngredients;
                break;
            case State.WaitingForIngredients:
                OnRequestEvaluateRecipe?.Invoke();
                state = State.WaitingForIngredients;
                break;
        }
    }

    public void ProvideRecipe(RecipeData recipe)
    {
        currentRecipe = recipe;
    }

    public void EvaluateRecipe(bool correct)
    {
        state = State.ReactingToIngredients;

        if(correct)
        {
            StartCoroutine(PraiseRecipe());
        }
        else
        {
            StartCoroutine(ScoldRecipe());
        }
    }

    private IEnumerator SayRecipe()
    {
        // TODO: proper text for each recipe
        var text = "WITCH: Bring me: ";
        foreach(var ingredient in currentRecipe.ingredients)
            text += ingredient.title + " ";

        Debug.Log(text);

        yield return null;
    }

    private IEnumerator PraiseRecipe()
    {
        // TODO: proper text for each recipe
        var text = "WITCH: You did well...this time!";

        Debug.Log(text);

        OnWitchPraiseRecipe?.Invoke();

        yield return null;
    }

    private IEnumerator ScoldRecipe()
    {
        // TODO: proper text for each recipe
        var text = "WITCH: You fool! This is not what I asked for!";

        Debug.Log(text);

        yield return null;
    }
}
