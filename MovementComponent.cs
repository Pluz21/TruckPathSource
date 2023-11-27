using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Recorder;

public class MovementComponent : baseComp
{
    [SerializeField] int currentIndex = 0;

    [SerializeField] float initialMoveSpeed = 0;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float maxMoveSpeed = 20;
    [SerializeField] float rotateSpeed = 50;
    [SerializeField] float lerpSpeed = 1;
    [SerializeField] float phantomWalkLerpSpeed = 2;

    [SerializeField] float minDistanceToContinuePhantomWalk = 1;

    [SerializeField] float currentTime = 0;
    [SerializeField] float maxtime = 2;

    [SerializeField] Vector3 positionAtEndRecord = Vector3.zero;
    //[SerializeField] Vector3 positionAtStartPhantomWalk = Vector3.zero;

    [SerializeField] bool canStartTimer = false;
    [SerializeField] bool canReset = false;
    [SerializeField] bool loopRecord = true;
    [SerializeField] bool canStartReturning = false;
    [SerializeField] bool canStartPhantomWalk = false;

    [SerializeField] Recorder recorder = null;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; } 
    }

    public float MaxMoveSpeed => maxMoveSpeed;

    public event Action OnReturnedToInitialPosition;
    public event Action OnTick;

   

    protected override void Start()
    {
        base.Start();
        OnReturnedToInitialPosition += ActivateCanStartTimer;
        OnTick += ActivateCanStartPhantomWalk;
        Init();

    }

    void Update()
    {
        playerRef.StartReturn.performed += ActivateCanStartReturning;
       
        Move();
        RotateX();
        RotateY();
        ReturnToInitialPosition();
        StartPhantomWalk();

        CallIncreaseTime();
        CountDown();
        if(canReset == true)
            ResetAll();
    }

    void Init()
    {

        recorder = GetComponent<Recorder>();
        initialMoveSpeed = moveSpeed;
    }
   
    public void Move()
    {
        if (playerRef == null) return;

        Vector3 _moveDirection = playerRef.Move.ReadValue<Vector3>();
        if(_moveDirection.z > 0 || _moveDirection.z < 0)
            transform.position += transform.forward * _moveDirection.z * Time.deltaTime * moveSpeed;
        if(_moveDirection.x > 0 || _moveDirection.x < 0)
            transform.position += transform.right * _moveDirection.x * Time.deltaTime * moveSpeed;
        
    }

    public void RotateX()
    {
        if (playerRef == null) return;
        float _rotationValue = playerRef.RotateX.ReadValue<float>();
        transform.eulerAngles += transform.up*  _rotationValue * Time.deltaTime * rotateSpeed;


    }
    public void RotateY()
    {
        //if (playerRef == null) return;
        //float _rotationValue = playerRef.RotateY.ReadValue<float>();
        //transform.eulerAngles += transform.right*  _rotationValue * Time.deltaTime * rotateSpeed;
        


    }
    public void ActivateCanStartReturning(InputAction.CallbackContext _context)
    {
        // CALLED ON R KEYBIND 
        canStartReturning = true;
        MoveSpeed = moveSpeed;
    }
    public void ReturnToInitialPosition()
    {
        if (canStartReturning && recorder.AllPositions.Count != 0)
        { 
            Vector3 _firstPosition = recorder.AllPositions.First(); //index0
            positionAtEndRecord = transform.position;
            transform.position = Vector3.Lerp(positionAtEndRecord, _firstPosition, Time.deltaTime * lerpSpeed);
            OnReturnedToInitialPosition.Invoke();
            
            
        }
    }
    void ActivateCanStartTimer()
    {
        //canStartPhantomWalk = !canStartPhantomWalk;
        float _distance = Vector3.Distance(recorder.AllPositions.First(), transform.position);
            //Debug.Log($"{_distance}");
        if (_distance < 2 && !canStartTimer)
        {
        canStartTimer = true;
        }
    }
    float IncreaseTime(float _current, float _max)
    {
        if(canStartTimer)
        {
            _current += Time.deltaTime;
            if (_current > _max)
            { 
            OnTick.Invoke();
            _current = 0;
            canStartTimer= false;
            return _current;
            }
        }
        return _current;
    }

    void CallIncreaseTime()
    {
        currentTime = IncreaseTime(currentTime, maxtime);
    }
    void ActivateCanStartPhantomWalk()
    {
        //canStartPhantomWalk = !canStartPhantomWalk;
        canStartPhantomWalk = true;

    }
    void StartPhantomWalk()
    {
       // Debug.LogError($"PHANTOM MOVE START");
        if (canStartPhantomWalk && recorder.AllPositions.Count > 0)
        {
            Vector3 _newFirstPos = recorder.AllPositions.First();
            Quaternion _newFirstRot = recorder.AllRotations.First();

            transform.position = _newFirstPos;  //replace with slerp
            transform.rotation = _newFirstRot;  //replace with lerp
            if (loopRecord == true)
            { 
            recorder.AllPositions.Add(_newFirstPos);  // for looping the sequence
            recorder.AllRotations.Add(_newFirstRot);
                
            }
            recorder.AllPositions.Remove(_newFirstPos);
            recorder.AllRotations.Remove(_newFirstRot);

        }

            //ATTEMPT WITH LERP
            //Debug.LogError($"PHANTOM MOVE START");
            //if (canStartPhantomWalk && recorder.AllPositions.Count > 0)
            //{
            //    //Vector3 _newFirstPos = recorder.AllPositions.First();  // working one 

            //    Vector3 _newFirstPos = recorder.AllPositions[currentIndex];
            //    //Quaternion _newFirstRot = recorder.AllRotations.First();
            //    positionAtStartPhantomWalk = transform.position;

            //    transform.position = Vector3.Lerp(positionAtStartPhantomWalk, _newFirstPos, Time.deltaTime * phantomLerpSpeed);
            //    float _distance = Vector3.Distance(positionAtStartPhantomWalk, _newFirstPos);
            //    Debug.Log($"DISTANCE{_distance}");
            //    if (_distance < minDistanceToContinuePhantomWalk)
            //    {
            //        Vector3 _newStartPosition = transform.position;
            //        positionAtStartPhantomWalk = _newStartPosition;
            //        currentIndex++;
            //        minDistanceToContinuePhantomWalk++;
            //        Debug.Log($"INDEX{currentIndex}");

            //    }

            //}


    }

    void CountDown()
    {
        if (currentTime >= 1)
            Debug.LogError("1");
        if(currentTime >= 2)
            Debug.LogError("2");
        else 
            Debug.LogError("3");
    }

    void ResetAll()
    { 
        canStartTimer = false;
        canStartReturning = false;
        canStartPhantomWalk = false;
        moveSpeed = initialMoveSpeed;
        recorder.AllPositions.Clear();
        canReset = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 position in recorder.AllPositions)
        {
            if(recorder.AllPositions.Count > 0)
            Gizmos.DrawWireSphere(position, 0.1f);
            if (canStartPhantomWalk)
                Gizmos.color = Color.green;
        }
    }









}
