using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable, IItemReceiver
{
    [SerializeField]
    private Texture defaultTexture;

    private new Renderer renderer;

    protected EventListeners<NPC> eventListeners;

    protected void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        if(!renderer)
        {
            throw new MissingComponentException(
                $"{gameObject.name} requires Renderer!"
            );
        }


        eventListeners = new EventListeners<NPC>(this);
    }

    private void OnDisable()
    {
        //npcRegister?.Unregister(this);
    }

    protected void Start()
    {
        SetTexture(GetDefaultTexture());
        //npcRegister?.Register(this);
    }

    protected Texture GetDefaultTexture()
    {
        return defaultTexture;
    }

    protected void SetTexture(Texture texture)
    {
        renderer.material.mainTexture = texture;
    }

    protected void NotifyEvent(string message)
    {
        Debug.Log($"[NPC]: ({name}) says: {message}");

        //npcRegister.NotifyEvent(this, message);
    }

    public abstract void Interact();

    public abstract void ReceiveMessage(string message);

    public abstract void ReceiveItems(List<ItemData> items);

    public class ClickedEvent : IEvent<NPC>
    {
        private NPC npc;

        public ClickedEvent(NPC npc = null)
        {
            this.npc = npc;
        }

        public string GetName() => "NPCClickedEvent";

        public dynamic GetPayload() => npc;
    }

    public class TextEvent : IEvent<NPC>
    {
        private string text;

        public TextEvent(string text = "")
        {
            this.text = text;
        }

        public string GetName() => "TextEvent";

        public dynamic GetPayload() => text;
    }
}
