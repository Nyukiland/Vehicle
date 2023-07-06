using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    [SerializeField] AllVariable manager;

    float accelerationInputValue;
    float decelerationInputValue;
    bool immediateBrake;
    float speedToGo;
    Vector2 direction;
    Vector3 velocity;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        manager.controls.Movement.Accelerate.performed += V => accelerationInputValue = V.ReadValue <float>();
        manager.controls.Movement.Accelerate.canceled += V => accelerationInputValue = 0;

        manager.controls.Movement.Brake.performed += V => decelerationInputValue = V.ReadValue <float>();
        manager.controls.Movement.Brake.canceled += V => decelerationInputValue = 0;

        manager.controls.Movement.CompleteBrake.performed += V => immediateBrake = true;
        manager.controls.Movement.CompleteBrake.canceled += V => immediateBrake = false;

        manager.controls.Movement.Move.performed += V => direction = V.ReadValue<Vector2>();
        manager.controls.Movement.Move.canceled += V => direction = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = rb.velocity;

        speedGestion();
        TurnVehicle();

        rb.velocity = velocity;
    }

    void speedGestion()
    {
        speedToGo = (accelerationInputValue * manager.speedMax);

        if (speedToGo <= 0) speedToGo -= decelerationInputValue * (manager.speedMax / manager.speedDeceleration);
        else speedToGo -= decelerationInputValue;

        if (immediateBrake) speedToGo = 0;

        speedToGo += manager.joystickImpactOnSpeed * direction.y;

        velocity = transform.forward *  speedToGo;
        
    }

    void TurnVehicle()
    {
        Quaternion rota = Quaternion.Euler(0, transform.eulerAngles.y +(direction.x * (1.75f - accelerationInputValue)), 0);
        rb.MoveRotation(rota);
        velocity += transform.right * (direction.x * (accelerationInputValue/2));
    }
}
