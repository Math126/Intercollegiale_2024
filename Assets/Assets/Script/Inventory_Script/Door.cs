using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> Doors;
    public GameObject interactUI;

    public string imgName;
    private bool CanInteract = false, isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            CanInteract = true;
            interactUI.SetActive(true);
        }
    }

    public bool GetCanInteract()
    {
        return CanInteract;
    }

    public void Open()
    {
        Doors[0].SetActive(false);
        Doors[1].SetActive(false);
        isOpen = true;
        interactUI.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            interactUI.SetActive(false);
        }
    }
}
