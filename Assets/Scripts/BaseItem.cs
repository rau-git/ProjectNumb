using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public string _itemName;
    public string _itemDescription;
    public Sprite _itemIcon;

    public abstract void UseItem();
}
