using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : NetworkBehaviour
{
    [SerializeField] private Image _healthSlider;
    [SerializeField] private Image _hungerSlider;
    [SerializeField] private Image _thirstSlider;

    [SerializeField] private List<Image> _inventorySprites;
    [SerializeField] private Inventory _playerInventory;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (!IsOwner) return;
        
        _playerInventory = GetComponent<Inventory>();
    }

    private void Update()
    {
       // InventoryHUD();
    }

    private void InventoryHUD()
    {
        for (var i = 0; i < _playerInventory._inventory.Count; i++)
        {
            _inventorySprites[i].sprite = _playerInventory._inventory[i]._uiImage;
        }
    }
    
    public void SetHUDHealth(float max, float current)
    {
        _healthSlider.fillAmount = current / max;
    }

    public void SetHUDHunger(float max, float current)
    {
        _hungerSlider.fillAmount = current / max;
    }
    
    public void SetHUDThirst(float max, float current)
    {
        _thirstSlider.fillAmount = current / max;
    }
}
