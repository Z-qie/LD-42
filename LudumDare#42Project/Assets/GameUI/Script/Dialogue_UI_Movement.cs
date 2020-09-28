using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dialogue_UI_Movement : MonoBehaviour
{
    public RectTransform UI;
    public float x_fromCenter;
    public float y_fromCenter;
    public float showSpeed;
    public bool isShow=false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& isShow==true)
        {
            UI.DOLocalMove(new Vector3(2000, y_fromCenter, 0), showSpeed);
            isShow = false;

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag== "Lines" && isShow ==false)
        {
            UI.DOLocalMove(new Vector3(x_fromCenter, y_fromCenter, 0), showSpeed);
            isShow = true;

        }
    }
}

