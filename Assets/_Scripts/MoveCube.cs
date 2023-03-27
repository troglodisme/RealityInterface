using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCube : MonoBehaviour
{
    public float moveSpeed = 5f; // speed of movement
    public float turnSpeed = 90f; // speed of turning

    // Update is called once per frame
    void Update()
    {
        // move forward/backward
        // transform.Translate(Vector3.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        // turn left/right
        transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
    }

    void Move() 
    {
        transform.Translate(Vector3.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);

    }
}



        // print(Input.GetAxis("Horizontal"));

        // transform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime, 0f, 0f);
        // transform.Translate(0f, Input.GetAxis("Vertical")*Time.deltaTime, 0f);


