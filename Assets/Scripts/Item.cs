using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour, IHarvestable
{
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
        Sap
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

        switch(state)
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

            case State.Sap: 
            if (harvesting)
            {
                Digging();
                Invoke("Sap", 2);
            }
            else
            {
                StopDigging();
                CancelInvoke("Sap");
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
        quadAnimation.StopDigging();
    }

    private void DigUp() // invokes after 5 seconds if not canceled
    {
        SetState(State.Pulled);
    }

    private void Sap()
    {
        StopDigging();
        collector.Collect(data);
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
}
