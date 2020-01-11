using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGuns : MonoBehaviour
{
    public GameObject spawnLocation;
    public GameObject gunPrefab;
    //[SerializeField] AudioClip testAudioClip;

    public void SpawnGun()
    {
        GameObject gun;
        gun = Instantiate(gunPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
        //GetComponent<AudioSource>().PlayOneShot(testAudioClip);
    }
}
