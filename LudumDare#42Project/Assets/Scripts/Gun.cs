using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunMode { Laser, Boom };
    public GunMode gunMode;

    public Transform muzzle;
    public EnergyManager energyManager;
    public Sprite[] flashSprites;
    public SpriteRenderer[] spriteRenderers;

    public float energyConsumedPerShoot;

    [Header("Laser Gun Setting")]
    public LaserFlash laserFlash;
    public LaserProjectile laserprojectile;

    public float msBtwShootsLaser = 40f;
    public float muzzleSpeedLaser = 100f;
    public float damageLaser = 10;
    public float energyPerLaser = 1f;
    public bool canShot = true;

    [Header("Boom Setting")]
    public BoomEffector boomEffector;
    public BoomProjectile boomProjectile;

    public float msBtwShootsBoom;
    public float muzzleSpeedLa;
    public float damageBoom = 200f;
    public float energyPerBoom = 10f;
    public float boomDelay = 1f;
    public bool canTriggerNextBoom = true;

    private float nextShootingTime;

    private void Start()
    {
        energyManager = FindObjectOfType<EnergyManager>();
    }

    public void Shoot()
    {
        // laser gun flash
        if (gunMode == GunMode.Laser && canShot)
        {
            canShot = false;
            // activate shooting effect
            laserFlash.Activate();
        }

        // laser gun shoot
        if (Time.time > nextShootingTime && gunMode == GunMode.Laser && laserFlash.isLaserLaunching)
        {
            nextShootingTime = Time.time + msBtwShootsLaser / 1000;
            energyConsumedPerShoot = energyPerLaser;

            // activate muzzle effect
            int flashSpriteIndex = Random.Range(0, flashSprites.Length);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
            }

            // generate projectile
            Invoke(nameof(GenerateLaserProjectile), laserFlash.laserDelay);
        }


        // boom 
        if (Time.time > nextShootingTime && gunMode == GunMode.Boom && canTriggerNextBoom)
        {
            nextShootingTime = Time.time + msBtwShootsBoom / 1000;
            energyConsumedPerShoot = energyPerBoom;

            // activate shooting effect
            boomEffector.Activate();
            // generate projectile
            Invoke(nameof(GenerateBoomProjectile), boomDelay);

        }



    }

    void GenerateLaserProjectile()
    {
        LaserProjectile newLaserProjectile = Instantiate<LaserProjectile>(laserprojectile, muzzle.position, muzzle.rotation);
        newLaserProjectile.SetProjectileSpeed(muzzleSpeedLaser);
        newLaserProjectile.SetProjectileDamage(damageLaser);

        energyManager.ConsumeEnergy(energyConsumedPerShoot);
    }

    void GenerateBoomProjectile()
    {
        BoomProjectile newBoomProjectile = Instantiate<BoomProjectile>(boomProjectile, boomEffector.targetPosition, Quaternion.identity);
        newBoomProjectile.SetProjectileDamage(damageLaser);
        energyManager.ConsumeEnergy(energyConsumedPerShoot);
    }



    public void OnTriggerHold()
    {
        Shoot();
        canTriggerNextBoom = false;
    }

    public void OnTriggerRelease()
    {
        canTriggerNextBoom = true;
    }
}
