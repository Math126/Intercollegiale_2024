using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public Transform cam;
    public Transform targetPoint;
    public GameObject objToThrow;

    public float throwForce;
    public float throwUpwardForce;

    public void Throw()
    {
        GameObject projectile = Instantiate(objToThrow, targetPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        projectile.AddComponent<projectile>();
    }
}
