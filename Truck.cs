using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using static Recorder;

[RequireComponent(typeof(AttackComponent), typeof(MovementComponent))] 
public class Truck : MonoBehaviour
{
    [SerializeField] MyInputs controls = null;
    [SerializeField] InputAction move = null;
    [SerializeField] InputAction rotateX = null;
    [SerializeField] InputAction rotateY = null;
    [SerializeField] InputAction fire = null;
    [SerializeField] InputAction startRecording = null;
    [SerializeField] InputAction startReturn = null;
    [SerializeField] MovementComponent movement = null;
    [SerializeField] AttackComponent attack = null;
    [SerializeField] Recorder recorder;
    [SerializeField] Rigidbody rb;
    
    public MovementComponent Movement => movement;
    public Rigidbody Rb => rb;
    public InputAction Move => move;
    public InputAction RotateX => rotateX;
    public InputAction RotateY => rotateY;
    public InputAction Fire => fire;
    public InputAction StartRecording => startRecording;
    public InputAction StartReturn => startReturn;


    // Start is called before the first frame update
    private void Awake()
    {
        controls = new MyInputs();
    }
    void Start()
    {

        Init();
    }

    void Init()
    {
        movement = GetComponent<MovementComponent>();
    }

    // Update is called once per frame
    void Update()
    {
       
        movement.Move();
    }

  
    private void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();

        fire = controls.Player.Fire;
        fire.Enable();
        //fire.performed += TruckFire;              /// moved to attack compo

        rotateX = controls.Player.RotateX;
        rotateX.Enable();

        rotateY = controls.Player.RotateY;
        rotateY.Enable();

        startRecording = controls.Player.Record;
        startRecording.Enable();

        startReturn = controls.Player.Return;
        startReturn.Enable();

    }

    private void OnDisable()
    {
        move.Disable();
        rotateX.Disable();
        rotateY.Disable();
        fire.Disable();
        fire.performed -= attack.Attack;

        startRecording.Disable();
        //startRecording.performed -= recorder.ActivateCanStartRecording;
        startReturn.Disable();
    }


}
