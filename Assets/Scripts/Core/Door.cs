using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{    
    [SerializeField] int doorKeyCode = 0001;
    [SerializeField] bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (locked)
    //     {
    //        GetComponent<Rigidbody>().constraints.
    //    }
    //}

    public void Lock()
    {
        locked = true;
    }

    public void UnLock()
    {
        locked = false;
    }

}
