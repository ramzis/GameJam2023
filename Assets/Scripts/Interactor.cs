using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Collider[] hitColliders;

    private TextBoxController textBoxController;
    private InventoryData inventoryData;

    private void Start()
    {
        textBoxController = FindObjectOfType<TextBoxController>(true);
    }

    private void Update()
    {
        //if (textBoxController.Busy()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Prevent actions with a not full inventory
            if (inventoryData != null && !inventoryData.InventoryFull()) return;

            hitColliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (var hitCollider in hitColliders)
            {
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();
                if (interactable == null) continue;
                interactable?.Interact();
                break;
            }
        }
    }

    public void ProvideInventory(InventoryData inventoryData)
    {
        this.inventoryData = inventoryData;
    }
}
