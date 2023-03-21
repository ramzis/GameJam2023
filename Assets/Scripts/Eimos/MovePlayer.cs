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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerQuadMat = playerQuad.GetComponent<Renderer>();
    }

    private void Update()
    {
        ProcessWalking();
        ProcessRotation();
        ProcessAnimation();
        ProcessSound();
    }

    private Vector3 movement;
    private void ProcessWalking()
    {
        movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;
        rb.velocity = transform.TransformDirection(movement * speed);
    }

    private void ProcessRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            newRotation += 45;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            newRotation -= 45;
        }

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, newRotation, 0),
            Time.deltaTime * rotationSpeed
        );
    }

    private void ProcessAnimation()
    {
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D)
        )
        {
            playerQuad.transform.localRotation = Quaternion.Euler(45, Mathf.Cos(Time.time * 10) * 20, 0);
        }
        else
        {
            playerQuad.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerQuadMat.material.mainTexture = right;
            playerQuad.transform.localScale = new Vector3(4, 4, 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PlayerQuadMat.material.mainTexture = right;
            playerQuad.transform.localScale = new Vector3(-4, 4, 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PlayerQuadMat.material.mainTexture = forward;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            PlayerQuadMat.material.mainTexture = back;
        }
    }

    private void ProcessSound()
    {
        if (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D)
        )
        {
            SoundManagerScript.PlaySound("walking");
        }
        else
        {
            SoundManagerScript.PlaySound("stop_walking");
        }
    }
}
