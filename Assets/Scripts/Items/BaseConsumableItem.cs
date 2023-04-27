using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable")]
public class ConsumableItem : BaseItem
{
    public float _foodValue;
    public float _waterValue;
    
    public override void UseItem()
    {
        Debug.Log($"{_itemName} used");
    }
}
