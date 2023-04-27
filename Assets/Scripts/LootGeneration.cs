using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

public class LootGeneration : MonoBehaviour
{
    private Inventory _inventory;
    [SerializeField] private LootTable _lootTable;
    private int _maxValue;
    [SerializeField] private int TEST_AMOUNT;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        GenerateLoot();
    }

    private void GetMaxValue()
    {
        _maxValue = 0;
        
        foreach (var loot in _lootTable._lootDrops)
        {
            _maxValue += loot._value;
        }
    }

    private int GenerateRandom()
    {
        var random = Mathf.RoundToInt(UnityEngine.Random.Range(0, _maxValue));

        return random;
    }

    [Button("Generate Random Loot")]
    private void GenerateLoot()
    {
        _inventory._inventory.Clear();
        GetMaxValue();
        
        foreach (var loot in _lootTable._lootDrops)
        {
            var randomInt = GenerateRandom();

            if (randomInt < loot._value)
            {
                _inventory.AddItem(loot);
                loot._amountDropped += 1;
            }
        }
    }
    
    [Button("Test Loot")]
    private void TestLootGeneration()
    {
        foreach (var item in _lootTable._lootDrops)
        {
            item._amountDropped = 0;
        }
        
        for (int i = 0; i < TEST_AMOUNT; i++)
        {
            GenerateLoot();
        }
        
        DisplayDebugLog();
    }

    private void OutputToTXT()
    {
        string path = Application.dataPath + "/LootDropLog.txt";

        File.WriteAllText(path, "");
        
        foreach (var item in _lootTable._lootDrops)
        {
            string content = $"!{item._amountDropped},";
            File.AppendAllText(path, content);
        }
        File.AppendAllText(path, "\n");
    }

    private void DisplayDebugLog()
    {
        Debug.Log($"In {TEST_AMOUNT} runs these were the drops of each item:\n");

        foreach (var item in _lootTable._lootDrops)
        {
            double percentageDrop = (float)item._amountDropped / (float)TEST_AMOUNT * 100;
            Debug.Log($"{item._itemName} dropped {item._amountDropped} times which equates to a {percentageDrop}% drop chance.\n");
        }
    }
}
