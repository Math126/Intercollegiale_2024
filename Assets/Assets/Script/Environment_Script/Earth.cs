using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    void Update()
    {
        if(transform.rotation.y >= 360)
        {
            transform.Rotate(Vector3.zero);
        }

        transform.Rotate(Vector3.up, Time.deltaTime * 10);
    }
}
