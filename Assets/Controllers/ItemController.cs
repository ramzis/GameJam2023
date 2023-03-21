using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IListen<Item>
{
    public (List<int>, Action<dynamic>) HandleEvent(Item component, IEvent<Item> @event)
    {
        return @event switch
        {
            Item.HarvestedEvent e => (
                new List<int>() { 0 },
                (payload) =>
                {
                    Debug.Log($"[ItemController]: Harvested {payload.title}");
                }
            ),
            _ => (null, null)
        };
    }
}
