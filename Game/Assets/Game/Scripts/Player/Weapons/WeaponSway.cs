using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [SerializeField] private float amount = 0.0125f;
    [SerializeField] private float maxAmountX = 0.075f;
    [SerializeField] private float maxAmountY = 0.04f;
    [SerializeField] private float smoothAmount = 8;

    private Vector3 initialPosition;

    public WeaponFOVFixer weaponFOV;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        if(!weaponFOV)
            weaponFOV = GetComponent<WeaponFOVFixer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        movement.x = Input.GetAxis("Mouse X") * amount;
        movement.y = Input.GetAxis("Mouse Y") * amount;

        movement.x = Mathf.Clamp(movement.x, -maxAmountX, maxAmountX);
        movement.y = Mathf.Clamp(movement.y, -maxAmountY, maxAmountY);

        transform.localPosition = Vector3.Lerp(transform.localPosition, movement + weaponFOV.localPosition, smoothAmount * Time.deltaTime);
    }
}
