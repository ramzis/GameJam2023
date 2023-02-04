using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectiveController : MonoBehaviour
{
    public event Action OnRequestNewRecipe;

    public event Action OnObjectivesStarted;
    public event Action OnObjectiveProgressed;
    public event Action OnObjectivesExhausted;

    public event Action<int> OnLivesCountChanged;

    private RecipeData currentRecipe;
    private RecipeData poisonRecipe;
    private int lives = 3;
    private int recipeIndex = 1;

    private bool lastRecipeValid;
    private bool potionProvided;
    private bool poisonProvided;

    private void Start()
    {
        StartCoroutine(GameLoop());
        OnObjectivesStarted?.Invoke();
    }

    public void SetCurrentRecipe(RecipeData recipe)
    {
        currentRecipe = recipe;
    }

    public void SetPoisonRecipe(RecipeData recipe)
    {
        poisonRecipe = recipe;
    }

    public void EvaluateRecipe(HashSet<ItemData> ingredients)
    {
        if (recipeIndex == 5)
        {
            poisonProvided = poisonRecipe.ingredients.TrueForAll((x) =>
            ingredients.Contains(x));
        }

        lastRecipeValid = currentRecipe.ingredients.TrueForAll((x) =>
            ingredients.Contains(x));

        potionProvided = true;
    }

    private IEnumerator GameLoop()
    {
        while(true)
        {
            Debug.Log("[GameLoop]: Requesting recipe");
            OnRequestNewRecipe?.Invoke();

            lastRecipeValid = false;
            potionProvided = false;
            poisonProvided = false;

            yield return new WaitUntil(() => potionProvided);

            Debug.Log("[GameLoop]: Evaluating recipe");

            // All levels
            if (recipeIndex < 5)
            {
                if (lastRecipeValid)
                {
                    yield return SleepCutscene();
                }
                else if (--lives < 1)
                {
                    OnLivesCountChanged?.Invoke(lives);
                    StartCoroutine(EarlyGameOver());
                    yield break;
                }
            }
            // Last level
            else
            {
                if(poisonProvided)
                {
                    StartCoroutine(PoisonCutscene());
                    yield break;
                }

                lives = 0;
                OnLivesCountChanged?.Invoke(lives);

                // You fail even if the recipe is "valid"
                StartCoroutine(GameOver());
                yield break;
            }

            recipeIndex++;
        }
    }

    private IEnumerator PoisonCutscene()
    {
        Debug.Log("[GameLoop]: You poisoned the witch!");
        yield return null;
    }

    private IEnumerator SleepCutscene()
    {
        Debug.Log("[GameLoop]: The witch fell asleep...");
        yield return null;
    }

    private IEnumerator EarlyGameOver()
    {
        Debug.Log("[GameLoop]: The witch got angry and ate you!");
        yield return null;
    }

    private IEnumerator GameOver()
    {
        Debug.Log("[GameLoop]: You did well, but the witch still ate you!");
        yield return null;
    }
}
