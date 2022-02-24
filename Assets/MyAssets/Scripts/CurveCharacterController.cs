using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurveCharacterController : MonoBehaviour
{

//settings;
    [SerializeField] float maxSpeed;
    [SerializeField] AnimationCurve accelerationCurve;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] AnimationCurve fallCurve;



    //fetch data
    CharacterController getCharacterController { get { return GetComponent<CharacterController>(); } }
    bool grounded { get { return getCharacterController.isGrounded; } }
    PlayerInput getInput { get { return GetComponent<PlayerInput>(); } }


    void Start()
    {

    }

    void Update()
    {

     
        print("grounded: " + grounded);
        print("Getcomponent: " + GetComponent<CharacterController>().isGrounded);
    }

}
