using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject BallPrefab;
    public float SpawnTime = 2.0f;

    private float mStartTime;

    // Start is called before the first frame update
    void Start()
    {
        mStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - mStartTime >= SpawnTime)
        {
            Instantiate(BallPrefab, transform.position, transform.rotation);
            mStartTime = Time.time;
        }
    }
}
