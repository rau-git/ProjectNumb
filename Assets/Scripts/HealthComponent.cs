using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class HealthComponent : NetworkBehaviour, IDamageable
{
    private HUDManager _hudManager;
    
    [SerializeField, SyncVar]
    private float _currentHealth;
    
    [SerializeField, SyncVar]
    private float _maxHealth;

    private void Awake()
    {
        _hudManager = GetComponentInParent<HUDManager>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void OnHeal(float healAmount)
    {
        _currentHealth += healAmount;
    }

    public void OnDamage(float damageAmount, GameObject attackerGO)
    {
        if (_currentHealth - damageAmount <= 0f)
        {
            Destroy(gameObject);
        }

        _currentHealth -= damageAmount;
        _hudManager.SetHUDHealth(NetworkObject.LocalConnection, _maxHealth, _currentHealth);
    }
}
