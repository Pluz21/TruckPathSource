using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.InputSystem;

public class Recorder : baseComp
{
    [SerializeField] float currentTime = 0;
    [SerializeField] float maxtime = 0.1f;
    [SerializeField] bool canStartRecording = false;


    [SerializeField] List<float> allWayPoints;
    [SerializeField] List<Vector3> allPositions;
    [SerializeField] Vector3 currentDebugPosition;
    [SerializeField] List<Quaternion> allRotations;
    private RecordPosition currentRecordPosition;

    public List<Vector3> AllPositions => allPositions;  // surely only need this one 
    public List<Quaternion> AllRotations => allRotations;
    public List<float> AllWayPoints => allWayPoints;
    public event Action OnTick;


    public struct RecordPosition
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;
            public RecordPosition(Vector3 _pos, Quaternion _rotation, float _time)
            {
                position = _pos;
                rotation = _rotation;
                time = _time;

            }
        public struct RecordForwardVector
        {
            public Vector3 forward;
            public RecordForwardVector(Vector3 _fwd)
            { 
                forward = _fwd;
            }
        }

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerRef.StartRecording.performed += ActivateCanStartRecording;
        OnTick += CustomTick;


    }

    public void ActivateCanStartRecording(InputAction.CallbackContext context)
    {
        //if(canStartRecording == false)
        //canStartRecording = true;
        //else 
        //    canStartRecording = false;
        //canStartRecording = canStartRecording == false ? true : false;
        canStartRecording = !canStartRecording;  // toggle 
        
        Debug.LogError($"Record has been set to : {canStartRecording}");

    }

    void CustomTick()
    {
        RecordAll();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = IncreaseTime(currentTime,maxtime);


    }
    float IncreaseTime(float _current, float _max)
    {
        _current += Time.deltaTime; // 0 = 0 + delta
        if (_current > _max)
        {
            _current = 0;
            OnTick.Invoke();
            Debug.Log("TICK");
            return _current;

        }
        return _current;

    }

    public void RecordAll()
    {
        if (canStartRecording)
        { 
        currentRecordPosition = new RecordPosition(transform.position, transform.rotation, Time.deltaTime);
        //name should be changed
        RecordPosition _currentRecordPosition = new RecordPosition(transform.position, transform.rotation, Time.deltaTime);
        allPositions.Add(_currentRecordPosition.position);
        currentDebugPosition = _currentRecordPosition.position;
        allRotations.Add(_currentRecordPosition.rotation);
        allWayPoints.Add(_currentRecordPosition.time);
        //Debug.Log($"Position: {currentRecordPosition.position}, Rotation: {currentRecordPosition.rotation}, Time: {currentRecordPosition.time}");
        }

    }

    public void RemoveLastPosition(Vector3 _lastPos)
    {
        allPositions.Remove(_lastPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(currentDebugPosition, 0.5f);
    }
}
