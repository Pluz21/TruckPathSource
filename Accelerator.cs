using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : baseComp
{
    // Start is called before the first frame update
    [SerializeField] bool hasSetSpeed = false;
    [SerializeField] float accelerationRate = 3f;
    protected override void Start()
    {
        base.Start();
        playerRef = FindObjectOfType<Truck>();
    }

    // Update is called once per frame
    void Update()
    {
        Accelerate();
    }
    

    void Accelerate()
    {
        float _dist = Vector3.Distance(transform.position, playerRef.transform.position);
        Debug.Log($"distance to platform: {_dist}");
        if (_dist < 4 && hasSetSpeed == false && playerRef.Movement.MoveSpeed <playerRef.Movement.MaxMoveSpeed)
        { 
            playerRef.Movement.MoveSpeed += accelerationRate;
            hasSetSpeed = true;
        Debug.Log("SETSPEED OK");
        }

    }
}
