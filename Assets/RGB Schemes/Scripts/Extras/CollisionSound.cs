using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSound : MonoBehaviour
{
    public bool RequireTags;
    public List<string> CollisionTags = new List<string>();

    private AudioSource mAudioSource;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!RequireTags || CollisionTags.Contains(collision.gameObject.tag))
        {
            mAudioSource.Play();
        }
    }
}
