using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [HideInInspector] public int nextWeaponIndex;

    public void SwapWeapon()
    {
        FindObjectOfType<PlayerWeaponry>().SwapWeapon(nextWeaponIndex);
    }
}
