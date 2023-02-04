using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private InventoryData inventoryData;
    [SerializeField]
    private Slot slotPrefab; // Used to init new slots by cloning
    [SerializeField]
    private List<CategoryData> categoryData;

    private List<Slot> slots;

    private void OnEnable()
    {
        slots = new List<Slot>();
        RegenerateInventory();
    }

    // RegenerateInventory should only be called only with an empty inventory.
    // It will adjust the visible slot count and reset them to be empty.
    public void RegenerateInventory()
    {
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }

        slots.Clear();

        foreach (var category in inventoryData.currentCategories)
        {
            var clone = Instantiate(
                slotPrefab,
                slotPrefab.transform.parent
            );

            var slot = clone.GetComponent<Slot>();
            slot.ProvideData(category.slotPlaceholder);
            slot.SetState(Slot.State.Empty);
            slot.gameObject.SetActive(true);
        }
    }

    // Sets the visible item for a full slot
    public void SetSlotThumbnail(int slot, Sprite thumbnail)
    {
        if (slots.Count >= slot)
        {
            Debug.LogError("Atttempted to set slot to a too small inventory");
            return;
        }

        slots[slot].SetThumbnail(thumbnail);
    }
}
