using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "Data/Inventory", order = 1)]
public class InventoryData : ScriptableObject
{
    public CategoryData[] currentCategories;

    [SerializeField]
    private CategoryData[] inventoryCategories1;
    [SerializeField]
    private CategoryData[] inventoryCategories2;

    public ItemData[] currentItems;

    public void OnEnable()
    {
        currentCategories = inventoryCategories1;
        currentItems = new ItemData[]
        {
            null, null, null, null, null
        }; 
    }

    // Attempts to find an empty matching category slot based on the current
    // inventory type and insert the item.
    // Returns >0 if the item was inserted into that position, else -1.
    public int TryAddItem(ItemData item)
    {
        Debug.Log("[InventoryData:TryAddItem] Looking for room in inventory");

        // Search for an empty category slot and fill
        for (int i = 0; i < currentCategories.Length; i++)
        {
            if (currentCategories[i].category != item.category) continue;
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
}