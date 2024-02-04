using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float jumpForce;
    public float groundCheckDistance;
    public float movementSpeed;
    private Rigidbody playerRb;

    public float mouseSensitivity = 5f;
    private float verticalRotation = 0.0f;

    public bool invertMouseY = true;

    public float autoFiringRate = 0.15f;
    public float singleFiringRate = 0.25f;
    public bool autoFiringMode = false;
    [SerializeField] private TextMeshProUGUI firingModeText;
    private float firingRateTimer = 0;
    private bool resetFireRate = false;

    public ParticleSystem muzzleFlashPS;

    public float playerHealth = 100;

    private SpawnManager spawnManager;

    public AudioSource gunAudioSrc;

    private int bulletCount = 0;
    

    public float recoilAmount = 2f; // How much the gun kicks back
    public float recoilRecoveryRate = 1f; // How quickly the gun returns to original position
    private float currentRecoil = 0f; // Current recoil amount


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        spawnManager.LockAndHideCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnManager.gameOver && !spawnManager.gamePause)
        {
            PlayerMovement();
            PlayerHorizontalRotation();
            PlayerVerticalRotation();
            FiringMode();
            if (currentRecoil > 0)
            {
                float recoilRecovery = recoilRecoveryRate * Time.deltaTime;
                currentRecoil -= recoilRecovery;
                ApplyRecoilEffect();
            }

            if(playerHealth <= 0)
            {
                spawnManager.GameLoseCondition();
            }
        }


    }

    private void FiringMode()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            autoFiringMode = !autoFiringMode;
        }
        if (autoFiringMode)
        {
            Shooting(autoFiringRate);
            firingModeText.text = "T to Single";
        }
        else
        {
            Shooting(singleFiringRate);
            firingModeText.text = "T to Auto";
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

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * movementSpeed);
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * movementSpeed);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Inside Jump");
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        // Check if the player is grounded
        // You can use a Raycast or check the player's position relative to the ground
        // Example:
        return Physics.Raycast(transform.position, -Vector3.up, groundCheckDistance);
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

    void Shooting(float fireRate)
    {
        
        if (autoFiringMode)
        {
            
            if (Input.GetMouseButton(0)) // Continuous fire for automatic mode
            {
                firingRateTimer += Time.deltaTime;

                if (firingRateTimer >= fireRate)
                {
                    FireBullet(fireRate);
                    firingRateTimer = 0;
                }
            }
        }
        else
        {
            
            if (Input.GetMouseButtonDown(0) && firingRateTimer <= 0) // Single fire for single-shot mode
            {
                FireBullet(fireRate);
            }

            if (firingRateTimer >= 0)
            {
                firingRateTimer -= Time.deltaTime;
            }
        }

        if (resetFireRate)
        {
            if (!Input.GetMouseButton(0))
            {
                resetFireRate = false;
            }
        }
    }

    void FireBullet(float fireRate)
    {
        muzzleFlashPS.Play();
        bulletCount++;
        resetFireRate = true;
        firingRateTimer = fireRate; // Reset the timer for next shot

        Instantiate(bulletPrefab, firePoint.position, firePoint.transform.rotation); // Fire the bullet
        if (gunAudioSrc != null)
        {
            gunAudioSrc.Play(); // And play the sound from the beginning
        }

        currentRecoil += recoilAmount;
        ApplyRecoilEffect();
    }

    void ApplyRecoilEffect()
    {
        verticalRotation -= currentRecoil;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
       
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
