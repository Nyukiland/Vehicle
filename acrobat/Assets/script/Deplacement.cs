using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    //bool grounded;

    float supposedSpeed;
    float modifiedMaxSpeed;

    bool crunch;
    float speedCrunch;

    [SerializeField] GameObject gestionVisu;
    [SerializeField] GameObject pivotRota;
    [SerializeField] GameObject boost;
    [SerializeField] GameObject cam;
    [SerializeField] TextMeshProUGUI textSpeed;
    [SerializeField] TextMeshProUGUI textBestSpeed;

    Vignette vign;
    LensDistortion lens;
    float bestSpeed;

    float crunchShapeKey = 0;

    bool slowmo;

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

        manager.controls.Movement.SlowMo.performed += ctx => slowmo = true;
        manager.controls.Movement.SlowMo.canceled += ctx => slowmo = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = speedGestion() + TurnVehicle();
        velocity.y += rb.velocity.y + GravtyControl();
        Crunching();
        

        rb.velocity = velocity;
    }

    private void Update()
    {
        SlowMotionActivation();


        //animation control

        //crunch anim

        if (crunch)
        {
            boost.SetActive(true);
            crunchShapeKey += 100 * Time.deltaTime;
        }
        else
        {
            boost.SetActive(false);
            crunchShapeKey -= 100 * Time.deltaTime;
        }

        crunchShapeKey = Mathf.Clamp(crunchShapeKey, 0, 100);
        gestionVisu.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, crunchShapeKey);

        /*
        //landing anim
        float landShapeKey = 0;

        if (!grounded) landShapeKey += 200 * Time.deltaTime;
        else landShapeKey -= 200 * Time.deltaTime;
        landShapeKey = Mathf.Clamp(landShapeKey, 0, 100);
        gestionVisu.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, landShapeKey); */

        //rota anim
         pivotRota.transform.localEulerAngles = new Vector3(0, 0, -(direction.x * ((Mathf.Abs(supposedSpeed) / manager.speedMax) * 45)));

        //anim cam
        cam.GetComponent<Volume>().profile.TryGet<Vignette>(out vign);
        vign.intensity.value = (Mathf.Abs(supposedSpeed) / manager.speedMax);

        cam.GetComponent<Volume>().profile.TryGet<LensDistortion>(out lens);
        lens.intensity.value = -(Mathf.Abs(supposedSpeed) / manager.speedMax)/3;

        //text
        if (supposedSpeed>bestSpeed)
        {
            bestSpeed = supposedSpeed;
            textBestSpeed.text = "Best speed:" + Mathf.Round(bestSpeed);
        }
        textSpeed.text = Mathf.Round(supposedSpeed) + "km/h";
    }

    Vector3 speedGestion()
    {
        speedToGo = (accelerationInputValue * manager.speedMax);

        if (speedToGo <= 0) speedToGo -= decelerationInputValue * manager.maxBackwardSpeed;
        else speedToGo -= decelerationInputValue;
        speedToGo += manager.joystickImpactOnSpeed * direction.y;

        if (crunch) speedToGo += speedCrunch;

        if (immediateBrake) speedToGo = 0;


        return transform.forward *  MathForSpeed(speedToGo);
        
    }


    float MathForSpeed(float valeurToGo)
    {
        if (valeurToGo > supposedSpeed && supposedSpeed < 50)
        {
            supposedSpeed += manager.acceleration *2 * Time.deltaTime;
        }
        else if (valeurToGo > supposedSpeed)
        {
            supposedSpeed += manager.acceleration * Time.deltaTime;
        }
        else if (valeurToGo < supposedSpeed && valeurToGo > manager.speedMax)
        {
            supposedSpeed -= (manager.deceleration + decelerationInputValue)/2 * Time.deltaTime;
        }
        else if (valeurToGo < supposedSpeed)
        {
            supposedSpeed -= (manager.deceleration + decelerationInputValue) * Time.deltaTime;
        }
        else if (valeurToGo < supposedSpeed && immediateBrake)
        {
            supposedSpeed += manager.deceleration * 3 * Time.deltaTime;
        }
        else if (valeurToGo > supposedSpeed && immediateBrake)
        {
            supposedSpeed -= manager.deceleration * 3 * Time.deltaTime;
        }

        return supposedSpeed;
    }

    Vector3 TurnVehicle()
    {
        if (supposedSpeed < 0.5f && supposedSpeed > -0.5f)
        {
            rb.MoveRotation(Quaternion.Euler(transform.eulerAngles));
            return Vector3.zero;
        }


        Quaternion rota = Quaternion.Euler(0, transform.eulerAngles.y +(direction.x * (1.75f - (Mathf.Abs(supposedSpeed)/manager.speedMax))), 0);
        if (slowmo && supposedSpeed > 25) rota = Quaternion.Euler(0, transform.eulerAngles.y + ((direction.x * (1.75f - (Mathf.Abs(supposedSpeed) / manager.speedMax)))*2), 0);
        if (slowmo && supposedSpeed > 60) rota = Quaternion.Euler(0, transform.eulerAngles.y + ((direction.x * (1.75f - (Mathf.Abs(supposedSpeed) / manager.speedMax))) * 3), 0);
        if (crunch) rota = Quaternion.Euler(0, transform.eulerAngles.y + ((direction.x * (1.75f - (Mathf.Abs(supposedSpeed) / manager.speedMax))) /1.5f), 0);
        rb.MoveRotation(rota);
        

        return transform.right * (direction.x * ((Mathf.Abs(supposedSpeed) / manager.speedMax) / 2));
    }

    float GravtyControl()
    {
        RaycastHit hit;
        float lenghtOfRaycast = GetComponent<BoxCollider>().size.z * manager.heightForGrounded;

        if (Physics.Raycast(transform.position, -transform.up, out hit, lenghtOfRaycast))
        {
            //grounded = true;
            return 0;
        }
        else
        {
            //grounded = false;
            return -manager.gravityOfVehicle * Time.deltaTime;
        }
    }

    void Crunching()
    {

        if (crunch)
        {
            GetComponent<BoxCollider>().size = new Vector3(1, 0.5f, 1);
            GetComponent<BoxCollider>().center = new Vector3(0, -0.25f, 0);

            speedCrunch += 5 * Time.deltaTime;
        }
        else
        {
            GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            GetComponent<BoxCollider>().center = Vector3.zero;

            speedCrunch = 0;
        }
    }

    void SlowMotionActivation()
    {
        if (manager.isPause) return;

        if(slowmo)
        {
            Time.timeScale = 0.5f;
            ChromaticAberration chrom;
            cam.GetComponent<Volume>().profile.TryGet<ChromaticAberration>(out chrom);
            chrom.intensity.value = 1;
        }
        else
        {
            Time.timeScale = 1f;
            ChromaticAberration chrom;
            cam.GetComponent<Volume>().profile.TryGet<ChromaticAberration>(out chrom);
            chrom.intensity.value = 0;
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
