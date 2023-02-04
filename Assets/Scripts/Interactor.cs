using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Collider[] hitColliders;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
}
