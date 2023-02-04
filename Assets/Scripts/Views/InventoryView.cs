using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public event Action<int> OnItemRemovedAtSlot;

    [SerializeField]
    private InventoryData inventoryData;
    [SerializeField]
    private Slot slotPrefab; // Used to init new slots by cloning
    [SerializeField]
    private List<CategoryData> categoryData;

    private List<Slot> slots;

    private void Awake()
    {
        slots = new List<Slot>();
        RegenerateInventory();
    }

    // RegenerateInventory should only be called only with an empty inventory.
    // It will adjust the visible slot count and reset them to be empty.
    public void RegenerateInventory()
    {
        for(int i=0; i<slots.Count; i++)
        {
            var slot = slots[i];
            slot.OnRemoveButtonPressed -= OnRemoveButtonPressedHandler(i);
            Destroy(slot.gameObject);
        }

        slots.Clear();

        for(int i=0; i<inventoryData.CurrentCategories().Length; i++)
        {
            var category = inventoryData.CurrentCategories()[i];

            Debug.Log($"[InventoryView:RegenerateInventory] Adding slot for {category.category}");

            var clone = Instantiate(
                slotPrefab.gameObject,
                slotPrefab.transform.parent
            );

            var slot = clone.GetComponent<Slot>();
            slot.ProvideData(category.slotPlaceholder);
            slot.SetState(Slot.State.Empty);
            slot.OnRemoveButtonPressed += OnRemoveButtonPressedHandler(i);
            slot.gameObject.SetActive(true);
            slots.Add(slot);
        }
    }

    // Sets the visible item for a full slot
    public void SetSlotThumbnail(int slot, Sprite thumbnail)
    {
        if (slots.Count <= slot)
        {
            Debug.LogError("Atttempted to set slot to a too small inventory");
            return;
        }

        slots[slot].SetThumbnail(thumbnail);
        slots[slot].SetState(Slot.State.Full);
    }

    // Cleans up a slot
    public void RemoveItemAtSlot(int slot)
    {
        if (slots.Count <= slot)
        {
            Debug.LogError("Atttempted to set slot to a too small inventory");
            return;
        }

        slots[slot].SetState(Slot.State.Empty);
        slots[slot].SetThumbnail(null);

        OnItemRemovedAtSlot?.Invoke(slot);
    }

    private Action OnRemoveButtonPressedHandler(int slot)
    {
        return () => RemoveItemAtSlot(slot);
    }
}
