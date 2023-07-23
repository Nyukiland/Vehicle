using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AllVariable : MonoBehaviour
{

    [Header("Player Control")]
    public PlayerControls controls;


    [Space(10)]
    [Header("-----------------------------------------------------------------")]
    [Header("Camera Variable")]

    [Tooltip("position of the camera when away from the player")] public Vector2 camDistFromPlayer1;
    [Tooltip("position of the camera when far away from the player")] public Vector2 camDistFromPlayer2;
    [Tooltip("position of the camera when in front of the vehicle")] public Vector3 positionCamAvant;
    [Tooltip("move the position and rotation of the camera using a 'smooth'")] [Range (0f,1f)] public float followSpeedCam;


    [Space(10)]
    [Header("-----------------------------------------------------------------")]
    [Header("Movement Variable")]

    [Tooltip("maximum speed of the vehicle")] public float speedMax;
    [Tooltip ("maximum speed of the vehicle when he goes backward")] public float maxBackwardSpeed;

    [Tooltip("time for the vehicle to reach it's full speed")] [Range(1f, 5f)] public float acceleration;
    [Tooltip("time for the vehicle to decrease his speed")] [Range(1f, 5f)] public float deceleration;

    [Tooltip ("amount of speed added to the vehicle when the joystick is forward or backward")] [Range (0f, 1f)] public float joystickImpactOnSpeed;
    [Tooltip("the lower the value is the closer to the ground it goes")] [Range(1f, 1.5f)] public float heightForGrounded;
    [Tooltip("speed applied to the fall (must be positive)")] public float gravityOfVehicle;
    [Tooltip("speed applied to the fall when crunching (must be positive)")] public float crunchFalling;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
    }
}
