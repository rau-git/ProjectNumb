using UnityEngine;
using UnityEngine.UI;

public abstract class BaseItem : ScriptableObject
{
    public string _itemName;
    public bool _isStackable;
    public int _quantity;

    public abstract void UseItem();
}
