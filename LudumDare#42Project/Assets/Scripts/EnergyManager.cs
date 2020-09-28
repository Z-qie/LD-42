using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{ 
    public float energyCharging;
    public float energyEquipped;

    public BatteryManager batteryManager;
    public Gun gun;

    private void Update()
    {
        UpdateEnergyDate();
    }

    private void UpdateEnergyDate()
    {
        // update Energy data
        energyEquipped = 0f;
        energyCharging = 0f;

        for (int i = 0; i < batteryManager.batteries.Length; i++)
        {
            if (batteryManager.batteries[i].state == BatteryManager.BatteryState.Equipped)
            {
                energyEquipped += batteryManager.batteries[i].currentBatteryVolume;
            }
            else if (batteryManager.batteries[i].state == BatteryManager.BatteryState.Charging)
            {
                energyCharging += batteryManager.batteries[i].currentBatteryVolume;
            }
        }

        if (energyEquipped <= gun.energyPerLaser)
        {
            print("No Energy!!");
        }
    }

    public void ConsumeEnergy(float energyConsumed)
    {
        for (int i = 0; i < batteryManager.batteries.Length; i++)
        {
            if (batteryManager.batteries[i].state == BatteryManager.BatteryState.Equipped && batteryManager.batteries[i].currentBatteryVolume >= energyConsumed)
            {
                batteryManager.batteries[i].currentBatteryVolume -= energyConsumed;
                return;
            }
            else if  (batteryManager.batteries[i].state == BatteryManager.BatteryState.Equipped && batteryManager.batteries[i].currentBatteryVolume < energyConsumed){
                energyConsumed -= batteryManager.batteries[i].currentBatteryVolume;
                batteryManager.batteries[i].currentBatteryVolume = 0f;
            }
        }

    }
}
