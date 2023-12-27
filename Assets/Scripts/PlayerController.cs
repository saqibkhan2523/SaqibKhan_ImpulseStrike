using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float movementSpeed;
    private Rigidbody playerRb;

    public float mouseSensitivity = 5f;
    private float verticalRotation = 0.0f;

    public bool invertMouseY = true;

    public float firingRate = 0.05f;
    private float firingRateTimer = 0;
    private bool resetFireRate = false;

    public float playerHealth = 100;

    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        LockAndHideCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnManager.gameOver && !spawnManager.gamePause)
        {
            PlayerMovement();
            PlayerHorizontalRotation();
            PlayerVerticalRotation();
            Shooting();
        }
    }

    public void InvertY()
    {
        invertMouseY = true;
    }

    public void NonInvertY()
    {
        invertMouseY = false;
    }

    //Call from start UnPuase Game
    void LockAndHideCursor()
    {
        if (!spawnManager.gamePause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Call from pause game.
    void UnLockAndHideCursor()
    {
        if (spawnManager.gamePause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * movementSpeed);
    }

    void PlayerHorizontalRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    void PlayerVerticalRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        if (invertMouseY)
        {
            mouseY *= -1;
        }
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Clamping the vertical rotation

        Camera.main.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }

    void Shooting()
    {
        if(resetFireRate)
        {
            firingRateTimer += Time.deltaTime;
            if(firingRateTimer >= firingRate)
            {
                resetFireRate = false;
                firingRateTimer = 0;
            }
        }
        if(Input.GetMouseButtonDown(0) && !resetFireRate)
        {
            resetFireRate = true;
            Instantiate(bulletPrefab, firePoint.position, firePoint.transform.rotation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Cage"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                other.gameObject.GetComponent<CageScript>().OpenDoor();
            }
        }
    }
}
