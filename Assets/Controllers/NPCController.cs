using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static ObjectiveController;

public class NPCController : MonoBehaviour, IListen<NPC>, IListen<ObjectiveController>
{
    private List<NPC> npcs;

    private EventListeners<NPCController> eventListeners;

    #region Lifecycle Methods

    private void Awake()
    {
        npcs = new List<NPC>();
        eventListeners = new EventListeners<NPCController>(this);
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void Update()
    {
        //InteractRandomly();
    }

    private void OnDisable()
    {
        npcs.Clear();
    }

    private void RegisterEvents()
    {
        eventListeners.RegisterEvent(new Event_Say());
    }

    #endregion

    private void InteractRandomly()
    {
        if (Time.frameCount % 60 != 0) return;

        foreach(var npc in npcs)
        {
            if (Random.Range(0f, 1f) < 0.8) continue;
            npc.Interact();
        }
    }

    public (List<int>, Action<dynamic>) HandleEvent(NPC component, IEvent<NPC> @event)
    {
        if(!npcs.Contains(component)) npcs.Add(component);

        return @event switch
        {
            NPC.ClickedEvent e => (
                new List<int>() { 0 },
                (payload) =>
                {
                    payload.ReceiveMessage("Cycle mood");
                }
            ),
            NPC.TextEvent e => (
                new List<int>() { 0 },
                (payload) =>
                {
                    //Debug.Log($"[NPCController]: ({component.name}) says: {payload}");
                    eventListeners.RaiseEvent(new Event_Say(payload));
                }
            ),
            _ => (null, null)
        };
    }

    public (List<int>, Action<dynamic>) HandleEvent(ObjectiveController component, IEvent<ObjectiveController> @event)
    {
        return @event switch
        {
            Objective1Event e => (
                new List<int>() { 0 },
                (_) =>
                {
                    foreach (var npc in npcs) npc.ReceiveMessage("Get ready for Objective 1");
                }
            ),
            _ => (null, null)
        };
    }

    public class Event_Say : IEvent<NPCController>
    {
        private readonly string message;

        public Event_Say(string message = "")
        {
            this.message = message;
        }

        public string GetName() => "NPCControllerSayEvent";

        public dynamic GetPayload() => message;
    }
}
