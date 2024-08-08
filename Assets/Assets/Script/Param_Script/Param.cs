using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Param : MonoBehaviour
{
    public Canvas InventoryCanvas, ParamCanvas;
    public GameObject Camera;

    public Slider VolumeSlider, CameraSlider, LumSlider;
    public TextMeshProUGUI ZoomVal, VolumeVal, LumVal;

    private void Update()
    {
        if (GetComponent<inputManager>().GetParamIsClosed())
        {
            ParamCanvas.gameObject.SetActive(true);
            InventoryCanvas.gameObject.SetActive(false);
            GetComponent<ThirdPersonController>().enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ParamCanvas.gameObject.SetActive(false);
            InventoryCanvas.gameObject.SetActive(true);
            GetComponent<ThirdPersonController>().enabled = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseBtn()
    {
        GetComponent<inputManager>().SetParamIsClosed();
        ParamCanvas.gameObject.SetActive(false);
        InventoryCanvas.gameObject.SetActive(true);
        GetComponent<ThirdPersonController>().enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitBtn()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void SetCameraZoom()
    {
        Camera.GetComponent<Camera>().sensorSize = new Vector2(CameraSlider.value, 24);
        ZoomVal.text = CameraSlider.value.ToString();
    }

    public void SetLuminausite()
    {
        RenderSettings.ambientIntensity = LumSlider.value;
        LumVal.text = LumSlider.value.ToString("0.0");
    }

    public void SetVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        VolumeVal.text = VolumeSlider.value.ToString("0.0");
    }
}