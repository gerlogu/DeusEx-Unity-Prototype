using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum WeaponType
{
    PISTOL,
    SUBMACHINE_GUN,
    RIFLE
}
public class Gun : MonoBehaviour
{
    public float accurate;
    public float shakeDuration;
    public float shakeAmplitude;
    public float shakeFrequency;
    public GameObject projectile;
    public LayerMask layerToIgnore;
    public float shootVelocity = 50f;
    public int damage = 10;
    public float fireRate = 100f;
    private float maxFireRate;

    private GameObject fpsCam;
    private SoundManager soundManager;
    private AudioSource audioSource;
    public int ammoCapacity = 30;

    public int chargerAmmo;
    public int currentAmmo;

    public GameObject muzzleEffect;
    public Transform muzzleEffectLocation;

    [HideInInspector] public bool canShoot = false;

    PlayerWeaponry playerWeaponry;

    [SerializeField] Transform position;

    public bool isAutomatic = false;

    public WeaponType weaponType;

    float originalTimeFixedDeltaTime;

    

    private void Awake()
    {
        
        fireRate = fireRate / 100;
        maxFireRate = fireRate;
    }

    bool canReload = true;

    private void Start()
    {
        originalTimeFixedDeltaTime = Time.fixedDeltaTime;
        fpsCam = GameObject.FindGameObjectWithTag("MainCamera");
        //position = GameObject.FindGameObjectWithTag("ShootPoint").transform;
        soundManager = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();

        playerWeaponry = FindObjectOfType<PlayerWeaponry>();
        switch (weaponType)
        {
            case WeaponType.PISTOL:
                chargerAmmo = playerWeaponry.pistolChargerAmmo;
                currentAmmo = playerWeaponry.pistolCurrentAmmo;
                break;
            case WeaponType.SUBMACHINE_GUN:
                chargerAmmo = playerWeaponry.submachineGunChargerAmmo;
                currentAmmo = playerWeaponry.submachineGunCurrentAmmo;
                break;
            case WeaponType.RIFLE:
                chargerAmmo = playerWeaponry.rifleChargerAmmo;
                currentAmmo = playerWeaponry.rifleCurrentAmmo;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!isAutomatic)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleFire();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                HandleFire();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if(chargerAmmo < ammoCapacity && currentAmmo > 0)
        {
            canReload = true;
        }
        else
        {
            canReload = false;
        }

        switch (weaponType)
        {
            case WeaponType.PISTOL:
                playerWeaponry.pistolChargerAmmo = chargerAmmo;
                playerWeaponry.pistolCurrentAmmo = currentAmmo;
                break;
            case WeaponType.SUBMACHINE_GUN:
                playerWeaponry.submachineGunChargerAmmo = chargerAmmo;
                playerWeaponry.submachineGunCurrentAmmo = currentAmmo;
                break;
            case WeaponType.RIFLE:
                playerWeaponry.rifleChargerAmmo = chargerAmmo;
                playerWeaponry.rifleCurrentAmmo = currentAmmo;
                break;
        }

        
        waitedTime += Time.deltaTime;
        
        
    }

    

    private void FixedUpdate()
    {
        fireRate += Time.fixedDeltaTime;
    }

    private bool fireRateCompleted = true;
    float waitedTime = 0f;
    void SetFireRate()
    {
        fireRateCompleted = true;
    }

    void Reload()
    {
        if (canReload && !reloading && canShoot)
        {
            canShoot = false;
            GetComponentInParent<Animator>().SetTrigger("Reload");
            canReload = false;
            reloading = true;
        }
        
    }

    bool reloading = false;
    // int chargerMaxAmmo = 0;

    void HandleFire()
    {
        if (fireRate >= maxFireRate /*fireRateCompleted */&& canShoot && chargerAmmo > 0)
        {
            GetComponentInParent<Animator>().SetTrigger("Shoot");
            FindObjectOfType<RecoilController>().Fire();
            Fire();
            fireRate = 0;
            
            //fireRateCompleted = false;
            Invoke("SetFireRate", maxFireRate);
            chargerAmmo--;
        }
        else if(chargerAmmo <= 0 && fireRate > maxFireRate /*&& fireRateCompleted*/)
        {
            Reload();
        }
    }

    public void SetCanShootToTrue()
    {
        this.canShoot = true;
    }

    

    public void ReloadWeapon()
    {
        SetCanShootToTrue();

        if(currentAmmo >= ammoCapacity)
        {
            currentAmmo -= (ammoCapacity - chargerAmmo);
            chargerAmmo = ammoCapacity;
        }
        else
        {
            int aux = chargerAmmo;
            chargerAmmo = Mathf.Clamp(chargerAmmo + currentAmmo, 1, 30);
            currentAmmo -= chargerAmmo - aux;
            
        }

        reloading = false;
    }

    RaycastHit hit;
    Vector3 dir;
    public float push = 6f;
    public float headPush = 3f;
    public float chestPush = 5f;
    public float forearmPush = 3f;

    void Fire()
    {
        soundManager.weapons.pistol.PlaySFX(audioSource, 0);
        if(muzzleEffect && muzzleEffectLocation) 
        {
            GameObject muzzleFlash = Instantiate(muzzleEffect, muzzleEffectLocation.position, muzzleEffectLocation.rotation);
            muzzleFlash.transform.parent = muzzleEffectLocation;
        }
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 1000f, ~layerToIgnore))
        {
            if (projectile)
            {
                GameObject proj = Instantiate(projectile, position.position, position.rotation);

                proj.transform.parent = position;
                dir = hit.point - position.position;
                dir.Normalize();
                proj.GetComponent<Rigidbody>().velocity = dir * shootVelocity;
            }

            print(hit.collider.gameObject.tag);

            if (hit.collider.CompareTag("EnemyHitbox") || hit.collider.CompareTag("EnemyHeadHitbox") || hit.collider.CompareTag("EnemyForearmHitbox") || hit.collider.CompareTag("EnemyChestHitbox"))
            {
                Vector3 deathPush;
                deathPush = (hit.point - playerWeaponry.gameObject.transform.position).normalized;
                float pushStrength = 0;
                if (hit.collider.CompareTag("EnemyHeadHitbox"))
                {
                    pushStrength = headPush * hit.rigidbody.mass / 11.71875f;
                    hit.collider.GetComponentInParent<Enemy>().Damage(damage * 3);
                }
                else if (hit.collider.CompareTag("EnemyForearmHitbox"))
                {
                    pushStrength = forearmPush * hit.rigidbody.mass / 11.71875f;
                    hit.collider.GetComponentInParent<Enemy>().Damage(damage);
                }
                else if (hit.collider.CompareTag("EnemyChestHitbox"))
                {
                    pushStrength = chestPush * hit.rigidbody.mass / 11.71875f;
                    hit.collider.GetComponentInParent<Enemy>().Damage(damage);
                }
                else
                {
                    pushStrength = push * hit.rigidbody.mass / 11.71875f;
                    hit.collider.GetComponentInParent<Enemy>().Damage(damage);
                }


                hit.collider.GetComponentInParent<Enemy>().Push(deathPush, pushStrength, hit.collider.name);
                
            }

            if (hit.collider.gameObject.GetComponentInParent<DestructibleExplosion>())
            {
                hit.transform.gameObject.GetComponentInParent<DestructibleExplosion>().Explode();
            }
        }
        else
        {
            if (projectile)
            {
                GameObject proj = Instantiate(projectile, position.position, position.rotation);

                proj.transform.parent = position;
                dir = fpsCam.transform.forward * 1000f - playerWeaponry.gameObject.transform.position;
                dir.Normalize();
                proj.GetComponent<Rigidbody>().velocity = dir * shootVelocity;
            }
        }
        

    }
    
    
}
