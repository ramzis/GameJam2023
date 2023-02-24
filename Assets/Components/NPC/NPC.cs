using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Texture defaultTexture;

    private new Renderer renderer;

    private NPCController npcController;

    protected void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        if(!renderer)
        {
            throw new MissingComponentException(
                $"{gameObject.name} requires Renderer!"
            );
        }

        npcController = FindObjectOfType<NPCController>(true);
        if (!npcController)
        {
            Debug.LogWarning($"[{gameObject.name}]: No NPCController found!");
        }
    }

    private void OnDisable()
    {
        npcController?.Unregister(this);
    }

    protected void Start()
    {
        SetTexture(GetDefaultTexture());
        npcController?.Register(this);
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
        npcController.NotifyEvent(this, message);
    }

    public abstract void Interact();

    public abstract void ReceiveMessage(string message);
}
