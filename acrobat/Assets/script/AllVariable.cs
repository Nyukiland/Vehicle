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
    [Tooltip("move the position and rotation of the camera using a 'smooth'")] [Range (0f,1f)] public float lerpSpeedCam;


    [Space(10)]
    [Header("-----------------------------------------------------------------")]
    [Header("Movement Variable")]

    [Tooltip("maximum speed of the vehicle")] public float speedMax;
    [Tooltip ("time for the vehicle to increase his speed")] [Range (0f, 1f)] public float lerpAcceleration;
    [Tooltip ("decrease power")] [Range (1f, 10f)] public float speedDeceleration;
    [Tooltip ("amount of speed added to the vehicle when the joystick is forward or backward")] [Range (0f, 1f)] public float joystickImpactOnSpeed;
    [Tooltip ("time for the vehicle to rotate to the input value")] [Range (0f, 1f)] public float lerpRotation;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
    }
}
