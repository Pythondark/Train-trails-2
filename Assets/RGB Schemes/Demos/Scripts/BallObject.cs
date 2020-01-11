using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    public float DeathTime = 5.0f;

    private float mStartTime;

    // Start is called before the first frame update
    void Start()
    {
        mStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - mStartTime > DeathTime)
        {
            Destroy(gameObject);
        }
    }
}
