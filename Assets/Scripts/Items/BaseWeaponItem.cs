using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class BaseWeaponItem : BaseItem
{
    public float _durability;
    public float _damage;
    
    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }
}