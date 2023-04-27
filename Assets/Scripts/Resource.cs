using System;
using UnityEngine;

public class Resource : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    private Inventory _inventory;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _inventory = GetComponent<Inventory>();
    }

    public void OnDamage(float damage, GameObject attackerGO)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            if (attackerGO.GetComponent<Inventory>() != null)
            {
                for (var i = 0; i < _inventory._inventory.Count; i++)
                {
                    if (attackerGO.GetComponent<Inventory>().AddItem(_inventory._inventory[i]))
                    {
                        attackerGO.GetComponent<Inventory>().AddItem(_inventory._inventory[i]);
                    }

                    _inventory.RemoveItem(i);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
