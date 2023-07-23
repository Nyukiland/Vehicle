using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplacement : MonoBehaviour
{
    [SerializeField] AllVariable manager;

    float accelerationInputValue;
    float decelerationInputValue;
    bool immediateBrake;
    Vector2 direction;

    float speedToGo;
    Vector3 velocity;
    Rigidbody rb;

    bool grounded;

    float supposedSpeed;

    bool crunch;

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

        manager.controls.Movement.crunch.performed += ctx => crunch = true;
        manager.controls.Movement.crunch.canceled += ctx => crunch = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = speedGestion() + TurnVehicle();
        velocity.y += rb.velocity.y + GravtyControl() + Crunching();

        rb.velocity = velocity;
    }


    Vector3 speedGestion()
    {
        speedToGo = (accelerationInputValue * manager.speedMax);

        if (speedToGo <= 0) speedToGo -= decelerationInputValue * manager.maxBackwardSpeed;
        else speedToGo -= decelerationInputValue;
        speedToGo += manager.joystickImpactOnSpeed * direction.y;

        if (immediateBrake) speedToGo = 0;

        return transform.forward *  MathForSpeed(speedToGo);
        
    }

    float MathForSpeed(float valeurToGo)
    {
        if (valeurToGo > supposedSpeed)
        {
            supposedSpeed += manager.acceleration * Time.deltaTime;
        }
        else if (valeurToGo < supposedSpeed)
        {
            supposedSpeed -= manager.deceleration * Time.deltaTime;
        }
        else if (valeurToGo < supposedSpeed && immediateBrake)
        {
            supposedSpeed += manager.acceleration * 3 * Time.deltaTime;
        }
        else if (valeurToGo > supposedSpeed && immediateBrake)
        {
            supposedSpeed -= manager.acceleration * 3 * Time.deltaTime;
        }

        return supposedSpeed;
    }

    Vector3 TurnVehicle()
    {
        Quaternion rota = Quaternion.Euler(0, transform.eulerAngles.y +(direction.x * (1.75f - accelerationInputValue)), 0);
        rb.MoveRotation(rota);
        return transform.right * (direction.x * (accelerationInputValue/2));
    }

    float GravtyControl()
    {
        RaycastHit hit;
        float lenghtOfRaycast = GetComponent<BoxCollider>().size.z * manager.heightForGrounded;

        if (Physics.Raycast(transform.position, -transform.up, out hit, lenghtOfRaycast))
        {
            grounded = true;
            return 0;
        }
        else
        {
            grounded = false;
            return -manager.gravityOfVehicle * Time.deltaTime;
        }
    }

    float Crunching()
    {
        if (crunch)
        {
            GetComponent<BoxCollider>().size = new Vector3(1, 0.5f, 1);
            GetComponent<BoxCollider>().center = new Vector3(0, -0.25f, 0);

            if (!grounded) return -manager.crunchFalling;
            else return 0;
        }
        else
        {
            GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            GetComponent<BoxCollider>().center = Vector3.zero;
            return 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            supposedSpeed = 0;
            velocity = Vector3.zero;
        }
    }
}
