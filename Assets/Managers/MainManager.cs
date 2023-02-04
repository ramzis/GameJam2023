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
        objectiveController.OnObjectivesExhausted += OnObjectivesExhaustedHandler;
        inventoryController.OnItemCollected += OnItemCollectedHandler;
    }

    private void UnsubscribeEvents()
    {
        objectiveController.OnObjectivesStarted -= OnObjectivesStartedHandler;
        objectiveController.OnObjectivesExhausted -= OnObjectivesExhaustedHandler;
        inventoryController.OnItemCollected -= OnItemCollectedHandler;
    }

    #region Objective Handler event handlers

    private void OnObjectivesStartedHandler()
    {
        Debug.Log("OnObjectivesStartedHandler");
    }

    private void OnObjectivesExhaustedHandler()
    {
        Debug.Log("OnObjectivesExhaustedHandler");
    }

    #endregion

    #region Inventory Controller event handlers

    private void OnItemCollectedHandler(ItemData item, int insertedAt)
    {
        inventoryView.SetSlotThumbnail(insertedAt, item.thumbnail);
    }

    #endregion
}
