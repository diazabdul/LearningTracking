using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MicInput : MonoBehaviour
{
    [SerializeField] Image image;
    public string microphoneDevice;
    public AudioSource audioSource;
    public int sampleRate = 44100;
    public float recordingDuration = 10.0f; // Duration in seconds
    public float updateInterval = 0.1f; // Interval in seconds to update volume level

    private AudioClip recordedClip;
    private int sampleWindow = 1024; // Number of samples to analyze
    private float[] samples;
    private float nextUpdate;
    float dbValue;
    private void Start()
    {
        // If no microphone device is specified, use the default microphone
        if (string.IsNullOrEmpty(microphoneDevice))
        {
            microphoneDevice = Microphone.devices[1];
        }

        // Start recording from the microphone
        recordedClip = Microphone.Start(microphoneDevice, true, (int)recordingDuration, sampleRate);

        // Initialize the samples array
        samples = new float[sampleWindow];

        // Wait until the recording has started
        while (!(Microphone.GetPosition(microphoneDevice) > 0)) { }

        Debug.Log("Recording started");

        // Set the recorded clip to the AudioSource
        audioSource.clip = recordedClip;
    }

    private void Update()
    {
        // Update the volume level at the specified interval
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + updateInterval;
            TrackVolume();
        }

        // Play the audio when the recording is finished
        if (!Microphone.IsRecording(microphoneDevice) && Microphone.GetPosition(microphoneDevice) == 0)
        {
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Playing recorded audio");
            }
        }
        if (Microphone.IsRecording(microphoneDevice))
        {
            float tempDb = dbValue / 2;
            float value = Mathf.Clamp(dbValue, 1, 2.5f);

            image.rectTransform.DOScale(value, .01f);
        }
    }

    private void TrackVolume()
    {
        // Get the current microphone position
        int micPosition = Microphone.GetPosition(microphoneDevice);
        if (micPosition < sampleWindow) return;

        // Get samples from the audio clip
        recordedClip.GetData(samples, micPosition - sampleWindow);

        // Calculate RMS value
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += samples[i] * samples[i];
        }
        float rmsValue = Mathf.Sqrt(sum / sampleWindow);

        // Calculate the volume in decibels (dB)
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);
        if (dbValue < -80) dbValue = -80; // Clamp to -80dB

        Debug.Log("Volume: " + dbValue + " dB");
    }

    private void OnDisable()
    {
        // Stop the microphone recording when the script is disabled
        Microphone.End(microphoneDevice);
    }
}

