using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public string _itemName;
    public bool _isStackable;
    public int _quantity;
    public Sprite _uiImage;

    public abstract void UseItem();
}
