using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraGestion : MonoBehaviour
{
    [SerializeField] AllVariable manager;
    [SerializeField] GameObject moto;
    [SerializeField] GameObject motoPivot;

    int camState = 0;
    bool lookBehind;
    Vector2 cameraRot;

    Vector3 smoothVelocity = Vector3.zero;


    private void Start()
    {
        manager.controls.CameraMove.ChangeCam.performed += dontCare => ChangeCam();

        manager.controls.CameraMove.LookBack.performed += changeVariable => lookBehind = true;
        manager.controls.CameraMove.LookBack.canceled += changeVariable => lookBehind = false;

        manager.controls.CameraMove.Rotation.performed += useVariable => cameraRot = useVariable.ReadValue<Vector2>();
        manager.controls.CameraMove.Rotation.canceled += useVariable => cameraRot = Vector2.zero;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        CamPosition();
        followAndRot();
    }

    void ChangeCam()
    {
        camState++;
        if (camState >= 3) camState = 0;
    }

    void CamPosition()
    {
        Vector2 camStatePos = Vector2.zero;
        if (camState == 0) camStatePos = manager.camDistFromPlayer1;
        else if (camState == 1) camStatePos = manager.camDistFromPlayer2;

        

        if (camState == 2)
        {
            Camera.main.transform.localPosition = manager.positionCamAvant;

            if (lookBehind) Camera.main.transform.localRotation = new Quaternion(0, 180, 0, 0);
            else Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            

            Camera.main.transform.LookAt(transform);

            if (lookBehind)
            {
                Camera.main.transform.localPosition = new Vector3(0, camStatePos.y, -camStatePos.x);
                Camera.main.transform.localRotation = new Quaternion(0, 180, 0, 0);
                

            }
            else
            {
                Camera.main.transform.localPosition = new Vector3(0, camStatePos.y, camStatePos.x);
                Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            transform.localEulerAngles = new Vector3(Mathf.Clamp(cameraRot.y * 90, 0, 90), -cameraRot.x * 90, 0);
        }
    }

    void followAndRot()
    {
        motoPivot.transform.localRotation = moto.transform.localRotation;
        if (camState != 2) motoPivot.transform.position = Vector3.SmoothDamp(motoPivot.transform.position, moto.transform.position, ref smoothVelocity, manager.followSpeedCam);
        else motoPivot.transform.position = moto.transform.position;
    }
}
