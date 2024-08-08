using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotation : MonoBehaviour
{
    void Update()
    {
        if(transform.rotation.z >= 360)
        {
            transform.Rotate(Vector3.zero);
        }

        transform.Rotate(Vector3.forward, Time.deltaTime * 20);
    }
}
