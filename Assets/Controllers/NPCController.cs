using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private List<NPC> npcs;

    private void Awake()
    {
        npcs = new List<NPC>();
    }

    private void Update()
    {
        InteractRandomly();
    }

    private void OnDisable()
    {
        npcs.Clear();
    }

    private void InteractRandomly()
    {
        if (Time.frameCount % 60 != 0) return;

        foreach(var npc in npcs)
        {
            if (Random.Range(0f, 1f) < 0.8) continue;
            npc.Interact();
        }
    }

    public void Register(NPC npc)
    {
        npcs.Add(npc);
    }

    public void Unregister(NPC npc)
    {
        Debug.Log($"[NPCController]: Unregistering {npc.name}");
        npcs.Remove(npc);
    }

    public void NotifyEvent(NPC npc, string message)
    {
        Debug.Log($"[NPCController]: ({npc.name}) says: {message}");
        npc.ReceiveMessage("GOTCHA!");
    }
}
