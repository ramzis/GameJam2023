using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ItemCollector : MonoBehaviour, ICollector
{
    public event Action<ItemData> OnItemCollected;

    private List<IHarvestable> nearbyHarvestables;

    private void Start()
    {
        nearbyHarvestables = new List<IHarvestable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetHarvesting(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetHarvesting(false);
        }

    }

    public void SetHarvesting(bool harvesting)
    {
        
        foreach(var harvestable in nearbyHarvestables)
        {
            harvestable.Harvest(harvesting);
           // OnItemCollected?.Invoke(itemData);
            return; // Just interact with the first one for now.
        }
    }

    public void RegisterHarvestable(IHarvestable harvestable)
    {
        //Debug.Log($"[ItemCollector:RegisterHarvestable] Registered {harvestable}");
        nearbyHarvestables.Add(harvestable);
    }

    public void UnregisterHarvestable(IHarvestable harvestable)
    {
       // Debug.Log($"[ItemCollector:RegisterHarvestable] Unregistered {harvestable}");
        nearbyHarvestables.Remove(harvestable);
    }

    public void Collect(ItemData itemData)
    {
        OnItemCollected?.Invoke(itemData);
    }
}

public interface ICollector
{
    // Methods raised by IHarvestable when their triggers are activated
    public void RegisterHarvestable(IHarvestable harvestable);
    public void UnregisterHarvestable(IHarvestable harvestable);
    public void Collect(ItemData itemData);
}

public interface IHarvestable
{
    // Notify a harvestable to change its harvesting state
    public void Harvest(bool harvesting);
}
