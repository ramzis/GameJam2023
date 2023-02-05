using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action<ItemData, int> OnItemCollected;
    public event Action<CategoryData[]> OnCategoriesUpdated;

    [SerializeField]
    private InventoryData inventoryData;

    private ItemCollector itemCollector;

    private void OnEnable()
    {
        itemCollector = FindObjectOfType<ItemCollector>(true);
        itemCollector.OnItemCollected += OnItemCollectedHandler;
        inventoryData.OnCategoriesUpdated += OnCategoriesUpdatedHandler;
        FindObjectOfType<Interactor>(true)?.ProvideInventory(inventoryData);
    }

    private void OnDisable()
    {
        itemCollector.OnItemCollected -= OnItemCollectedHandler;
        inventoryData.OnCategoriesUpdated -= OnCategoriesUpdatedHandler;
    }

    private void OnItemCollectedHandler(ItemData item)
    {
        Debug.Log("[InventoryController:CollectItem] Collected item");
        var insertedAt = inventoryData.TryAddItem(item);
        if(insertedAt >= 0) OnItemCollected?.Invoke(item, insertedAt);
    }

    public void RemoveItemAtSlot(int slot)
    {
        if(!inventoryData.TryRemoveItem(slot)) Debug.LogError("Failed to remove item");
    }

    public HashSet<ItemData> GetIngredients()
    {
        return new HashSet<ItemData>(inventoryData.currentItems);
    }

    public void EmptyInventory()
    {
        inventoryData.EmptyInventory();
    }

    public void RequestNextCategories()
    {
        inventoryData.NextCategories();
    }

    private void OnCategoriesUpdatedHandler(CategoryData[] categories)
    {
        OnCategoriesUpdated?.Invoke(categories);
    }
}
