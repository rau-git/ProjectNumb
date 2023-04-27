using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LootTable", menuName = "Items/LootTable")]
public class LootTable : ScriptableObject
{
    public List<BaseItem> _lootDrops;
}
