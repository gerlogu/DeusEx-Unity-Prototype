using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponry : MonoBehaviour
{
    public GameObject weaponParent;
    public GameObject currentWeapon;
    public int currentWeaponIndex = -1;
    private WeaponChanger weaponChanger;
    public List<GameObject> weapons;

    [Header("Pistol")]
    public int pistolChargerAmmo;
    public int pistolCurrentAmmo;
    public int pistolMaxAmmo;
    public int pistolAmmoCapacity = 12;

    [Header("Submachine Gun")]
    public int submachineGunChargerAmmo;
    public int submachineGunCurrentAmmo;
    public int submachineGunMaxAmmo;
    public int submachineGunAmmoCapacity = 25;

    [Header("Rifle")]
    public int rifleChargerAmmo;
    public int rifleCurrentAmmo;
    public int rifleMaxAmmo;
    public int rifleAmmoCapacity = 30;

    public bool hasWeapon = true;

    // Start is called before the first frame update
    void Start()
    {

        if (!currentWeapon && GameObject.FindGameObjectWithTag("Weapon"))
        {
            currentWeapon = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponentInChildren<Gun>().gameObject;

            weaponChanger = currentWeapon.GetComponent<WeaponChanger>();

            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].GetComponent<Gun>().weaponType == currentWeapon.GetComponent<Gun>().weaponType)
                {
                    currentWeaponIndex = i;
                }
            }
        }
        else
        {
            hasWeapon = false;
            currentWeaponIndex = -1;
        }
        
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count >= 1 /*&& hasWeapon*/)
        {
            SelectWeapon(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count >= 2 /*&& hasWeapon*/)
        {
            SelectWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count >= 3/*&& hasWeapon*/)
        {
            SelectWeapon(2);
        }
    }

    void SelectWeapon(int index)
    {
        if(index != currentWeaponIndex && currentWeapon)
        {
            currentWeapon.GetComponent<Gun>().canShoot = false;
            currentWeapon.GetComponent<Animator>().SetTrigger("HideWeapon");
            weaponChanger.nextWeaponIndex = index;
        }
        else if(index != currentWeaponIndex && !currentWeapon)
        {
            SwapWeapon(index);
        }
    }

    public void SwapWeapon(int index)
    {

        if (!hasWeapon)
        {
            hasWeapon = true;
            GameObject newObj = GameObject.Instantiate(weapons[index]) as GameObject;

            currentWeaponIndex = index;
            // currentWeapon = newWeapon;

            GameObject parent = GameObject.FindGameObjectWithTag("WeaponHolder");

            newObj.transform.parent = parent.transform;

            newObj.transform.localPosition = new Vector3(0,0,0);
            newObj.transform.localRotation = Quaternion.Euler(0,0,0);
            newObj.transform.localScale = new Vector3(0.8000001f, 0.8000001f, 0.8000001f);
            //newObj.transform.localPosition = currentWeapon.transform.localPosition;

            currentWeapon = newObj;
            weaponChanger = currentWeapon.GetComponent<WeaponChanger>();
        }
        else
        {
            Vector3 weaponPosition = currentWeapon.transform.position;
            Quaternion weaponRotation = currentWeapon.transform.rotation;
            Vector3 localPosition = currentWeapon.transform.localPosition;
            // GameObject newWeapon = Instantiate(weapons[index], Vector3.zero, Quaternion.identity);
            GameObject newObj = GameObject.Instantiate(weapons[index]) as GameObject;

            currentWeaponIndex = index;
            // currentWeapon = newWeapon;

            GameObject parent = GameObject.FindGameObjectWithTag("WeaponHolder");

            newObj.transform.parent = currentWeapon.transform.parent;
            // currentWeapon.transform.position = weaponPosition;
            // currentWeapon.transform.localPosition = localPosition;

            newObj.transform.position = currentWeapon.transform.position;
            newObj.transform.rotation = currentWeapon.transform.rotation;
            newObj.transform.localScale = currentWeapon.transform.localScale;
            newObj.transform.localPosition = currentWeapon.transform.localPosition;

            Destroy(currentWeapon);

            currentWeapon = newObj;
            weaponChanger = currentWeapon.GetComponent<WeaponChanger>();
        }

        
    }
}
