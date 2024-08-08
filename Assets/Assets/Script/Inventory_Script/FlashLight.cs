using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private float lightDrainSpeed = 10;
    public float flashlightBatteryMax;
    public float batteryHpAdd = 50;
    public float intensity = 3;
    private float flashlightBattery;
    public GameObject flashlight;
    private bool flashlightOn = false;
    private Light flashlightLight;
    // Start is called before the first frame update
    void Start()
    {
        flashlightLight = flashlight.GetComponent<Light>();
        flashlightBattery = flashlightBatteryMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlightOn)
        {
            if (flashlightBattery > 0)
            {
                flashlightBattery -= lightDrainSpeed * Time.deltaTime;
                float light = flashlightBattery / flashlightBatteryMax * 100;
                float flickerRate = flashlightBatteryMax / 1000f;

                flashlightLight.intensity = Mathf.Max((Mathf.Sqrt(light / 100f) + (Mathf.Sin(light * 100f * flickerRate) / 10f) + (Mathf.Sin(light * 100f * (flickerRate / 2.6f)) / 10f)) * intensity, 0);
                flashlight.transform.position = Camera.main.transform.position;
                flashlight.transform.rotation = Camera.main.transform.rotation;
            }
            else
            {
                flashlightLight.intensity = 0;
            }
        }
        else
        {
            flashlightLight.intensity = 0;
        }
    }

    public void RechargeBattery()
    {
        flashlightBattery = batteryHpAdd;
    }

    public void SetFlashLightOn(bool value)
    {
        flashlightOn = value;
    }
}