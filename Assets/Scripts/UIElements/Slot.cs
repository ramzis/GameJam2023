using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public event Action OnRemoveButtonPressed;

    [SerializeField]
    private Image placeholder;
    [SerializeField]
    private Image thumbnail;
    [SerializeField]
    private Button removeButton;

    private State state;

    private void OnEnable()
    {
        removeButton.onClick.AddListener(OnRemoveButtonPressedHandler);
    }

    private void OnDisable()
    {
        removeButton.onClick.RemoveListener(OnRemoveButtonPressedHandler);
    }

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

    private void OnRemoveButtonPressedHandler()
    {
        OnRemoveButtonPressed?.Invoke();
    }
}
