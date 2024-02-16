using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public class PlayerHungerManager : NetworkBehaviour
{
    [SerializeField] private float _maxHunger;
    [SerializeField] private float _maxThirst;
    
    [SyncVar(Channel = Channel.Reliable, OnChange = nameof(on_hunger)), SerializeField] private float _hunger;
    [SyncVar(Channel = Channel.Reliable, OnChange = nameof(on_thirst)), SerializeField] private float _thirst;

    [SerializeField] private float _decayTimeHunger;
    [SerializeField] private float _decayTimeThirst;

    [SerializeField] private float _hungerDrainRate;
    [SerializeField] private float _thirstDrainRate;

    [SerializeField] private HUDManager _hudManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        EnableHunger();
        EnableThirst();
    }

    [Server]
    private void EnableHunger()
    {
        InvokeRepeating(nameof(CauseHunger), 0, _decayTimeHunger);
    }

    [Server]
    private void EnableThirst()
    {
        InvokeRepeating(nameof(CauseThirst), 0, _decayTimeThirst);
    }
    
    [Server]
    private void DisableThirst()
    {
        CancelInvoke(nameof(CauseThirst));
    }

    [Server]
    private void DisableHunger()
    {
        CancelInvoke(nameof(CauseHunger));
    }

    [Server]
    private void CauseHunger()
    {
        _hunger -= _hungerDrainRate;
    }
    
    [Server]
    private void CauseThirst()
    {
        _thirst -= _thirstDrainRate;
    }

    private void on_hunger(float previous, float next, bool asServer)
    {
        _hudManager.SetHUDHunger(_maxHunger, _hunger);
    }
    
    private void on_thirst(float previous, float next, bool asServer)
    {
        _hudManager.SetHUDThirst(_maxThirst, _thirst);
    }
}
