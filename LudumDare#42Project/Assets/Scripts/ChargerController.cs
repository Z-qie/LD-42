using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerController : MonoBehaviour
{
    public Player player;
    public BatteryManager batteryManager;
    public Transform batteryGraphics;

    public enum ChargerState { isCharging, isEmpty };

    public ChargerState chargerState = ChargerState.isEmpty;

    public int batteryIndex;

    public float timeToFullVolume = 60f;
    public float minLightIntensity = 0.2f;
    public float maxLightIntensity = 2f;


    public void ControllChager()
    {
        if (chargerState == ChargerState.isEmpty)
        {
            chargerState = ChargerState.isCharging;
            StartCoroutine(ChargeUp());
        }
        else if (chargerState == ChargerState.isCharging)
        {
            chargerState = ChargerState.isEmpty;
            RetriveBattery();

        }
    }

    private void RetriveBattery()
    {
        // set charger inactive 
        batteryGraphics.gameObject.SetActive(false);

        // set battery of gun active 
        batteryManager.batteries[batteryIndex].batteryT.gameObject.SetActive(true);
        batteryManager.batteries[batteryIndex].state = BatteryManager.BatteryState.Equipped;

    }

    IEnumerator ChargeUp()
    {
        // find the battery with min volume
        float lastComparedVolume = 1000f;
        for (int i = 0; i < batteryManager.batteries.Length; i++)
        {
            if (batteryManager.batteries[i].state == BatteryManager.BatteryState.Equipped && batteryManager.batteries[i].currentBatteryVolume < lastComparedVolume)
            {
                batteryIndex = i;
                lastComparedVolume = batteryManager.batteries[i].currentBatteryVolume;
            }
        }

        if (batteryManager.batteries[batteryIndex].state == BatteryManager.BatteryState.Equipped)
        {
            // set charger active and ligth intensity
            batteryGraphics.gameObject.SetActive(true);
            batteryGraphics.gameObject.GetComponent<Light>().intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, batteryManager.batteries[batteryIndex].currentBatteryVolume / 100);

            // set battery of gun inactive 
            batteryManager.batteries[batteryIndex].batteryT.gameObject.SetActive(false);
            batteryManager.batteries[batteryIndex].state = BatteryManager.BatteryState.Charging;

            while (chargerState == ChargerState.isCharging)
            {
                // keep charging!
                batteryManager.batteries[batteryIndex].currentBatteryVolume += 100 * Time.deltaTime / timeToFullVolume;

                if (batteryManager.batteries[batteryIndex].currentBatteryVolume >= 100f)
                {
                    batteryManager.batteries[batteryIndex].currentBatteryVolume = 100f;

                    print("FullyCharged: " + batteryIndex);
                }
                else
                {
                    player.powerRemaining -= 100 * Time.deltaTime / timeToFullVolume;
                }
                yield return null;
            }
        }
        else
        {
            chargerState = ChargerState.isEmpty;
        }
    }


}

