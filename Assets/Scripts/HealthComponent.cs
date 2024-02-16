using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public class HealthComponent : NetworkBehaviour, IDamageable
{
    [SerializeField] private HUDManager _hudManager;
    
    [SerializeField, SyncVar(Channel = Channel.Reliable, OnChange = nameof(on_health))]
    private float _currentHealth;
    
    [SerializeField, SyncVar]
    private float _maxHealth;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        SetMaxHealth();
    }

    [Server]
    private void SetMaxHealth()
    {
        _currentHealth = _maxHealth;
    }

    [Server]
    public void OnHeal(float healAmount)
    {
        _currentHealth += healAmount;
    }

    [Server]
    public void OnDamage(float damageAmount, GameObject attackerGO)
    {
        if (_currentHealth - damageAmount <= 0f)
        {
            Destroy(gameObject);
        }

        _currentHealth -= damageAmount;
    }

    private void on_health(float previous, float next, bool asServer)
    {
        _hudManager.SetHUDHealth(_maxHealth, _currentHealth);
    }
}
