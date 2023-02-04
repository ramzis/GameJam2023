using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMenu : MonoBehaviour
{
    private float maxCordOffsetX = 15.0f;
    private float minCordOffsetX = -15.0f;
    private float maxCordOffsetZ = 15.0f;
    private float minCordOffsetZ = -50.0f;

    private float amountToRotate = 20.0f;
    private float rotationAmountPerChange;

    private float movingSpeed = 0.15f;

    private bool reachedXZEnd = false;

    private Transform cameraTransform;


    void Start()
    {
        rotationAmountPerChange = amountToRotate / ((maxCordOffsetX + (minCordOffsetX * -1)) / movingSpeed);
        cameraTransform = GetComponent<Transform>();

        InvokeRepeating("ExecuteMethod", 0, 0.03125f);

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
