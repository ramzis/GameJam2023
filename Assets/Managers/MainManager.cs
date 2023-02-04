using UnityEngine;

[RequireComponent(typeof(RecipeController))]
[RequireComponent(typeof(InventoryController))]
[RequireComponent(typeof(ObjectiveController))]
[RequireComponent(typeof(WitchController))]
public class MainManager : MonoBehaviour
{
    // 1. Receive recipe
    // 2. Collect to inventory
    // 3. Give to witch

    private RecipeController recipeController;
    private InventoryController inventoryController;
    private ObjectiveController objectiveController;
    private WitchController witchController;

    [SerializeField]
    private InventoryView inventoryView;

    private void Awake()
    {
        ResolveControllers();
        UnsubscribeEvents();
        SubscribeEvents();
    }

    private void ResolveControllers()
    {
        recipeController = GetComponent<RecipeController>();
        inventoryController = GetComponent<InventoryController>();
        objectiveController = GetComponent<ObjectiveController>();
        witchController = GetComponent<WitchController>();
    }

    private void SubscribeEvents()
    {
        objectiveController.OnRequestPoisonRecipe += OnRequestPoisonRecipeHandler;
        objectiveController.OnRequestNewRecipe += OnRequestNewRecipeHandler;

        witchController.OnRequestEvaluateRecipe += OnRequestEvaluateRecipeHandler;

        inventoryController.OnItemCollected += OnItemCollectedHandler;

        inventoryView.OnItemRemovedAtSlot += OnItemRemovedAtSlotHandler;
    }

    private void UnsubscribeEvents()
    {
        objectiveController.OnRequestPoisonRecipe -= OnRequestPoisonRecipeHandler;
        objectiveController.OnRequestNewRecipe -= OnRequestNewRecipeHandler;

        witchController.OnRequestEvaluateRecipe -= OnRequestEvaluateRecipeHandler;

        inventoryController.OnItemCollected -= OnItemCollectedHandler;

        inventoryView.OnItemRemovedAtSlot -= OnItemRemovedAtSlotHandler;
    }

    #region Objective Handler event handlers

    private void OnRequestPoisonRecipeHandler()
    {
        objectiveController.SetPoisonRecipe(recipeController.PoisonRecipe());
    }

    private void OnRequestNewRecipeHandler()
    {
        var recipe = recipeController.NextRecipe();
        objectiveController.SetCurrentRecipe(recipe);
        witchController.ProvideCurrentRecipe(recipe);
    }

    #endregion

    #region Inventory Controller event handlers

    private void OnItemCollectedHandler(ItemData item, int insertedAt)
    {
        inventoryView.SetSlotThumbnail(insertedAt, item.thumbnail);
    }

    #endregion

    #region Inventory View event handlers

    private void OnItemRemovedAtSlotHandler(int slot)
    {
        inventoryController.RemoveItemAtSlot(slot);
    }

    #endregion


    #region Witch Controller event handlers

    private void OnRequestEvaluateRecipeHandler()
    {
        var ingredients = inventoryController.GetIngredients();
        var correct = objectiveController.EvaluateRecipe(ingredients);
        witchController.EvaluateRecipe(correct);
    }

    #endregion
}
