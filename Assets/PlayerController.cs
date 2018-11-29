using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float runMultiplier;

    private bool isRunning = false;
    private float MAXSPEED;
    private float BASESPEED;

    public float rotateFactor = 500.0f;
    public float pitchFactor = 500.0f;
    public int pickupCount = 0;

    public float detectionDistance = 4.0f;
    private Vector3 playerVec;
    private Vector3 targetVec;

    private Transform eyeMount;
    private CharacterController characterController;
    //private SpawnerScript spawnerThing;
    private Camera mainCam;

    public LayerMask raycastLayers;
    public LayerMask PickupOnly;
    public LayerMask Objects;
    public bool isHittingPickup = false;

    public float rayDistance = 3.0f;
    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //spawnerThing = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        eyeMount = transform.Find("EyeMount");

        speed = 10;
        BASESPEED = speed;
        MAXSPEED = speed * runMultiplier;
    }

    void Update()
    {
        
        #region movement
        if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            speed = BASESPEED;
        }
        if (isRunning)
        {
            speed *= runMultiplier;
            if (speed > MAXSPEED) speed = MAXSPEED;
        }

        //movement
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection += -transform.right;
        if (Input.GetKey(KeyCode.S)) moveDirection += -transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        transform.Rotate(Vector3.up, rotateFactor * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null)
        {
            eyeMount.Rotate(Vector3.right, -rotateFactor * (Input.GetAxis("Mouse Y") * Time.deltaTime));
        }
        characterController.SimpleMove(moveDirection.normalized * speed);
        #endregion
    }
}
