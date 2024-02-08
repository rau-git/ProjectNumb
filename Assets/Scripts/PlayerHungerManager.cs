using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerHungerManager : NetworkBehaviour
{
    [SerializeField]
    private float _maxHunger;
    [SerializeField]
    private float _maxThirst;
    
    [SyncVar, SerializeField]
    private float _hunger;
    [SyncVar, SerializeField]
    private float _thirst;

    [SerializeField] 
    private float _decayTimeHunger;
    [SerializeField]
    private float _decayTimeThirst;

    [SerializeField]
    private float _hungerDrainRate;
    [SerializeField]
    private float _thirstDrainRate;

    private HUDManager _hudManager;

    private void Awake()
    {
        _hudManager = GetComponentInParent<HUDManager>();
    }

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
        if (!IsOwner || !IsServer) return; 
        RpcUpdateHunger(base.Owner);
    }
    
    [Server]
    private void CauseThirst()
    {
        if (!IsOwner || !IsServer) return; 
        RpcUpdateThirst(base.Owner);
    }

    [TargetRpc]
    private void RpcUpdateHunger(NetworkConnection connection)
    {
        _hunger -= _hungerDrainRate;
        _hudManager.SetHUDHunger(connection, _maxHunger, _hunger);
    }

    [TargetRpc]
    private void RpcUpdateThirst(NetworkConnection connection)
    {
        _thirst -= _thirstDrainRate;
        _hudManager.SetHUDThirst(connection, _maxThirst, _thirst);
    }
}
