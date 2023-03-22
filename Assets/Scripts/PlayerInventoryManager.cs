using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    private List<BaseItem> _playerInventory = new List<BaseItem>();
    private int _inventoryMaxSlots = 30;

    public bool AddItem(BaseItem item)
    {
        if (_playerInventory.Count >= _inventoryMaxSlots)
        {
            return false;
        }
        
        _playerInventory.Add(item);
        return true;
    }

    private void RemoveItem(BaseItem item)
    {
        _playerInventory.Remove(item);
    }
}
