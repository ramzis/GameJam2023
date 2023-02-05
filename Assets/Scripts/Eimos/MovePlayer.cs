using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;
    private Rigidbody rb;
    float newRotation = 0;

    [SerializeField] Texture forward;
    [SerializeField] Texture back;
    [SerializeField] Texture right;

    [SerializeField] GameObject playerQuad;
    Renderer PlayerQuadMat;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerQuadMat = playerQuad.GetComponent<Renderer>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
        rb.velocity = transform.TransformDirection(movement * speed);

        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D))
        {
            SoundManagerScript.PlaySound("walking");
            
            playerQuad.transform.localRotation = Quaternion.Euler(45, Mathf.Cos(Time.time * 10) * 20, 0);
        }
        else
        {
            SoundManagerScript.PlaySound("stop_walking");
            playerQuad.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }


        if (Input.GetKey(KeyCode.D))
        {
            PlayerQuadMat.material.mainTexture = right;
            playerQuad.transform.localScale= new Vector3(4,4,1);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            PlayerQuadMat.material.mainTexture = right;
            playerQuad.transform.localScale = new Vector3(-4, 4, 1);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            PlayerQuadMat.material.mainTexture = forward;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            PlayerQuadMat.material.mainTexture = back;
        }




        if (Input.GetKeyDown(KeyCode.Q))
        {
            newRotation += 45;
            //transform.Rotate(0, 45, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            newRotation -= 45;
            //transform.Rotate(0, -45, 0);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,newRotation,0), Time.deltaTime * rotationSpeed);
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.CompareTag("Hitbox"))
    //    {
    //        // Handle collision with hitbox
    //        rb.velocity = Vector3.zero;
    //    }
    //}
}
