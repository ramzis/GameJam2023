using UnityEngine;

public class InventoryController : MonoBehaviour
{
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
        inventoryData.TryAddItem(item);
    }
}
