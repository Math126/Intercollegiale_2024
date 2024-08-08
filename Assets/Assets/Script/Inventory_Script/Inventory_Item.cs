using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Item : MonoBehaviour
{
    public Sprite imgObj;
    public GameObject canvas;
    public bool CanStack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canvas.SetActive(false);
    }
}