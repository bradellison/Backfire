using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip worldHitSound;
    public AudioClip bulletHitSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitWorld() {
        audioSource.PlayOneShot(worldHitSound);
    }

    public void HitPlayer() {
        audioSource.PlayOneShot(bulletHitSound);
    }

}
