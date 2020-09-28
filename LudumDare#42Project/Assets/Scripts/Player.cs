using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity
{
    public Vector3 camOffset;

    private Camera cam;
    private PlayerController playerController;
    private GunController gunController;
    private EnergyManager energyManager;

    private Vector3 movementInput;
    private Vector3 cursorPosition;

    public float initialPowerAmount = 1000f;
    public float powerRemaining;

    protected override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        energyManager = GetComponent<EnergyManager>();

        cam = Camera.main;
        powerRemaining = initialPowerAmount;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = transform.position + camOffset;
        // get user input
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        CheckCursorPosition();



        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gunController.equippedGun.gunMode == Gun.GunMode.Laser)
                gunController.equippedGun.gunMode = Gun.GunMode.Boom;
            else
                gunController.equippedGun.gunMode = Gun.GunMode.Laser;
        }

        // get shooting input
        // Weapon input
        if (energyManager.energyEquipped > gunController.equippedGun.energyConsumedPerShoot)
        { 
            if (Input.GetMouseButton(0))
            {
                gunController.OnTriggerHold();
            }
            if (Input.GetMouseButtonUp(0))
            {
                 gunController.OnTriggerRelease();
            }
        }
    }

    private void FixedUpdate()
    {
        // make player move
        playerController.Move(movementInput);
        // make player look at mouse
        playerController.LookAt(cursorPosition);
    }


    private void CheckCursorPosition()
    {
        // get user cursor position, only start from the near plane of camera to the position's (x,z) pixel, y is ignored.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // we use this because we will have obstacles later
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // we dont use raycasthit because we only want this plane to catch the ray.

        if (groundPlane.Raycast(ray, out float distanceFromCam))
            cursorPosition = ray.GetPoint(distanceFromCam);

        Debug.DrawLine(ray.origin, cursorPosition, Color.red);
    }



    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Charger"))
        {
            other.gameObject.GetComponent<ChargerController>().ControllChager();
        }
    }
}
