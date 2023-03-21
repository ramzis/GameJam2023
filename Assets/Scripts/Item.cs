using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IHarvestable, IInteractable
{
    private EventListeners<Item> eventListeners;

    [SerializeField]
    private ItemData data;

    [SerializeField]
    private State state;

    [SerializeField]
    private GameObject underground;

    [SerializeField]
    private GameObject pulled;

    [SerializeField]
    private GameObject hole;

    [SerializeField]
    private SwayQuad quadAnimation;

    private ICollector collector;

    // Is the Item being harvested?
    // Used to update UI and harvesting state.
    private bool harvesting;

    public enum State
    {
        Available,
        Pulled,
        Held,
        Empty,
        Tree,
        Sap
    }

    public class HarvestedEvent : IEvent<Item>
    {
        private ItemData data;

        public HarvestedEvent(ItemData data = null)
        {
            this.data = data;
        }

        public string GetName() => "HarvestedEvent";

        public dynamic GetPayload() => data;
    }

    private void Awake()
    {
        eventListeners = new EventListeners<Item>(this);
    }

    private void Start()
    {
        eventListeners.RegisterEvent(new HarvestedEvent());
    }

    public void OnTriggerEnter(Collider other)
    {
        TryRegisterHarvestable(other);
    }

    public void OnTriggerExit(Collider other)
    {
        TryUnregisterHarvestable(other);
    }

    public void SetState(State state)
    {
        this.state = state;

        switch (this.state)
        {
            case State.Available:
                underground.SetActive(true);
                pulled.SetActive(false);
                hole.SetActive(false);
                break;

            case State.Pulled:
                underground.SetActive(false);
                pulled.SetActive(true);
                hole.SetActive(true);
                break;

            case State.Held:
                underground.SetActive(false);
                pulled.SetActive(true);
                hole.SetActive(false);
                break;

            case State.Empty:
                underground.SetActive(false);
                pulled.SetActive(false);
                hole.SetActive(true);
                GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false; // cringe
                GetComponent<SphereCollider>().center = new Vector3(0,-100,0); // yeet the trigger away.
                break;

            case State.Sap:
                //underground.SetActive(false);
                pulled.SetActive(true);
                hole.SetActive(true);
                break;

            case State.Tree:
                //underground.SetActive(false);
                pulled.SetActive(false);
                hole.SetActive(true);
                break;
        }
    }

    public string GetTitle()
    {
        return data.title;
    }

    #region Harvestables

    public void Harvest(bool harvesting)
    {
        this.harvesting = harvesting;

        if(harvesting)
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = true;
        else
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;


        switch (state)
        {
            case State.Available:
                if (harvesting)
                {
                    SoundManagerScript.PlaySound("diging");
                    Digging();
                    Invoke("DigUp", 2);
                }
                else
                {
                    StopDigging();
                    CancelInvoke("DigUp");
                }
                break;

            case State.Pulled:
                if (harvesting)
                {
                    SoundManagerScript.PlaySound("success");
                    collector.Collect(data);
                    SetState(State.Empty);
                }
                break;


            case State.Tree:
                if (harvesting)
                {
                    SoundManagerScript.PlaySound("diging");
                    Digging();
                    Invoke("Sap", 2);
                }
                else
                {
                    StopDigging();
                    CancelInvoke("Sap");
                }
                break;

            case State.Sap:
                if (harvesting)
                {
                    SoundManagerScript.PlaySound("success");
                    SetState(State.Tree);
                    collector.Collect(data);
                }

                break;
        }
    }


    private void Digging() // digging animation
    {
        quadAnimation.Dig();
    }

    private void StopDigging()
    {
        GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;
        quadAnimation.StopDigging();
    }

    private void DigUp() // invokes after 5 seconds if not canceled
    {
        GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;
        SetState(State.Pulled);
    }

    private void Sap()
    {
        GameObject.FindWithTag("Player").GetComponent<Rigidbody>().isKinematic = false;
        StopDigging();
        SetState(State.Sap);
    }

    private void TryRegisterHarvestable(Collider other)
    {
        var collector = other.GetComponentInChildren<ICollector>();
        if (collector != null)
        {
            this.collector = collector;
            collector.RegisterHarvestable(this);
        }
    }

    private void TryUnregisterHarvestable(Collider other)
    {
        var collector = other.GetComponentInChildren<ICollector>();
        if (collector != null) collector.UnregisterHarvestable(this);
    }

    #endregion

    public void Interact()
    {
        eventListeners.RaiseEvent(new HarvestedEvent(data));
    }
}
