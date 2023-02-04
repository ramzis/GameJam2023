using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour, ICollector
{
    public event Action<ItemData> OnItemCollected;

    private List<IHarvestable> nearbyHarvestables;

    private void Start()
    {
        nearbyHarvestables = new List<IHarvestable>();
    }

    public void SetHarvesting(bool harvesting)
    {
        foreach(var harvestable in nearbyHarvestables)
        {
            var itemData = harvestable.Harvest(harvesting);
            OnItemCollected?.Invoke(itemData);
            return; // Just interact with the first one for now.
        }
    }

    public void RegisterHarvestable(IHarvestable harvestable)
    {
        Debug.Log($"[ItemCollector:RegisterHarvestable] Registered {harvestable}");
        nearbyHarvestables.Add(harvestable);
    }

    public void UnregisterHarvestable(IHarvestable harvestable)
    {
        Debug.Log($"[ItemCollector:RegisterHarvestable] Unregistered {harvestable}");
        nearbyHarvestables.Remove(harvestable);
    }
}

public interface ICollector
{
    // Methods raised by IHarvestable when their triggers are activated
    public void RegisterHarvestable(IHarvestable harvestable);
    public void UnregisterHarvestable(IHarvestable harvestable);
}

public interface IHarvestable
{
    // Notify a harvestable to change its harvesting state
    public ItemData Harvest(bool harvesting);
}
