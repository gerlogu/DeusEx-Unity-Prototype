using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class AmmoUIController : MonoBehaviour
{
    [Tooltip("")]
    [SerializeField] TextMeshPro chargerAmmo;
    [Tooltip("")]
    [SerializeField] TextMeshPro currentAmmo;

    private Gun gun; // Reference to gun

    // Start is called before the first frame update
    void Start()
    {
        gun = GetComponentInParent<Gun>();
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        chargerAmmo.text = gun.chargerAmmo < 10 ? "0" + gun.chargerAmmo.ToString() : gun.chargerAmmo.ToString();
        currentAmmo.text = gun.currentAmmo < 10 ? "0" + gun.currentAmmo.ToString() : gun.currentAmmo.ToString();
    }
}
