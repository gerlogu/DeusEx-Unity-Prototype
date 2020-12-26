using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon headbob class.
/// </summary>
public class Headbob : MonoBehaviour
{
    [HideInInspector] public float p_z = 1f;
    [HideInInspector] public GameObject weaponHolder;
    [HideInInspector] public Vector3 weaponHolderOriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder");
        weaponHolderOriginalPosition = weaponHolder.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Weapon breath animation.
    /// </summary>
    /// <param name="p_x_intensity">x intensity</param>
    /// <param name="p_y_intensity">y intensity</param>
    /// <param name="p_z_Multiplier">z position multiplier per frame</param>
    public void HeadbobMovement(float p_x_intensity, float p_y_intensity, float p_z_Multiplier)
    {
        if (weaponHolder)
            weaponHolder.transform.localPosition = new Vector3(weaponHolderOriginalPosition.x, weaponHolderOriginalPosition.y, weaponHolder.transform.localPosition.z) + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
        p_z += Time.deltaTime * p_z_Multiplier;
    }
}
