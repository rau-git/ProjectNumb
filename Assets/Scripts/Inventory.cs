using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<BaseItem> _inventory = new();
    [SerializeField] private int _inventoryMaxSlots = 8;

    private const int INVENTORY_ITEM_NOT_FOUND = -1;
    
    public bool AddItem(BaseItem item)
    {
        if (_inventory.Count >= _inventoryMaxSlots)
        {
            return false;
        }

        if (item._isStackable)
        {
            var itemIndex = CheckForItem(item);
            
            if (itemIndex != INVENTORY_ITEM_NOT_FOUND)
            {
                _inventory[itemIndex]._quantity += item._quantity;
                return true;
            }
        }
        
        _inventory.Add(item);
        return true;
    }

    private int CheckForItem(BaseItem item)
    {
        for (var i = 0; i < _inventory.Count; i++)
        {
            if (item.name == _inventory[i]._itemName)
            {
                return i;
            }
        }
        return INVENTORY_ITEM_NOT_FOUND;
    }

    public void RemoveItem(int itemIndex)
    {
        _inventory.RemoveAt(itemIndex);
    }
}
