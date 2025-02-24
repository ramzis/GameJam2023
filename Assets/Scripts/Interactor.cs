using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Collider[] hitColliders;

    private InventoryData inventoryData;

    private bool interactionLocked;

    private void Start()
    {
        interactionLocked = false;
    }

    private void Update()
    {
        if (interactionLocked) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Prevent actions with a not full inventory
            //if (inventoryData != null && !inventoryData.InventoryFull()) return; // TODO: reenable

            hitColliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (var hitCollider in hitColliders)
            {
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();
                IItemReceiver receiver = hitCollider.GetComponent<IItemReceiver>();
                if (interactable == null && receiver == null) continue;
                receiver?.ReceiveItems(new List<ItemData>()); // TODO: pass real items
                interactable?.Interact();
                break;
            }
        }
    }

    public void ProvideInventory(InventoryData inventoryData)
    {
        this.inventoryData = inventoryData;
    }

    public void LockInteraction(bool locked)
    {
        lock (this)
        {
            interactionLocked = locked;
        }
    }
}
