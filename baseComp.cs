using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class baseComp : MonoBehaviour
{
    [SerializeField] public Truck playerRef = null;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerRef = GetComponent<Truck>();
        if (playerRef == null)
        {
            Debug.LogError("Failed to find truck Component");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
