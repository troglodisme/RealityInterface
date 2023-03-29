using UnityEngine;
using UnityEngine.UI;

public class SliderObjectMover : MonoBehaviour
{
    public GameObject objectToMove;
    public Slider slider;

    private void Update()
    {
        float slideValue = slider.value;

        Vector3 newPosition = objectToMove.transform.position;
        newPosition.x = Mathf.Lerp(0f, 0.063f, slideValue);

        objectToMove.transform.position = newPosition;
    }
}



