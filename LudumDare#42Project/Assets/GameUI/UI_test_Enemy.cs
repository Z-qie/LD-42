using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_test_Enemy: MonoBehaviour
{
    public Transform Enemy;
    public Camera refCamera;
    private bool reverFace = false;
    public static UI_test_Enemy Instance;
    Vector3 offset;
    public float moveSmooth = 0f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - Enemy.position;
    }


    // Update is called once per frame
    void Update()
    {
        transform.forward = refCamera.transform.forward;
        transform.rotation = refCamera.transform.rotation;
        Vector3 targetPostion = offset + Enemy .position;
        transform.position = Vector3.Lerp(transform.position, targetPostion, moveSmooth * Time.deltaTime);
        transform.position = targetPostion;
    }
}
