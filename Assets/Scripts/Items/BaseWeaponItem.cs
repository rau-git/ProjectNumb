using UnityEngine;
using UnityEngine.UI;

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
