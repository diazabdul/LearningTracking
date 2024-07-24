using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMic : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
        void Awake()
        {
            Microphone.Init();
            Microphone.QueryAudioInput();
        }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        void Update()
        {
            Microphone.Update();
        }
#endif

}
