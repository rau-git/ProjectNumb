using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : NetworkBehaviour
{
    [SerializeField] private Image _healthSlider;
    [SerializeField] private Image _hungerSlider;
    [SerializeField] private Image _thirstSlider;

    [SerializeField] private List<Image> _inventorySprites;
    [SerializeField] private Inventory _playerInventory;

    private void Start()
    {
        _playerInventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        InventoryHUD();
    }

    private void InventoryHUD()
    {
        for (var i = 0; i < _playerInventory._inventory.Count; i++)
        {
            _inventorySprites[i].sprite = _playerInventory._inventory[i]._uiImage;
        }
    }

    [TargetRpc]
    public void SetHUDHealth(NetworkConnection connection, float max, float current)
    {
        _healthSlider.fillAmount = current / max;
    }

    [TargetRpc]
    public void SetHUDHunger(NetworkConnection connection, float max, float current)
    {
        _hungerSlider.fillAmount = current / max;
    }

    [TargetRpc]
    public void SetHUDThirst(NetworkConnection connection, float max, float current)
    {
        _thirstSlider.fillAmount = current / max;
    }
}
