using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilController : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float rotationSpeed = 6;
    public float returnSpeed = 25;

    [Header("Shooting")]
    public Vector3 RecoilRotation = new Vector3(2f, 2f, 2f);
    public Vector3 HitRotation = new Vector3(2f, 2f, 2f);

    private Vector3 currentRotation;
    private Vector3 Rot;

    private void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
    }

    public void Fire()
    {
        currentRotation += new Vector3(currentRotation.x - RecoilRotation.x, currentRotation.y + Random.Range(-RecoilRotation.y, RecoilRotation.y), currentRotation.z + Random.Range(-RecoilRotation.z, RecoilRotation.z));
    }

    public void Hit()
    {
        currentRotation += new Vector3(-HitRotation.x, Random.Range(-HitRotation.y, HitRotation.y), Random.Range(-HitRotation.z, HitRotation.z));
    }
}
