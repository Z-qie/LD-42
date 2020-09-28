using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    //public LayerMask whatIsEnemy;
    public float destoryTime = 3f;

    private float muzzleSpeed;
    private float damage;

    private void Start()
    {
        Destroy(gameObject, destoryTime);
    }

    private void Update()
    {
        //CheckHit(muzzleSpeed);
        transform.Translate(Vector3.forward * muzzleSpeed * Time.deltaTime);
    }

    public void SetProjectileSpeed(float muzzleSpeed)
    {
        this.muzzleSpeed = muzzleSpeed;
    }
    public void SetProjectileDamage(float damage)
    {
        this.damage = damage;
    }


    //private void CheckHit(float muzzleSpeed)
    //{
    //    if(Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, muzzleSpeed * Time.deltaTime, whatIsEnemy, QueryTriggerInteraction.Collide))
    //    {
    //        print(hit.collider.gameObject.name);
    //        Destroy(gameObject);
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null && other.gameObject.name != "Player")
            damagable.TakeHit(damage, transform.eulerAngles);

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    
}
