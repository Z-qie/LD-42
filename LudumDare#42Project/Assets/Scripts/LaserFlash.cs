using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFlash : MonoBehaviour
{
    public Gun gun;
    public Transform muzzle;
    public GameObject flashHolder;
    public LineRenderer lineRenderer;
    public LayerMask obstacleMask;
    public LayerMask enemyMask;
    public Light lightFlash;
    public EnergyManager energyManager;


    public float maxDistance = 100f;
    public float minZoomWidth = 0.5f;
    public float maxZoomWidth = 30f;
    public float laserDelay = 1f;
    public float intensity = 1.6f;

    public bool isLaserLaunching;

    Gradient originalGredient;

    void Start()
    {
        Deactivate();
        originalGredient = lineRenderer.colorGradient;
        energyManager = FindObjectOfType<EnergyManager>();
    }


    private void Update()
    {

        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, obstacleMask))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance / 2));
                lineRenderer.SetPosition(2, new Vector3(0, 0, hit.distance));
                lightFlash.transform.position = hit.point;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, maxDistance / 2));
            lineRenderer.SetPosition(2, new Vector3(0, 0, maxDistance));
        }

        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, enemyMask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // will create damage effect here
            }
        }
    }

    public void Activate()
    {
        flashHolder.SetActive(true);
        StartCoroutine(ZoomUp());
    }

    IEnumerator ZoomUp()
    {
        isLaserLaunching = false;
        // charging
        float remainFlashTime = 0f;
        lineRenderer.startWidth = 0f;
        while (remainFlashTime < laserDelay)
        {
            lightFlash.intensity = 0;
            remainFlashTime += Time.deltaTime;

            lineRenderer.widthMultiplier = Mathf.Lerp(300f, minZoomWidth, remainFlashTime / laserDelay);
            Gradient gradient = new Gradient();

            float alpha = Mathf.Lerp(0f, 0.1f, remainFlashTime / laserDelay);
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;

            yield return null;
        }

        // launch
        remainFlashTime = 0f;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.colorGradient = originalGredient;

        while (remainFlashTime < laserDelay && energyManager.energyEquipped > gun.energyPerLaser)
        {
            // set real projectile can be generated
            isLaserLaunching = true;
            // while pressed, keep flash
            if (Input.GetMouseButton(0))
            {
                remainFlashTime = 0f;
            }
            remainFlashTime += Time.deltaTime;

            lightFlash.intensity = Mathf.Lerp(0f, intensity, Mathf.PingPong(Time.time * maxZoomWidth, 1));
            lineRenderer.widthMultiplier = Mathf.Lerp(minZoomWidth, maxZoomWidth, Mathf.PingPong(Time.time * maxZoomWidth, 1));
            yield return null;
        }


        // Cool down
        remainFlashTime = 0f;
        isLaserLaunching = false;
        while (remainFlashTime < laserDelay)
        {
            lightFlash.intensity = 0;
            remainFlashTime += Time.deltaTime;

            lineRenderer.widthMultiplier = Mathf.Lerp(maxZoomWidth, minZoomWidth, remainFlashTime / laserDelay);
            yield return null;
        }
        // set another round of shot true
        gun.canShot = true;
        Deactivate();
    }


    public void Deactivate()
    {
        lineRenderer.widthMultiplier = 1f;
        flashHolder.SetActive(false);
    }
}
