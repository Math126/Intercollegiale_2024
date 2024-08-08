using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockerHide : MonoBehaviour
{
    [SerializeField] private bool isHiding = false;
    [SerializeField] private bool isPlayerNear = false;
    [SerializeField] private GameObject interactUI, playerBody;
    [SerializeField] private TextMeshProUGUI interactDescription;
    public GameObject Character;
    public Transform ReSpawn;

    public Camera playerCamera, lockerHideCamera;

    void Start()
    {
        lockerHideCamera.enabled = false;
        playerCamera.enabled = true;
        interactDescription.text = "Se cacher";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHiding && isPlayerNear)
                DesactivateHide();
            else if (isPlayerNear && !isHiding)
                ActivateHide();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHiding)
        {
            interactUI.SetActive(true);
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isHiding)
        {
            interactUI.SetActive(false);
            isPlayerNear = false;
        }
    }

    //Activates the locker`s camera and desactivates the player model and camera
    //Called in Update() when the player presses on E and is near the locker
    void ActivateHide()
    {
        isHiding = true;
        playerCamera.enabled = false;
        lockerHideCamera.enabled = true;
        playerBody.SetActive(false);
        interactDescription.text = "Sortir";
    }

    //Desactivates the locker`s camera and activates the player model and camera
    //Called in Update() when the player presses on E and is hiding
    void DesactivateHide()
    {
        isHiding = false;
        playerCamera.enabled = true;
        lockerHideCamera.enabled = false;
        playerBody.SetActive(true);
        Character.transform.position = ReSpawn.position;
        interactDescription.text = "Se cacher";
    }

}
