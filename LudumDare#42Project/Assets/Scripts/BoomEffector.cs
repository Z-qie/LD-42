using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffector : MonoBehaviour
{
    public Gun gun;
    public Light lightFlash;
    public Transform muzzle;
    public Vector3 targetPosition;
    public GameObject effectHolder;
    public LineRenderer lineRenderer;
    public LayerMask obstacleMask;
    public LayerMask enemyMask;
    public ParticleSystem boomParticle;

    public float lockingDelay = 1f;
    public float zoomTime;
    public float explodeTime;
    public float coolTime;




    public float minZoomWidth = 0.1f;
    public float maxZoomWidth = 0.5f;
    public float maxDistance = 20f;

    ParticleSystem.Particle[] particles;

    void Start()
    {
        Deactivate();
         particles = new ParticleSystem.Particle[boomParticle.main.maxParticles];

    }


    public void Activate()
    {
        effectHolder.SetActive(true);
        StartCoroutine(Boom());
    }

    IEnumerator Boom()
    {
        // get position
        if (Physics.Raycast(muzzle.position, muzzle.forward,  out RaycastHit hit, maxDistance, obstacleMask) || Physics.Raycast(muzzle.position, muzzle.forward, out hit, maxDistance, enemyMask))
        {
            //if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Enemy"))
            //{
                lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance / 2));
                lineRenderer.SetPosition(2, new Vector3(0, 0, hit.distance));
                // will generate locking effect here
                targetPosition = hit.point;
                lightFlash.transform.position = hit.point;
        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, maxDistance / 2));
            lineRenderer.SetPosition(2, new Vector3(0, 0, maxDistance));
            // will generate locking effect here
            targetPosition = muzzle.position + muzzle.forward * maxDistance;
            lightFlash.transform.position = muzzle.position + muzzle.forward * maxDistance;
        }

        float timeRemain = 0f;
        lineRenderer.startWidth = 0f;

        // targeting effect
        while (timeRemain < lockingDelay)
        {
            timeRemain += Time.deltaTime;

            lineRenderer.widthMultiplier = Mathf.Lerp(maxZoomWidth, minZoomWidth, Mathf.PingPong(timeRemain * 100f, 1));
            yield return null;
        }


        //yield return new WaitForSeconds(gun.boomDelay - lockingDelay);
        //boomParticle.gameObject.SetActive(true);


        //// boom effect - zoom
        //timeRemain = 0f;
        //while (timeRemain < zoomTime)
        //{
        //    timeRemain += Time.deltaTime;

  
        //    lineRenderer.widthMultiplier = Mathf.Lerp(maxZoomWidth, minZoomWidth, Mathf.PingPong(timeRemain * 100f, 1));
        //    yield return null;
        //}


        Deactivate();
    }



    public void Deactivate()
    {
        lineRenderer.widthMultiplier = 1f;
        effectHolder.SetActive(false);
    }
}


