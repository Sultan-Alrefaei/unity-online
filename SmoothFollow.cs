using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SmoothFollow : MonoBehaviour
{

    /*
    This camera smoothes out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.

    There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

    For every of those smoothed values we calculate the wanted value and the current value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position.
    */


    // The target we are following
    //[Header("Target object")]
    Transform target;
    
    // The distance in the x-z plane to the target
    [Header("The distance between camera and object")]
    [Range(0, 50)]
    [SerializeField] float distance = 10.0f;
    // the height we want the camera to be above the target
    [Range(0, 100)]
    [Header("Height of camera")]
    [SerializeField] float height = 5.0f;
    // How much we 
    [Header("Setup")]
    [Range(0, 20)]
    [SerializeField] float heightDamping = 2.0f;
    [Range(0, 20)]
    [SerializeField] float rotationDamping = 3.0f;

    [MenuItem("Camera-Control/Smooth Follow")]

    // Update function
    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else if (GameObject.FindGameObjectWithTag("Car") != null)
        {
            target = GameObject.FindGameObjectWithTag("Car").transform;
        }



        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        Vector3 pos = new Vector3(transform.position.x, currentHeight, transform.position.z);
        transform.position = pos;

        // Always look at the target
        transform.LookAt(target);
    }
}