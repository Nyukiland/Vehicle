using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AllVariable : MonoBehaviour
{

    [Header("Player Control")]
    public PlayerControls controls;


    [Space (10)]
    [Header("-----------------------------------------------------------------")]
    [Header ("Camera Variable")]

    [Tooltip("position of the camera when away from the player")] public Vector2 camDistFromPlayer1;
    [Tooltip("position of the camera when far away from the player")] public Vector2 camDistFromPlayer2;
    [Tooltip("position of the camera when in front of the vehicle")] public Vector3 positionCamAvant;
    [Tooltip("move the position and rotation of the camera using a 'smooth'")] public float lerpSpeedCam;


    [Space(10)]
    [Header("-----------------------------------------------------------------")]
    [Header("Movement Variable")]

    [Tooltip("maximum speed of the vehicle")] public float speedMax;
    [Tooltip ("time for the vehicle to increase his speed")] public float speedAcceleration;
    [Tooltip ("time for the vehicle to decrease his speed")] public float speedDeceleration;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
    }
}
