using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    float horiOffset;
    float vertOffset;
    
    [Header("Player Setup")]
    [Tooltip("Set movement controls")] 
    [SerializeField] InputAction movement;
    [Tooltip("Set firing controls")] 
    [SerializeField] InputAction fire;
    [Tooltip("Set exit key")] 
    [SerializeField] InputAction exit;

    [Header("Translation Multiplier Factors")]
    [Tooltip("Horizontal movement multiplier")] 
    [SerializeField] float horiMultiplier = 1f;
    [Tooltip("Vertical movement multiplier")] 
    [SerializeField] float vertMultiplier = 1f;
    [Tooltip("Horizontal clamp range")] 
    [SerializeField] float xRange = 5f;
    [Tooltip("Vertical clamp range")] 
    [SerializeField] float yRange = 5f;

    [Header("Rotation Multiplier Factors")]
    [Tooltip("Ship's nose's up-down rotation factor according to position")] 
    [SerializeField] float positionPitchFactor = -2f;
    [Tooltip("Ship's nose's up-down rotation factor according to the thrust on the ship")] 
    [SerializeField] float throwPitchFactor = -5f;
    [Tooltip("Ship's nose's left-right rotation factor according to position")] 
    [SerializeField] float positionYawFactor = 2f;
    [Tooltip("Ship's side to side rotation factor according to the thrust on the ship")] 
    [SerializeField] float throwRollFactor = -10f;

    [Header("Laser Beams")]
    [Tooltip("Laser Beam GameObjects present on the ship")]
    [SerializeField] ParticleSystem[] lasers;
    // [SerializeField] ParticleSystem leftFire;
    // [SerializeField] ParticleSystem rightFire;

    float xThrow;
    float yThrow;
    bool laserStatus = false;

    // Start is called before the first frame update
    void Awake()
    {
        exit.performed += _ => ApplicationQuit();
        fire.started += _ => handleFiring();
        fire.performed += _ => stopFiring();

        foreach (ParticleSystem laser in lasers)
        {
            laser.Stop();
        }
    }

    void OnEnable() {
        movement.Enable();
        exit.Enable();
        fire.Enable();
    }

    void OnDisable() {
        movement.Disable();
        exit.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        handleTranslation();
        handleRotation();
    }

    void ApplicationQuit () {
        Debug.Log("Quitting App");
        Application.Quit();
    }

    void handleTranslation() {
        // horiOffset = horiMultiplier * Time.deltaTime * Input.GetAxis("Horizontal");
        // vertOffset = vertMultiplier * Time.deltaTime * Input.GetAxis("Vertical");
        
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        horiOffset = horiMultiplier * Time.deltaTime * xThrow;
        vertOffset = vertMultiplier * Time.deltaTime * yThrow;
        
        float rawXPos = transform.localPosition.x + horiOffset;
        float rawYPos = transform.localPosition.y + vertOffset;

        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        // transform.Translate(horiOffset, vertOffset, 0, Space.Self)
        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void handleRotation() {
        // down - increase pitch, up - decrease pitch
        // right - increase yaw, left - reduce yaw

        float positionPitch = transform.localPosition.y * positionPitchFactor;
        float controlThrowPitch = yThrow * throwPitchFactor;
        float positionYaw = transform.localPosition.x * positionYawFactor;
        float controlThrowRoll = xThrow * throwRollFactor;

        float pitch = positionPitch + controlThrowPitch;
        float yaw = positionYaw;
        float roll = controlThrowRoll;
        
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void handleFiring() {
        // activate particle system
        toggleLasers();
    }

    void stopFiring() {
        toggleLasers();
    }

    void toggleLasers() {
        // Debug.Log(laserStatus);

        if (laserStatus == true) {
            foreach (ParticleSystem laser in lasers) {
                laser.Stop();
            } 
            laserStatus = false;
        } else {
            foreach (ParticleSystem laser in lasers) {
                laser.Play();
            }
            laserStatus = true;
        }
    }
}
