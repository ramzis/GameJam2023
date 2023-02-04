using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public event Action<ItemData, int> OnItemCollected;

    [SerializeField]
    private InventoryData inventoryData;

    private void OnEnable()
    {
        FindObjectOfType<ItemCollector>(true).OnItemCollected +=
            OnItemCollectedHandler;
    }

    private void OnDisable()
    {
        FindObjectOfType<ItemCollector>(true).OnItemCollected -=
            OnItemCollectedHandler;
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
}
