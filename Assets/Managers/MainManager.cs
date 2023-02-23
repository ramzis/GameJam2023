using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RecipeController))]
[RequireComponent(typeof(InventoryController))]
[RequireComponent(typeof(ObjectiveController))]
[RequireComponent(typeof(WitchController))]
[RequireComponent(typeof(TextBoxController))]
public class MainManager : MonoBehaviour
{
    // 1. Receive recipe
    // 2. Collect to inventory
    // 3. Give to witch

    private RecipeController recipeController;
    private InventoryController inventoryController;
    private ObjectiveController objectiveController;
    private WitchController witchController;
    private TextBoxController textBoxController;

    [SerializeField]
    private InventoryView inventoryView;
    [SerializeField]
    private TextBoxView textBoxView;

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
        textBoxController = GetComponent<TextBoxController>();
    }

    private void SubscribeEvents()
    {
        objectiveController.OnRequestPoisonRecipe += OnRequestPoisonRecipeHandler;
        objectiveController.OnRequestNewRecipe += OnRequestNewRecipeHandler;
        objectiveController.OnRequestNewCategories += OnRequestNewCategoriesHandler;
        objectiveController.OnRequestIntroDialog += OnRequestIntroDialogHandler;
        objectiveController.OnRequestFirstIngredientDialog += OnRequestFirstIngredientDialogHandler;
        objectiveController.OnRequestCorrectIngredientDialog += OnRequestCorrectIngredientDialogHandler;
        objectiveController.OnRequestWrongIngredientDialog += OnRequestWrongIngredientDialogHandler;
        objectiveController.OnLivesCountChanged += OnLivesCountChangedHandler;

        witchController.OnRequestEvaluateRecipe += OnRequestEvaluateRecipeHandler;
        witchController.OnWitchPraiseRecipe += OnWitchPraiseRecipeHandler;

        inventoryController.OnItemCollected += OnItemCollectedHandler;
        inventoryController.OnCategoriesUpdated += OnCategoriesUpdatedHandler;

        inventoryView.OnItemRemovedAtSlot += OnItemRemovedAtSlotHandler;

        textBoxController.OnShowTextBox += OnShowTextBoxHandler;
        textBoxController.OnSwitchSpeaker += OnSwitchSpeakerHandler;
    }

    private void UnsubscribeEvents()
    {
        objectiveController.OnRequestPoisonRecipe -= OnRequestPoisonRecipeHandler;
        objectiveController.OnRequestNewRecipe -= OnRequestNewRecipeHandler;
        objectiveController.OnRequestNewCategories -= OnRequestNewCategoriesHandler;
        objectiveController.OnRequestIntroDialog -= OnRequestIntroDialogHandler;
        objectiveController.OnRequestFirstIngredientDialog -= OnRequestFirstIngredientDialogHandler;
        objectiveController.OnRequestCorrectIngredientDialog -= OnRequestCorrectIngredientDialogHandler;
        objectiveController.OnRequestWrongIngredientDialog -= OnRequestWrongIngredientDialogHandler;
        objectiveController.OnLivesCountChanged -= OnLivesCountChangedHandler;

        witchController.OnRequestEvaluateRecipe -= OnRequestEvaluateRecipeHandler;
        witchController.OnWitchPraiseRecipe -= OnWitchPraiseRecipeHandler;

        inventoryController.OnItemCollected -= OnItemCollectedHandler;
        inventoryController.OnCategoriesUpdated -= OnCategoriesUpdatedHandler;

        inventoryView.OnItemRemovedAtSlot -= OnItemRemovedAtSlotHandler;

        textBoxController.OnShowTextBox -= OnShowTextBoxHandler;
        textBoxController.OnSwitchSpeaker -= OnSwitchSpeakerHandler;
    }

    #region Objective Handler event handlers

    private void OnRequestPoisonRecipeHandler()
    {
        objectiveController.SetPoisonRecipe(recipeController.PoisonRecipe());
    }

    private void OnLivesCountChangedHandler(int lives)
    {
        var witchRender = FindObjectOfType<RenderWitch>();
        witchRender.SetMoodImg(lives);
        FindObjectOfType<TextBoxView>().SwitchSpeaker(Author.Witch, lives);
    }

    private void OnRequestNewRecipeHandler()
    {
        recipeController.NextRecipe();
        var recipe = recipeController.CurrentRecipe();
        objectiveController.SetCurrentRecipe(recipe);
        witchController.ProvideCurrentRecipe(recipe);
    }

    private void OnRequestNewCategoriesHandler()
    {
        inventoryController.RequestNextCategories();
    }

    private void OnRequestIntroDialogHandler(int level)
    {
        textBoxController.SayIntroDialog(level);
    }

    private void OnRequestFirstIngredientDialogHandler(int level)
    {
        textBoxController.SayFirstIngredientDialog(level);
    }

    private void OnRequestCorrectIngredientDialogHandler(int level)
    {
        textBoxController.SayCorrectIngredientDialog(level);
    }

    private void OnRequestWrongIngredientDialogHandler(int level)
    {
        textBoxController.SayWrongIngredientDialog(level);
    }

    #endregion

    #region Inventory Controller event handlers

    private void OnItemCollectedHandler(ItemData item, int insertedAt)
    {
        inventoryView.SetSlotThumbnail(insertedAt, item.thumbnail);

        var level = objectiveController.GetLevel();
        RecipeData recipe;
        if(level < 5)
            recipe = recipeController.CurrentRecipe();
        else
            recipe = recipeController.PoisonRecipe();

        if (recipe.ingredients.Contains(item))
            objectiveController.NotifyIngredientCollected(item);
    }

    #endregion

    #region Inventory View event handlers

    private void OnItemRemovedAtSlotHandler(int slot)
    {
        inventoryController.RemoveItemAtSlot(slot);
    }

    private void OnCategoriesUpdatedHandler(CategoryData[] categories)
    {
        inventoryView.RegenerateInventory();
    }

    #endregion


    #region Witch Controller event handlers

    private void OnRequestEvaluateRecipeHandler()
    {
        var ingredients = inventoryController.GetIngredients();
        var correct = objectiveController.EvaluateRecipe(ingredients);
        witchController.EvaluateRecipe(correct);
        inventoryController.EmptyInventory();
    }

    private void OnWitchPraiseRecipeHandler()
    {
        var ingredients = inventoryController.GetIngredients();
        witchController.BrewPotion(ingredients);
    }

    #endregion

    #region Text Box Controller event handlers

    private void OnShowTextBoxHandler(bool show)
    {
        if (show) textBoxView.Show();
        else textBoxView.Hide();
    }

    private void OnSwitchSpeakerHandler(Author speaker)
    {
        int lives = objectiveController.GetLives();
        textBoxView.SwitchSpeaker(speaker, lives);
    }

    #endregion
}
