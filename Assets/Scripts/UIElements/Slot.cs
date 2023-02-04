using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image placeholder;
    [SerializeField]
    private Image thumbnail;

    private State state;

    public enum State
    {
        Empty,
        Full,
    }

    public void SetState(State state)
    {
        this.state = state;

        switch(state)
        {
            case State.Empty:
                placeholder.enabled = true;
                thumbnail.enabled = false;
                break;
            case State.Full:
                placeholder.enabled = false;
                thumbnail.enabled = true;
                break;
            default:
                Debug.LogErrorFormat("Unhandled state: {}", state);
                break;
        }
    }

    public void ProvideData(Sprite placeholder)
    {
        this.placeholder.sprite = placeholder;
    }

    public void SetThumbnail(Sprite thumbnail)
    {
        this.thumbnail.sprite = thumbnail;
    }
}
