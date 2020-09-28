using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI_Movement : MonoBehaviour
{
    public RectTransform UI;
    public float x_fromCenter;
    public float y_fromCenter;
    public float showSpeed;
    public float showDelay;
    public bool isDialogue;
   


    // Start is called before the first frame update
    IEnumerator  Start()
    {
        
        yield return new WaitForSeconds(showDelay);
        UI.DOLocalMove(new Vector3(x_fromCenter, y_fromCenter, 0), showSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
