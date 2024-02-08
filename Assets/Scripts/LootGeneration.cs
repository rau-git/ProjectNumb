using System.IO;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

public class LootGeneration : NetworkBehaviour
{
    private Inventory _inventory;
    [SerializeField] private LootTable _lootTable;
    private int _maxValue;

    [Server]
    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        GenerateLoot();
    }

    private void GetMaxValue()
    {
        _maxValue = 0;

        foreach (var lootItem in _lootTable._lootTable)
        {
            _maxValue += lootItem.Value;
        }
    }

    private int GenerateRandom()
    {
        var random = Mathf.RoundToInt(UnityEngine.Random.Range(0, _maxValue));

        return random;
    }

    [Button("Re-Roll Loot"), Server]
    private void GenerateLoot()
    {
        _inventory._inventory.Clear();
        GetMaxValue();

        foreach (var lootItem in _lootTable._lootTable)
        {
            var randomInt = GenerateRandom();

            if (randomInt < lootItem.Value)
            {
                _inventory.AddItem(lootItem.Key);
                LogDetails(lootItem.Value, lootItem.Key._itemName);
            }
        }
    }

    [Server]
    private void LogDetails(int inputProbability, string inputName)
    {
        var dropChance = (float)inputProbability / _maxValue;
        dropChance *= 100;
        Debug.Log($"{inputName} dropped with a chance of {dropChance:F2}% \n");
    }

}
