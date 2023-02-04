using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BopUpAndDown : MonoBehaviour
{
    public Transform quad;
    float originalY;
    // Start is called before the first frame update
    void Start()
    {
        originalY = quad.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        quad.transform.position = new Vector3(quad.transform.position.x, originalY+Mathf.Sin(Time.time*3)*0.15f, quad.transform.position.z);
    }
}
