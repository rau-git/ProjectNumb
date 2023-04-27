using UnityEngine;
using UnityEngine.UI;

public abstract class BaseItem : ScriptableObject
{
    public string _itemName;
    public string _itemDescription;
    public Image _itemIcon;
    public bool _isStackable;
    public int _quantity;
    public int _value;
    public int _amountDropped;

    public abstract void UseItem();
}
