using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : NetworkBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;

    public void SetHUDHealth(float max, float current)
    {
        _healthSlider.value = current / max;
    }

    public void SetHUDHunger(float max, float current)
    {
        _hungerSlider.value = current / max;
    }

    public void SetHUDThirst(float max, float current)
    {
        _thirstSlider.value = current / max;
    }
}
