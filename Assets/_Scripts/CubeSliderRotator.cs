using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeSliderRotator : MonoBehaviour
{
    public float minRotationSpeed = 0f;
    public float maxRotationSpeed = 100f;
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = slider.value * (maxRotationSpeed - minRotationSpeed) + minRotationSpeed;
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
