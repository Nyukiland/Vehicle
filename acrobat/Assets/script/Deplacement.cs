using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    [SerializeField] AllVariable manager;

    float speedToGo;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        manager.controls.Movement.Accelerate.performed += V => speedToGo = V.ReadValue <float>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
