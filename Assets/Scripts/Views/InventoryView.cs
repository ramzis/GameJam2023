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
        inventoryData.OnCategoriesUpdated += OnCategoriesUpdatedHandler;
        inventoryData.OnItemDataUpdated += OnItemDataUpdatedHandler;
    }

    private void OnDisable()
    {
        if (inventoryData == null) return;
        inventoryData.OnCategoriesUpdated -= OnCategoriesUpdatedHandler;
        inventoryData.OnItemDataUpdated -= OnItemDataUpdatedHandler;
    }

    public void RegenerateInventory(CategoryData[] categories)
    {
        for(int i=0; i<slots.Count; i++)
        {
            var slot = slots[i];
            slot.OnRemoveButtonPressed -= OnRemoveButtonPressedHandler(i);
            Destroy(slot.gameObject);
        }

        slots.Clear();

        // Create slots
        for(int i=0; i<categories.Length; i++)
        {
            var category = categories[i];

            Debug.Log($"[InventoryView:RegenerateInventory] Adding slot for {category.category}");

            var clone = Instantiate(
                slotPrefab.gameObject,
                slotPrefab.transform.parent
            );

            var slot = clone.GetComponent<Slot>();
            slot.ProvideData(category.slotPlaceholder);
            slot.OnRemoveButtonPressed += OnRemoveButtonPressedHandler(i);
            slot.SetState(Slot.State.Empty);
            slot.gameObject.SetActive(true);
            slots.Add(slot);
        }
    }

    private void PopulateInventory(ItemData[] itemData)
    {
        // Populate slots
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (itemData[i] == null)
            {
                slot.SetState(Slot.State.Empty);
            }
            else
            {
                slot.SetThumbnail(itemData[i].thumbnail);
                slot.SetState(Slot.State.Full);
            }

        }
    }

    // TODO: remove, obsolete
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

    private void OnItemDataUpdatedHandler(ItemData[] itemData)
    {
        PopulateInventory(itemData);
    }

    private void OnCategoriesUpdatedHandler(CategoryData[] categories)
    {
        RegenerateInventory(categories);
    }
}
