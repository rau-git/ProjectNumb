using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        //RPCInformServer(base.OwnerId.ToString(), _hunger.ToString(), _thirst.ToString());
    }

    [ServerRpc]
    private void RPCInformServer(string sender, string messageHunger, string messageThirst)
    {
        //Debug.Log($"{sender}: my stats are food: {messageHunger} and thirst: {messageThirst}");
    }

    private void EnableHunger()
    {
        InvokeRepeating(nameof(CauseHunger), 0, _decayTimeHunger);
    }

    private void EnableThirst()
    {
        InvokeRepeating(nameof(CauseThirst), 0, _decayTimeThirst);
    }
    
    private void DisableThirst()
    {
        CancelInvoke(nameof(CauseThirst));
    }

    private void DisableHunger()
    {
        CancelInvoke(nameof(CauseHunger));
    }

    private void CauseHunger()
    {
        if (!IsOwner || !IsServer) return; 
        RPCUpdateHunger(base.Owner);
    }

    private void CauseThirst()
    {
        if (!IsOwner || !IsServer) return; 
        RPCUpdateThirst(base.Owner);
    }

    [ServerRpc]
    private void RPCUpdateThirst(NetworkConnection connection)
    {
        _thirst -= _thirstDrainRate;
        _hudManager.SetHUDThirst(connection, _maxThirst, _thirst);
    }

    [ServerRpc]
    private void RPCUpdateHunger(NetworkConnection connection)
    {
        _hunger -= _hungerDrainRate;
        _hudManager.SetHUDHunger(connection, _maxHunger, _hunger);
    }
}
