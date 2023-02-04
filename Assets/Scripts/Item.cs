using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData data;

    [SerializeField]
    private State state;

    public enum State
    {
        Available,
        Empty
    }

    public void SetState(State state)
    {
        this.state = state;
    }
}
