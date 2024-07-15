using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceForcePlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ForcePlay();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Source Time = "+source.time);
            Debug.Log("Source Time Samples = " + source.timeSamples);

            Debug.Log("Clip Lenght = " + clip.length);
            Debug.Log("Source Clip Lenght = " + source.clip.length);            
        }
    }
    void ForcePlay()
    {
        if (!source.clip)
            source.clip = clip;

        source.Play();
    }
}
