using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryDial : MonoBehaviour
{
    [Range(0, 1)]
    public float energyPercent;
    public int index;

    public BatteryManager batteryManager;

    public Image energyEquipped;
    public Image energyCharging;

    public Image energyEquippedBG;
    public Image energyChargingBG;




    // Update is called once per frame
    void Update()
    {
        if (batteryManager.batteries[index].state == BatteryManager.BatteryState.Equipped)
        {
            energyEquipped.gameObject.SetActive(true);
            energyCharging.gameObject.SetActive(false);
            energyEquippedBG.gameObject.SetActive(true);
            energyChargingBG.gameObject.SetActive(false);
            energyEquipped.fillAmount = energyPercent;

        }
        else if (batteryManager.batteries[index].state == BatteryManager.BatteryState.Charging)

        {
            energyEquipped.gameObject.SetActive(false);
            energyCharging.gameObject.SetActive(true);
            energyEquippedBG.gameObject.SetActive(false);
            energyChargingBG.gameObject.SetActive(true);
            energyCharging.fillAmount = energyPercent;

        }
    }
}
