using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecording : MonoBehaviour
{
    public string microphoneDevice;
    public AudioSource audioSource;
    public int sampleRate = 44100;
    public float maxRecordDuration = 60; // Duration in seconds

    private AudioClip recordedClip;

    [SerializeField] string[] microphoneDevices;

    [SerializeField] int minFreq;
    [SerializeField] int maxFreq;
    [SerializeField] int postionDevice;
    [SerializeField] int LastPosition;
    [SerializeField]
    float[] floatValue;
    private void OnValidate()
    {
        if(microphoneDevices.Length == 0)
        {
            microphoneDevices = Microphone.devices;
        }
    }
    private void Start()
    {
        // If no microphone device is specified, use the default microphone
        if (string.IsNullOrEmpty(microphoneDevice))
        {
            microphoneDevice = Microphone.devices[1];
        }

        // Start recording from the microphone
        //recordedClip = Microphone.Start(microphoneDevice, false, (int)recordingDuration, sampleRate);        
        // Wait until the recording has started
        //while (!(Microphone.GetPosition(microphoneDevice) > 0)) { }

        Debug.Log("Recording started");

        // Set the recorded clip to the AudioSource

    }

    private void Update()
    {
        /* // Play the audio when the recording is finished
         if (!Microphone.IsRecording(microphoneDevice) && Microphone.GetPosition(microphoneDevice) == 0)
         {
             if (audioSource.clip != null && !audioSource.isPlaying)
             {
                 audioSource.Play();
                 Debug.Log("Playing recorded audio");
             }
         }*/
        Microphone.GetDeviceCaps(microphoneDevice, out minFreq, out maxFreq);
        
        postionDevice =  Microphone.GetPosition(microphoneDevice);
        //Debug.Log();

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            floatValue = new float[128];
            recordedClip.GetData(floatValue, sampleRate);
        }

    }

    private void OnDisable()
    {
        // Stop the microphone recording when the script is disabled
        Microphone.End(microphoneDevice);
    }
    public void StartRecord()
    {
        recordedClip = Microphone.Start(microphoneDevice, false, (int)maxRecordDuration, sampleRate);

        
    }
    public void StopRecord()
    {
        Microphone.End(microphoneDevice);
        LastPosition = postionDevice;
    }
    public void PlayRecord()
    {
        audioSource.clip = recordedClip;
        audioSource.Play();
    }
}
