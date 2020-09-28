using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public enum BatteryState { Equipped, Charging };
    public float MaxBatteryVolume = 100f;

    public float fullVolumeLightIntensity = 500f;
    public float lowVolumeLightIntensity = 20f;

    [System.Serializable]
    public class Battery
    {
        public Transform batteryT;
        public int batteryIndex;
        public BatteryState state = BatteryState.Equipped;
        [Range(0, 100)]
        public float currentBatteryVolume;
    }
    public Battery[] batteries;


    private void Update()
    {
        // set effect
        for (int i = 0; i < batteries.Length; i++)
        {
            if (batteries[i].state == BatteryState.Equipped)
            {
                // set light
                batteries[i].batteryT.gameObject.GetComponent<Light>().intensity = Mathf.Lerp(lowVolumeLightIntensity, fullVolumeLightIntensity, batteries[i].currentBatteryVolume / 100f);
            }
            else if (batteries[i].state == BatteryState.Charging)
            {
                batteries[i].batteryT.gameObject.GetComponent<Light>().intensity = 0;
            }
        }
    }


}
