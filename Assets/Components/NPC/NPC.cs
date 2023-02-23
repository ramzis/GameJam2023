using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Texture defaultTexture;

    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        if(!renderer)
        {
            throw new MissingComponentException(
                $"{gameObject.name} requires Renderer!"
            );
        }
        SetTexture(GetDefaultTexture());
    }

    protected Texture GetDefaultTexture()
    {
        return defaultTexture;
    }

    protected void SetTexture(Texture texture)
    {
        renderer.material.mainTexture = texture;
    }

    public abstract void Interact();
}
