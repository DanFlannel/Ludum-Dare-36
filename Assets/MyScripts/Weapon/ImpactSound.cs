using UnityEngine;
using System.Collections;

public class ImpactSound : MonoBehaviour {

    public AudioClip clip;

    // Use this for initialization
    void Start()
    {
        AudioSource asource = this.GetComponent<AudioSource>();
        asource.PlayOneShot(clip);
    }
}
