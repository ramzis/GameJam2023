using UnityEngine;

[RequireComponent(typeof(RecipeController))]
[RequireComponent(typeof(InventoryController))]
[RequireComponent(typeof(ObjectiveController))]
public class MainManager : MonoBehaviour
{
    // 1. Receive recipe
    // 2. Collect to inventory
    // 3. Give to witch

    private RecipeController recipeController;
    private InventoryController inventoryController;
    private ObjectiveController objectiveController;

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
    }

    private void SubscribeEvents()
    {
        objectiveController.OnObjectivesStarted += OnObjectivesStartedHandler;
        objectiveController.OnRequestNewRecipe += OnRequestNewRecipeHandler;

        inventoryController.OnItemCollected += OnItemCollectedHandler;

        inventoryView.OnItemRemovedAtSlot += OnItemRemovedAtSlotHandler;
    }

    private void UnsubscribeEvents()
    {
        objectiveController.OnObjectivesStarted -= OnObjectivesStartedHandler;
        objectiveController.OnRequestNewRecipe -= OnRequestNewRecipeHandler;

        inventoryController.OnItemCollected -= OnItemCollectedHandler;

        inventoryView.OnItemRemovedAtSlot -= OnItemRemovedAtSlotHandler;
    }

    #region Objective Handler event handlers

    private void OnObjectivesStartedHandler()
    {
        Debug.Log("OnObjectivesStartedHandler");

        objectiveController.SetPoisonRecipe(recipeController.PoisonRecipe());
    }

    private void OnRequestNewRecipeHandler()
    {
        Debug.Log("OnRequestNewRecipeHandler");

        objectiveController.SetCurrentRecipe(recipeController.NextRecipe());
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
}
