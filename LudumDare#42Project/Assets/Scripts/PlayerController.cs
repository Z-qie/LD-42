using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movementInput)
    {
        rb.MovePosition(rb.position + moveSpeed * movementInput.normalized * Time.deltaTime);
    }

    public void LookAt(Vector3 mousePosition)
    {
        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, mousePosition.z));
    }
}
