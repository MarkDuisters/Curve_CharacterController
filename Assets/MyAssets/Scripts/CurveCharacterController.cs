using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CurveCharacterController : MonoBehaviour
{

    //settings;
    [SerializeField] float speed;
    [SerializeField] AnimationCurve accelerationCurve;
    // [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpMultiplier = 10;
    [SerializeField] float jumpGravityMultiplier = 2;
    [SerializeField] float fallGravityMultiplier = 4;

    //  [SerializeField] AnimationCurve fallCurve;

    //private
    PlayerActionControls getInputs;
    bool moveIsPressed = false;

    Vector3 moveDir;
    Rigidbody rb;
    float sampleTime = 0f;


    //fetch data
    CharacterController getCharacterController { get { return GetComponent<CharacterController>(); } }
    bool grounded { get { return getCharacterController.isGrounded; } }


    void Awake()
    {
        //create a new instance for your input controls.
        getInputs = new PlayerActionControls();
        rb = GetComponent<Rigidbody>();
    }


    void Start()
    {
        getInputs.Player.Jump.performed += _ => DoWhenJump();

        //with this state we can control a bool in order to detect if the button still is being pressed
        //an alternative would be using the .IsPressed() method from the current action.
        /* getInputs.Player.Move.performed += _ => moveIsPressed = true;
         getInputs.Player.Move.canceled += _ => moveIsPressed = false;*/


    }

    void OnEnable()
    {
        getInputs.Enable();
    }
    void OnDisalbe()
    {
        getInputs.Disable();
    }


    void Update()
    {
        moveIsPressed = getInputs.Player.Move.IsPressed();


        //when moving count the sample time up, otherwise count it down. Then clamp it between 0-1.
        if (moveIsPressed)
        {
            sampleTime += Time.deltaTime;
            sampleTime = Mathf.Clamp01(sampleTime);
        }
        else
        {
            sampleTime -= Time.deltaTime;
            sampleTime = Mathf.Clamp01(sampleTime);


        }


    }

    private float SampleAccelerationCurve()
    {
        return (accelerationCurve.Evaluate(sampleTime));
    }

    void FixedUpdate()
    {


        Movement();//Controls movement vectors based on input, gravity and jump force.

        BetterGravity();

    }


    //Controlled through jump input event.
    void DoWhenJump()
    {
        //place here code that should execute when a jump button/event is fired.

        print("jump is pressed");

        //  getCharacterController.Move(moveDir * Time.deltaTime);
        rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
    }



    void Movement()
    {

        if (!moveIsPressed)
        {
            return;
        }

        moveDir = MovementDirection() * SampleAccelerationCurve() * speed;
        transform.rotation = LookDirection(moveDir);

        moveDir.y = rb.velocity.y;//We keep our current velocity. This way we can use forces for gravity, jumping, etc...
        rb.velocity = moveDir;

        //   getCharacterController.Move(moveDir * Time.deltaTime);//We apply Time.deltaTime on the last line. This way we can skip Time.deltaTime in our other calculations.
    }


    //Calculate direction vector based on inputs
    Vector3 MovementDirection()
    {
        Vector2 movementInput = getInputs.Player.Move.ReadValue<Vector2>();
        Vector3 convertToVector3 = new Vector3(movementInput.x, 0, movementInput.y);
        return convertToVector3;

    }

    //Use a directional vector to rotate our object.
    Quaternion LookDirection(Vector3 dir)
    {
        Quaternion getRotation = Quaternion.LookRotation(dir);
        return getRotation;

    }


    void BetterGravity()
    {
        //When falling appy the extra gravity. When moving up and not pressing the jump button apply a different gravity to determine jump height.
        if (rb.velocity.y < 0f)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallGravityMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0f && !getInputs.Player.Jump.IsPressed())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * jumpGravityMultiplier * Time.deltaTime;

        }


    }

}
