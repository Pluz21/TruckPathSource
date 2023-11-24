using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackComponent : baseComp
{
    // Start is called before the first frame update
    [SerializeField] Projectile projectileToSpawn = null;
    [SerializeField] float currentTime = 0;
    [SerializeField] float maxTime = 1;
    [SerializeField] bool canAttack = true;
    protected override void Start()
    {
        base.Start();
        playerRef.Fire.performed += Attack;
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

   public void Attack(InputAction.CallbackContext _context)
    {
        if (playerRef == null) return;
        Debug.Log("firing");
    }

    float IncreaseTime(float _current, float _max)
    {
        _current += _current * Time.deltaTime;
        if (_current > _max) 
        { _current = 0;
            return _current;
        }
        return _current;
    }

    void SpawnProjectile()
    { 
    
    }

    
}
