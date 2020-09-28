using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    public BatteryManager batteryManager;
    public Player player;
    public Slider hpSlider;
    public Text text;

    public BatteryDial[] batteryDial;

    public float currentHP;
    public float currentMP;

    private float fullHP;
    private float fullMP;


    void Start()
    {
        fullHP = player.startingHealth;
        fullMP = batteryManager.MaxBatteryVolume;
    }

    // Update is called once per frame
    void Update()
    {
        GetMPs();
        GetHP();
    }

    private void GetMPs()
    {
        for (int i = 0; i < batteryManager.batteries.Length; i++)
        {
            batteryDial[i].energyPercent = batteryManager.batteries[i].currentBatteryVolume / fullMP;
            batteryDial[i].index = i;
        }

    }

    private void GetHP()
    {
        currentHP = player.currentHealth;
        hpSlider.value = currentHP / fullHP;
        text.text = currentHP.ToString() + " %";
    }
}
