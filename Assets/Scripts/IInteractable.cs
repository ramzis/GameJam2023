using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IInteractable
{
    public void Interact();
}

public interface IItemReceiver
{
    public void ReceiveItems(List<ItemData> items);
}
