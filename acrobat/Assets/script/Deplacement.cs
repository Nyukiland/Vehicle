using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    [SerializeField] AllVariable manager;

    float accelerationInputValue;
    float decelerationInputValue;
    float speedToGo;
    Vector2 direction;
    Vector3 velocity;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        manager.controls.Movement.Accelerate.performed += V => accelerationInputValue = V.ReadValue <float>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        speedGestion();

        rb.velocity = velocity;
    }

    void speedGestion()
    {
        speedToGo += (accelerationInputValue * manager.speedMax) - (decelerationInputValue * manager.speedMax);

    }
}
