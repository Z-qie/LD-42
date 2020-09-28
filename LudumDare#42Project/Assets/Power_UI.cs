using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Power_UI : MonoBehaviour
{
    public Player player;
    public Image powerBar;

    private void Update()
    {
        GetPower();
    }
    private void GetPower()
   {
        powerBar.fillAmount = player.powerRemaining / player.initialPowerAmount;
        
    }
}
   
