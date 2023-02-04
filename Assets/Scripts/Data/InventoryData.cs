using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "Data/Inventory", order = 1)]
public class InventoryData : ScriptableObject
{
    public ItemData.Category[] currentInventoryCategories;

    private ItemData.Category[] inventoryCategories1 = new ItemData.Category[] {
        ItemData.Category.Root,
        ItemData.Category.Sap,
        ItemData.Category.Any,
    };

    private ItemData.Category[] inventoryCategories2 = new ItemData.Category[] {
        ItemData.Category.Root,
        ItemData.Category.Root,
        ItemData.Category.Sap,
        ItemData.Category.Any,
    };

    public ItemData[] currentItems;

    public void OnEnable()
    {
        currentInventoryCategories = inventoryCategories1;
        currentItems = new ItemData[]
        {
            null, null, null, null, null
        }; 
    }

    // Attempts to find an empty matching category slot based on the current
    // inventory type and insert the item.
    // Returns true if the item was inserted.
    public bool TryAddItem(ItemData item)
    {
        // Search for an empty category slot and fill
        for (int i = 0; i < currentInventoryCategories.Length; i++)
        {
            if (currentInventoryCategories[i] != item.category) continue;
            if (currentItems[i] != null) continue;

            currentItems[i] = item;
            return true;
        }

        return false;
    }
}