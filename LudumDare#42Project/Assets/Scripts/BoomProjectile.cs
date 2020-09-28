using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomProjectile : MonoBehaviour
{

    //public LayerMask whatIsEnemy;
    public BoomEffector boomEffector;
    public float destoryTime;

    private float damage;

    private void Start()
    {
        destoryTime = boomEffector.zoomTime + boomEffector.explodeTime + boomEffector.coolTime;
        Destroy(gameObject, destoryTime);
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
    }


}
