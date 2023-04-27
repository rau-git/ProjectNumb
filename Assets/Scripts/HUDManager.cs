using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : NetworkBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;

    [SerializeField] private TextMeshProUGUI _healthNumber;
    [SerializeField] private TextMeshProUGUI _hungerNumber;
    [SerializeField] private TextMeshProUGUI _thirstNumber;

    [SerializeField] private List<TextMeshProUGUI> _inventoryTextList;
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
            _inventoryTextList[i].text = _playerInventory._inventory[i]._itemName;
        }
    }

    [TargetRpc]
    public void SetHUDHealth(NetworkConnection connection, float max, float current)
    {
        _healthSlider.value = current / max;
        _healthNumber.text = current.ToString();
    }

    [TargetRpc]
    public void SetHUDHunger(NetworkConnection connection, float max, float current)
    {
        _hungerSlider.value = current / max;
        _hungerNumber.text = current.ToString("F0");
    }

    [TargetRpc]
    public void SetHUDThirst(NetworkConnection connection, float max, float current)
    {
        _thirstSlider.value = current / max;
        _thirstNumber.text = current.ToString("F0");
    }
}
