using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun startingGun;
    public Gun equippedGun;

    public Transform gunHeldPosition;



    // Start is called before the first frame update
    private void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    

    public void EquipGun(Gun gunToEquip)
    {
        if (equippedGun != null)
            Destroy(equippedGun.gameObject);
        equippedGun = Instantiate<Gun>(gunToEquip, gunHeldPosition.position, gunHeldPosition.rotation, gunHeldPosition);
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
           equippedGun.OnTriggerRelease();
        }
    }


}
