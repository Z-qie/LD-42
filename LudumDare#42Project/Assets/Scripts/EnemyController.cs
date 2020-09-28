using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : LivingEntity
{
    public ParticleSystem enemyDeathEffect;
    public float reNavRate = 0.25f;

    private enum EnemyState { IDLE, CHASING, ATTACKING };
    private EnemyState currentState;

    private Transform target;
    private LivingEntity targetEntity;

    private bool targetAlive;

    private float attackRate = 1f;
    private float nextAttackTime;
    private float attackDistanceThreshold = 0.5f;
    private float attackSpeed = 3;
    private float attackDamage;
    private float myCollisionRadius;
    private float targetCollisionRadius;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            targetAlive = true;
            //get ref
            targetEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    protected override void Start()
    {
        base.Start();
        
        //register
        currentState = EnemyState.CHASING;
        StartCoroutine(FollowTarget());
    }

    public override void TakeHit(float damage, Vector3 hitDirection)
    {
        if (damage >= currentHealth)
        {
            Destroy(Instantiate(enemyDeathEffect, transform.position, Quaternion.Euler(hitDirection.x, hitDirection.y, hitDirection.z)), enemyDeathEffect.startLifetime);
        }

        base.TakeHit(damage, hitDirection);
    }

    public void SetCharacristics(float startingHealth, float moveSpeed, int hitsToKillPLayer)
    {
        this.startingHealth = startingHealth;
        GetComponent<NavMeshAgent>().speed = moveSpeed;

        if (targetAlive)
            attackDamage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPLayer);
    }

    private void Update()
    {
        if (targetAlive &&
            Time.time > nextAttackTime &&
            (target.position - transform.position).sqrMagnitude < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
        {
            nextAttackTime = Time.time + attackRate;
            StartCoroutine(Attack());
        }
    }

    private void OnTargetDeath()
    {
        targetAlive = false;
        currentState = EnemyState.IDLE;
    }

    IEnumerator Attack()
    {
        float percent = 0;
        bool damageApplied = false;

        // set states
        currentState = EnemyState.ATTACKING;
        GetComponent<NavMeshAgent>().enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);


        while (percent <= 1)
        {
            // apply damage
            if (percent > 0.5 && damageApplied == false)
            {
                damageApplied = true;
                targetEntity.TakeDamage(attackDamage);
            }

            // lunge
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        // set states back
        currentState = EnemyState.CHASING;
        GetComponent<NavMeshAgent>().enabled = true;
    }


    IEnumerator FollowTarget()
    {
        while (targetAlive)
        {
            if (currentState == EnemyState.CHASING)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);

                GetComponent<NavMeshAgent>().SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(reNavRate);
        }

    }
}
