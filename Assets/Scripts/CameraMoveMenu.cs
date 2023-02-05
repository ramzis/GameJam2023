using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMenu : MonoBehaviour
{
    //Cords for max x / z
    private float maxCordOffsetX = 15.0f;
    private float minCordOffsetX = -15.0f;
    private float maxCordOffsetZ = 15.0f;
    private float minCordOffsetZ = -50.0f;

    //Camera rotation variables
    private float amountToRotate = 30.0f;
    private float rotationAmountPerChange;

    //Camera moving speed
    private float movingSpeed = 0.15f;

    //Boolean to determine whether camera reached end of terrain
    private bool reachedXZEnd = false;

    private Transform cameraTransform;


    void Start()
    {
        //Rotation change amount per one call
        rotationAmountPerChange = amountToRotate / ((maxCordOffsetX + (minCordOffsetX * -1)) / movingSpeed);
        cameraTransform = GetComponent<Transform>();

        //Execute method 32 times per second (>30 fps for fluency)
        InvokeRepeating("ExecuteMethod", 0, 0.03125f);

        //Initial camera position
        cameraTransform.transform.position = new Vector3(minCordOffsetX, cameraTransform.position.y, cameraTransform.position.z);
    }

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
        else 
        {
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
