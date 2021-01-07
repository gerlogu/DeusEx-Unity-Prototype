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
    

    public int ammoCapacity = 30;

    public int chargerAmmo;
    public int currentAmmo;

    public GameObject muzzleEffect;
    public Transform muzzleEffectLocation;

    [HideInInspector] public bool canShoot = false;

    private PlayerWeaponry _playerWeaponry;

    [SerializeField] private Transform _position;

    public bool isAutomatic = false;

    public WeaponType weaponType;

    private float _originalTimeFixedDeltaTime;
    private float maxFireRate;
    private Transform _fpsCam;
    private SoundManager _soundManager;
    private AudioSource _audioSource;
    private bool _reloading = false;

    private void Awake()
    {
        
        fireRate = fireRate / 100;
        maxFireRate = fireRate;
    }

    private bool _canReload = true;

    private void Start()
    {
        _originalTimeFixedDeltaTime = Time.fixedDeltaTime;
        _fpsCam = Camera.main.transform;

        if(!_position)
            _position = GameObject.FindGameObjectWithTag("ShootPoint").transform;

        _soundManager = FindObjectOfType<SoundManager>();
        _audioSource = GetComponent<AudioSource>();

        _playerWeaponry = FindObjectOfType<PlayerWeaponry>();
        switch (weaponType)
        {
            case WeaponType.PISTOL:
                chargerAmmo = _playerWeaponry.pistolChargerAmmo;
                currentAmmo = _playerWeaponry.pistolCurrentAmmo;
                break;
            case WeaponType.SUBMACHINE_GUN:
                chargerAmmo = _playerWeaponry.submachineGunChargerAmmo;
                currentAmmo = _playerWeaponry.submachineGunCurrentAmmo;
                break;
            case WeaponType.RIFLE:
                chargerAmmo = _playerWeaponry.rifleChargerAmmo;
                currentAmmo = _playerWeaponry.rifleCurrentAmmo;
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
            _canReload = true;
        }
        else
        {
            _canReload = false;
        }

        switch (weaponType)
        {
            case WeaponType.PISTOL:
                _playerWeaponry.pistolChargerAmmo = chargerAmmo;
                _playerWeaponry.pistolCurrentAmmo = currentAmmo;
                break;
            case WeaponType.SUBMACHINE_GUN:
                _playerWeaponry.submachineGunChargerAmmo = chargerAmmo;
                _playerWeaponry.submachineGunCurrentAmmo = currentAmmo;
                break;
            case WeaponType.RIFLE:
                _playerWeaponry.rifleChargerAmmo = chargerAmmo;
                _playerWeaponry.rifleCurrentAmmo = currentAmmo;
                break;
        }

        _waitedTime += Time.deltaTime;

    }

    public void SetCanShootToTrue()
    {
        canShoot = true;
    }

    private void FixedUpdate()
    {
        fireRate += Time.fixedDeltaTime;
    }

    private float _waitedTime = 0f;

    void Reload()
    {
        if (_canReload && !_reloading && canShoot)
        {
            canShoot = false;
            GetComponentInParent<Animator>().SetTrigger("Reload");
            _canReload = false;
            _reloading = true;
        }
        
    }

    public void ReloadWeapon()
    {
        SetCanShootToTrue();

        if (currentAmmo >= ammoCapacity)
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

        _reloading = false;
    }

    void HandleFire()
    {
        if (fireRate >= maxFireRate && canShoot && chargerAmmo > 0)
        {
            GetComponentInParent<Animator>().SetTrigger("Shoot");
            FindObjectOfType<RecoilController>().Fire();
            Fire();
            fireRate = 0;
            chargerAmmo--;
        }
        else if(chargerAmmo <= 0 && fireRate > maxFireRate)
        {
            Reload();
        }
    }
    
    RaycastHit hit;
    Vector3 dir;
    public float push = 6f;
    public float headPush = 3f;
    public float chestPush = 5f;
    public float forearmPush = 3f;

    void Fire()
    {
        _soundManager.weapons.pistol.PlaySFX(_audioSource, 0);
        if(muzzleEffect && muzzleEffectLocation) 
        {
            GameObject muzzleFlash = Instantiate(muzzleEffect, muzzleEffectLocation.position, muzzleEffectLocation.rotation);
            muzzleFlash.transform.parent = muzzleEffectLocation;
        }
        
        if (Physics.Raycast(_fpsCam.position, _fpsCam.forward, out hit, 1000f, ~layerToIgnore))
        {

            print(hit.transform.gameObject.name);

            #region Parte únicamente visual de la bala
            if (projectile)
            {
                GameObject proj = Instantiate(projectile, _position.position, _position.rotation);

                proj.transform.parent = _position;
                dir = hit.point - _position.position;
                dir.Normalize();
                proj.GetComponent<Rigidbody>().velocity = dir * shootVelocity;
            }
            #endregion

            #region Cálculo de daño y empuje en enemigos humanos
            if (hit.collider.CompareTag("EnemyHitbox") || hit.collider.CompareTag("EnemyHeadHitbox") || hit.collider.CompareTag("EnemyForearmHitbox") || hit.collider.CompareTag("EnemyChestHitbox"))
            {
                Vector3 _pushDirection;
                _pushDirection = (hit.point - _playerWeaponry.transform.position).normalized;
                float _pushStrength = 0;
                float _calculatedDamage = damage;

                switch (hit.collider.tag)
                {
                    case "EnemyHeadHitbox":
                        _pushStrength = headPush * hit.rigidbody.mass / 11.71875f;
                        _calculatedDamage *= 3;
                        break;
                    case "EnemyForearmHitbox":
                        _pushStrength = forearmPush * hit.rigidbody.mass / 11.71875f;
                        break;
                    case "EnemyChestHitbox":
                        _pushStrength = chestPush * hit.rigidbody.mass / 11.71875f;
                        break;
                    default:
                        _pushStrength = push * hit.rigidbody.mass / 11.71875f;
                        break;
                }

                hit.collider.GetComponentInParent<SoldierController>().PushDamage(_calculatedDamage, _pushDirection, _pushStrength, hit.collider.name);
                
            }
            #endregion

            #region Detección de barril explosivo
            if (hit.collider.gameObject.GetComponentInParent<DestructibleExplosion>())
            {
                hit.transform.gameObject.GetComponentInParent<DestructibleExplosion>().Explode();
            }
            #endregion

            SoundEmitter.SpawnSoundSphere(hit.point, 15, 3f);
            SoundEmitter.SpawnSoundCapsule(_position.position, hit.point, 1f);
        }
        else
        {
            #region No se detecta colisión
            if (projectile)
            {
                GameObject proj = Instantiate(projectile, _position.position, _position.rotation);
                proj.transform.parent = _position;
                dir = _position.forward * 1000f - _playerWeaponry.gameObject.transform.position;
                dir.Normalize();
                SoundEmitter.SpawnSoundCapsule(_position.position, _position.forward * 1000f, 1f);
                proj.GetComponent<Rigidbody>().velocity = dir * shootVelocity;
            }
            #endregion
        }

        SoundEmitter.SpawnSoundSphere(transform.position, 14);
    }
    
    
}
