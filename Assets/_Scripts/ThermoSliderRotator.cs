using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermoSliderRotator : MonoBehaviour
{
    public GameObject objectToRotate;
    public Slider rotationSlider;
    public float minAngle = 0f;
    public float maxAngle = 90f;


    public Text temperatureText;
    
    private const int MIN_TEMP = 0;
    private const int MAX_TEMP = 30;

    void Start() 
    {
        temperatureText.text = $"{MIN_TEMP}°C";

    }


    // Update is called once per frame
    void Update()
    {
        float rotationValue = rotationSlider.value;

        float outputMin = 45f; // The minimum value of the output range
        float outputMax = -35f; // The maximum value of the output range

        float mappedValue = (rotationValue - 0f) * ((outputMax - outputMin) / (1f - 0f)) + outputMin;

        objectToRotate.transform.localRotation = Quaternion.Euler(0f, 0f, mappedValue);
        // Debug.Log("Mapped value: " + mappedValue); // Outputs "Mapped value: 90"

        //map temp
        int temperature = Mathf.RoundToInt(rotationSlider.value * (MAX_TEMP - MIN_TEMP)) + MIN_TEMP;
        temperatureText.text = $"{temperature}°C";


    }


}





