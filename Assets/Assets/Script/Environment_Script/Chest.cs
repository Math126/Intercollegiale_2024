using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private bool TryOpenChest = false;

    public void IsInteracted()
    {
        TryOpenChest = true;
    }

    void Update()
    {
        if (TryOpenChest)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -125f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 75f * Time.deltaTime);

            // Vérifie si la rotation a atteint la cible
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                TryOpenChest = false;
                GetComponent<BoxCollider>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<inputManager>().SetCanOpenChest();
            }
        }
    }
}