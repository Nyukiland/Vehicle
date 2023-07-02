using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AllVariable : MonoBehaviour
{

    [Header("Player Control")]
    public PlayerControls controls;

    [Space (10)]
    [Header ("Camera Variable")]

    public Vector2 camDistFromPlayer1;
    public Vector2 camDistFromPlayer2;
    public Vector3 positionCamAvant;
    public float lerpSpeedCam;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
    }
}
