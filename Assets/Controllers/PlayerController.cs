using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePlayer))]
public class PlayerController : MonoBehaviour, IListen<TextBoxController>
{
    private MovePlayer movePlayer;

    private void Awake()
    {
        movePlayer = GetComponent<MovePlayer>();
    }

    public (List<int>, Action<dynamic>) HandleEvent(TextBoxController component, IEvent<TextBoxController> @event)
    {
        return @event switch
        {
            TextBoxController.Event_ToggleTextBox e => (
                new List<int>() { 0 },
                (payload) =>
                {
                    bool active = payload;
                    movePlayer.LockMovement(active);
                }
            ),
            _ => (null, null)
        };
    }
}
