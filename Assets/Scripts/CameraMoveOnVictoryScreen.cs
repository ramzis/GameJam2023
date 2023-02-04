using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //Maximum, minimum map cords
    private float maxCordOffsetX = 15.0f;
    private float minCordOffsetX = -15.0f;
    private float maxCordOffsetZ = 15.0f;
    private float minCordOffsetZ = -50.0f;

    //Degrees to rotate from start (Default - -10 to 10)
    private float amountToRotate = 20.0f;
    private float rotationAmountPerChange;

    //Moving speed of camera
    private float movingSpeed = 0.15f;

    //Boolean to check whether camera reached end x/z iteration
    private bool reachedXZEnd = false;

    //Camera transform
    private Transform cameraTransform;

    
    
    void Start()
    {
        //Setting amount of degrees camera should rotate per method tick
        rotationAmountPerChange = amountToRotate / ((maxCordOffsetX + (minCordOffsetX * -1)) / movingSpeed);
        cameraTransform = GetComponent<Transform>();

        //Executes 32 times each second
        InvokeRepeating("ExecuteMethod", 0, 0.03125f);

        //Camera start position at left/center
        cameraTransform.transform.position = new Vector3(minCordOffsetX, cameraTransform.position.y, cameraTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Camera rotation logic
    void ExecuteMethod()
    {
        if (!reachedXZEnd)
        {
            if (cameraTransform.position.x < maxCordOffsetX)
            {
                moveX(movingSpeed);
                rotateCameraY(rotationAmountPerChange);
                return;
            }
            else if (cameraTransform.position.z < maxCordOffsetZ)
            {
                moveZ(movingSpeed);
                return;
            }
            reachedXZEnd = true;
        }
        else {
            if (cameraTransform.position.x > minCordOffsetX)
            {
                moveX(-movingSpeed);
                rotateCameraY(-rotationAmountPerChange);
                return;
            }
            else if (cameraTransform.position.z > minCordOffsetZ)
            {
                moveZ(-movingSpeed);
                return;
            }
            reachedXZEnd = false;
        }
    }

    private void moveX(float moveAmount)
    {
        cameraTransform.transform.position = new Vector3(
            cameraTransform.position.x + moveAmount, 
            cameraTransform.position.y, 
            cameraTransform.position.z);
    }

    private void moveZ(float moveAmount)
    {
        cameraTransform.transform.position = new Vector3(
            cameraTransform.position.x, 
            cameraTransform.position.y, 
            cameraTransform.position.z + moveAmount);
    }

    private void rotateCameraY(float rotationAmmount)
    {
        cameraTransform.transform.rotation = Quaternion.Euler(
            cameraTransform.transform.rotation.eulerAngles.x,
            cameraTransform.transform.rotation.eulerAngles.y + rotationAmmount,
            cameraTransform.transform.rotation.eulerAngles.z);
    }
}
