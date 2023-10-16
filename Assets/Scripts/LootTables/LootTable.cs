using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New LootTable", menuName = "Items/LootTable")]
public class LootTable : SerializedScriptableObject
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "Item",
        ValueLabel = "Roll Weight")]
    public Dictionary<BaseItem, int> _lootTable = new Dictionary<BaseItem, int>();
}
