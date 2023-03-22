using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class HealthComponent : NetworkBehaviour
{
    /*
     * this is a basic level of what should be a finished product
     * localising health to the client will allow the user to use
     * programs like cheat engine to change their values locally without
     * servers authorising
     *
     * the final product should be health values stored server side so the server can authorise
     * all changes to these values, same will apply to hunger and thirst, but being less important
     * values they should only be send periodically to the user
     * 
     */

    private HUDManager _hudManager;
    
    [SerializeField, SyncVar]
    private float _currentHealth;
    
    [SerializeField, SyncVar]
    private float _maxHealth;

    private void Awake()
    {
        _hudManager = GetComponentInParent<HUDManager>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnDamage(0);
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void OnHeal(float healAmount)
    {
        _currentHealth += healAmount;
    }

    public void OnDamage(float damageAmount)
    {
        if (_currentHealth - damageAmount <= 0f)
        {
            Destroy(gameObject);
        }

        _currentHealth -= damageAmount;
        _hudManager.SetHUDHealth(_maxHealth, _currentHealth);
    }
}
