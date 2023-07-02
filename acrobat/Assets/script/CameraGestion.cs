using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraGestion : MonoBehaviour
{
    [SerializeField] AllVariable manager;

    int camState = 0;
    bool lookBehind;
    Vector2 cameraRot;


    private void Start()
    {
        manager.controls.CameraMove.ChangeCam.performed += dontCare => ChangeCam();
        manager.controls.CameraMove.LookBack.performed += changeVariable => lookBehind = true;
        manager.controls.CameraMove.LookBack.canceled += changeVariable => lookBehind = false;
        manager.controls.CameraMove.Rotation.performed += useVariable => cameraRot = useVariable.ReadValue<Vector2>();
        manager.controls.CameraMove.Rotation.canceled += useVariable => cameraRot = Vector2.zero;
    }


    // Update is called once per frame
    void Update()
    {
        CamPosition();
    }

    void ChangeCam()
    {
        camState++;
        if (camState >= 3) camState = 0;
    }

    void CamPosition()
    {
        Vector2 camStatePos = Vector2.zero;
        if (camState == 0) camStatePos = manager.distFromPlayer1;
        else if (camState == 1) camStatePos = manager.distFromPlayer2;


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
            transform.localRotation = new Quaternion(Mathf.Abs(cameraRot.y) * 90, cameraRot.x * 90, 0, 0);
        }
    }
}
