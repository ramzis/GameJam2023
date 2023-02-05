using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "Data/Inventory", order = 1)]
public class InventoryData : ScriptableObject
{
    public event Action<CategoryData[]> OnCategoriesUpdated;

    [SerializeField]
    private CategoryData[] inventoryCategories1;
    [SerializeField]
    private CategoryData[] inventoryCategories2;
    [SerializeField]
    private CategoryData[] inventoryCategories3;
    [SerializeField]
    private CategoryData[] inventoryCategories4;
    [SerializeField]
    private CategoryData[] inventoryCategories5;

    public ItemData[] currentItems;

    private int currentCategories;
    private List<CategoryData[]> inventories;

    public void OnEnable()
    {
        currentCategories = 0;

        currentItems = new ItemData[]
        {
            null, null, null, null, null
        };

        inventories = new List<CategoryData[]> {
            inventoryCategories1,
            inventoryCategories2,
            inventoryCategories3,
            inventoryCategories4,
            inventoryCategories5,
        };
    }

    // Attempts to find an empty matching category slot based on the current
    // inventory type and insert the item.
    // Returns >0 if the item was inserted into that position, else -1.
    public int TryAddItem(ItemData item)
    {
        Debug.Log("[InventoryData:TryAddItem] Looking for room in inventory");

        // Search for an empty category slot and fill
        for (int i = 0; i < inventories[currentCategories].Length; i++)
        {
            if (inventories[currentCategories][i].category != item.category) continue;
            if (currentItems[i] != null) continue;

            currentItems[i] = item;
            Debug.Log("[InventoryData:TryAddItem] Added to inventory");
            return i;
        }

        Debug.Log("[InventoryData:TryAddItem] No room in inventory");
        return -1;
    }

    // Attempts to remove an item at the slot position
    // Returns a boolean for success
    public bool TryRemoveItem(int slot)
    {
        if (currentItems.Length <= slot)
        {
            Debug.LogError("Atttempted to set slot to a too small inventory");
            return false;
        }

        currentItems[slot] = null;
        return true;
    }

    public void NextCategories()
    {
        if (currentCategories + 1 >= inventories.Count) return;
        currentCategories++;
        OnCategoriesUpdated?.Invoke(inventories[currentCategories]);
    }

    public CategoryData[] CurrentCategories()
    {
        return inventories[currentCategories];
    }

    public void EmptyInventory()
    {
        for(int i=0; i<currentItems.Length; i++)
        {
            if (currentItems[i] == null) continue;
            TryRemoveItem(i);
        }
    }
}