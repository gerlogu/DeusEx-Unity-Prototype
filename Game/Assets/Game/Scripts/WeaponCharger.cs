using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum Weapon
{
    PISTOL,
    SUBMACHINE_GUN,
    RIFLE
}

public class WeaponCharger : MonoBehaviour
{
    public Weapon weapon;
    public GameObject skin;
    public LayerMask whatIsPlayer;
    private bool obtained = false;

    private Transform player;
    private PlayerWeaponry playerWeaponry;
    private float obtainWeaponSmooth = 11;
    private float radius = 4f;
    private bool playerInOutlineRange = false;
    private bool playerInRange = false;
    private bool playerInGetRange = false;
    private bool recharged = false;
    private Vector3 obtainedRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerWeaponry = player.GetComponent<PlayerWeaponry>();
        obtainedRot = transform.localRotation.eulerAngles + new Vector3(0,110,0);
    }
    RaycastHit hit;
    RaycastHit hit2;
    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerWeaponry = player.GetComponent<PlayerWeaponry>();
        }
        playerInOutlineRange = Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit2, radius + 4);
        playerInRange = Physics.Raycast(transform.position, (player.position-transform.position).normalized, out hit, radius);
        playerInGetRange = Physics.Raycast(transform.position, (player.position - transform.position).normalized, 0.8f);

        if (playerInRange)
        {
            if(hit.transform.gameObject.layer == 10)
            {
                switch (weapon)
                {
                    case Weapon.SUBMACHINE_GUN:
                        if (playerWeaponry.submachineGunCurrentAmmo < playerWeaponry.submachineGunMaxAmmo + (playerWeaponry.submachineGunAmmoCapacity - playerWeaponry.submachineGunChargerAmmo))
                        {
                            if (player.GetComponentInChildren<Gun>())
                            {
                                if (player.GetComponentInChildren<Gun>().weaponType == WeaponType.SUBMACHINE_GUN)
                                {
                                    if(!recharged)
                                        player.GetComponentInChildren<Gun>().currentAmmo = Mathf.Clamp(player.GetComponentInChildren<Gun>().currentAmmo + 15, 0, playerWeaponry.submachineGunMaxAmmo + (player.GetComponentInChildren<Gun>().ammoCapacity - playerWeaponry.submachineGunChargerAmmo));
                                    Debug.Log((player.GetComponentInChildren<Gun>().ammoCapacity - playerWeaponry.submachineGunCurrentAmmo));
                                    recharged = true;
                                    obtained = true;
                                }
                                else
                                {
                                    if(!recharged)
                                        playerWeaponry.submachineGunCurrentAmmo = Mathf.Clamp(playerWeaponry.submachineGunCurrentAmmo + 15, 0, playerWeaponry.submachineGunMaxAmmo + (playerWeaponry.submachineGunAmmoCapacity - playerWeaponry.submachineGunChargerAmmo));
                                    recharged = true;
                                    obtained = true;
                                }
                            }
                            else
                            {
                                if(!recharged)
                                    playerWeaponry.submachineGunCurrentAmmo = Mathf.Clamp(playerWeaponry.submachineGunCurrentAmmo + 15, 0, playerWeaponry.submachineGunMaxAmmo + (playerWeaponry.submachineGunAmmoCapacity - playerWeaponry.submachineGunChargerAmmo));
                                obtained = true;
                                recharged = true;
                            }
                        }
                        else
                        {

                        }
                        break;
                }
                
            }
        }
        

        if (obtained)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, obtainWeaponSmooth * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(obtainedRot), obtainWeaponSmooth * Time.deltaTime);

            if (playerInGetRange)
            {
                switch (weapon)
                {
                    case Weapon.SUBMACHINE_GUN:
                        if (playerWeaponry.submachineGunCurrentAmmo < playerWeaponry.submachineGunMaxAmmo + (playerWeaponry.submachineGunAmmoCapacity - playerWeaponry.submachineGunChargerAmmo))
                        {

                            
                        }
                        else
                        {
                            obtained = false;
                        }
                        break;
                }
                Destroy(gameObject);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && playerInRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
